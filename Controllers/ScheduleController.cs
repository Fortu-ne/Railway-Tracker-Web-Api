using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Railway.Data;
using Railway.DbDataContext;
using Railway.Dtos;
using Railway.Interface;
using System.Data;

namespace Railway.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private ISchedule _scheduleRep;
        private DataContext _context;
        private IMapper _mapper;
        public ScheduleController(ISchedule scheduleRep, IMapper mapper, DataContext context)
        {
            _scheduleRep = scheduleRep;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(ICollection<Schedule>))]
        [ProducesResponseType(400)]
        public IActionResult GetAllSchedule()
        {
            var schedule = _mapper.Map<List<ScheduleDto>>(_scheduleRep.GetAll());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(schedule);
        }

        [HttpGet("{trainId}/Schedule")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(ICollection<Schedule>))]
        [ProducesResponseType(400)]
        public IActionResult GetScheduleByTrain(int trainId)
        {
            var schedule = _mapper.Map<List<ScheduleDto>>(_scheduleRep.GetSchedulesByTrain(trainId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(schedule);
        }

        [HttpGet("{scheduleId}")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(Schedule))]
        [ProducesResponseType(400)]
        public IActionResult GetSchedule(int scheduleId)
        {

            if (!_scheduleRep.DoesItExist(scheduleId))
            {
                return NotFound();
            }

            var schedule = _mapper.Map<ScheduleDto>(_scheduleRep.GetSchedule(scheduleId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(schedule);

        }

        [HttpPost("Create")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateSchedule([FromBody] ScheduleDto request)
        {
            if (request == null)
                return BadRequest(ModelState);

            var arrivalTime = DateTime.Parse(request.Arrival);
            var depatureTime = DateTime.Parse(request.Depature);
            var durSpan = TimeOnly.FromDateTime(arrivalTime) - TimeOnly.FromDateTime(depatureTime);


            var schedule = _scheduleRep.GetAll().Where(r => r.Id == request.Id).FirstOrDefault();

            if (schedule != null)
            {
                ModelState.AddModelError(" ", "Train already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var insertTrain = _context.Trains.Where(r => r.Id == request.TrainId).FirstOrDefault();

            var model = new Schedule
            {

                IsDelayed = false,
                Depature = TimeOnly.FromDateTime(depatureTime),
                Arrival = TimeOnly.FromDateTime(arrivalTime),
                Duration = TimeOnly.FromTimeSpan(durSpan),
                TrainId = request.TrainId,
                Train = insertTrain,

            };
            var scheduleMap = _mapper.Map<Schedule>(model);

            scheduleMap.Arrival = TimeOnly.FromDateTime(arrivalTime);
            scheduleMap.Depature = TimeOnly.FromDateTime(depatureTime);


            if (!_scheduleRep.Create(request.TrainId, scheduleMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Sucessfully created");
        }

        [HttpPut("{scheduleId}")]
        [Authorize(Roles = "Admin,Supervisor")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateSchedule(int scheduleId, [FromBody] ScheduleDto request)
        {
            if (request == null)
                return BadRequest(ModelState);

            if (scheduleId != request.Id)
                return BadRequest(ModelState);

            if (!_scheduleRep.DoesItExist(request.Id)
                )
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var arrivalTime = DateTime.Parse(request.Arrival);
            var depatureTime = DateTime.Parse(request.Depature);
            var durSpan = TimeOnly.FromDateTime(arrivalTime) - TimeOnly.FromDateTime(depatureTime);


            //var scheduleUpdate = _scheduleRep.GetSchedule(scheduleId);
            //var trainId = scheduleUpdate.TrainId;
            var updateTrain = _context.Trains.Where(r => r.Id == request.TrainId).FirstOrDefault();

            var model = new Schedule
            {
                Id = scheduleId,
                IsDelayed = request.IsDelayed,
                Depature = TimeOnly.FromDateTime(depatureTime),
                Arrival = TimeOnly.FromDateTime(arrivalTime),
                Duration = TimeOnly.FromTimeSpan(durSpan),
                TrainId = request.TrainId,
                Train = updateTrain,

            };

            var trainModel = _context.Trains.Where(r => r.Id == request.TrainId).FirstOrDefault();

            if (request.IsDelayed == true)
            {
                var rand = new Random();
                double value = rand.Next(10, 50);
                model.Arrival = model.Arrival.AddMinutes(value);

                var dur = model.Arrival - model.Depature;
                model.Duration = TimeOnly.FromTimeSpan(dur);

                var stations = _context.Stations.Where(r => r.TrainId == trainModel.Id).ToList();

                foreach(var station in stations)
                {
                    station.ArrivalTime = station.ArrivalTime.AddMinutes(value);
                }
           
            }
            var scheduleMap = _mapper.Map<Schedule>(model);

            if (!_scheduleRep.Update(scheduleMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{scheduleId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteSchedule(int scheduleId)
        {
            if (!_scheduleRep.DoesItExist(scheduleId))
            {
                return NotFound();
            }

            var model = _scheduleRep.GetSchedule(scheduleId);


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_scheduleRep.Delete(model))
            {
                ModelState.AddModelError("", "An error occured, can't delete at the moment");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
