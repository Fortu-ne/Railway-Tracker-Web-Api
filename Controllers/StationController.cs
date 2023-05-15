using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Railway.Data;
using Railway.DbDataContext;
using Railway.Dtos;
using Railway.Interface;
using Railway.Repository;

namespace Railway.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StationController : ControllerBase
    {
        private readonly IStation _stationRep;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public StationController(IStation stationRep, IMapper mapper, DataContext context)
        {
            _stationRep = stationRep;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(ICollection<Station>))]
        [ProducesResponseType(400)]
        public IActionResult GetAllStations()
        {
            var stations = _mapper.Map<List<StationDto>>(_stationRep.GetAll());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(stations);
        }

        [HttpGet("{trainId}/Stations")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(ICollection<Station>))]
        [ProducesResponseType(400)]
        public IActionResult GetStationsByTrain(int trainId)
        {
            var stations = _mapper.Map<List<StationDto>>(_stationRep.GetStationByTrain(trainId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(stations);
        }

        [HttpGet("{stationId}")]
        [ProducesResponseType(200, Type = typeof(Station))]
        [ProducesResponseType(400)]
        public IActionResult GetStation(int stationId)
        {

            if (!_stationRep.DoesItExist(stationId))
            {
                return NotFound();
            }

            var station = _mapper.Map<StationDto>(_stationRep.GetStation(stationId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(station);

        }


        [HttpPost("Create")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200, Type = typeof(StationDto))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateStation([FromBody] StationDto request)
        {
            if (request == null)
                return BadRequest(ModelState);

            var station = _stationRep.GetAll().Where(r => r.Name.Trim().ToUpper() == request.Name.TrimEnd().ToUpper()).FirstOrDefault();

            if (station != null)
            {
                ModelState.AddModelError(" ", "Train already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stopTime = DateTime.Parse(request.ArrivalTime);

            var getTrain = _context.Trains.Where(r => r.Id == request.TrainId).FirstOrDefault();
          
            var model = new Station
            {
                Name = request.Name,
                ArrivalTime = TimeOnly.FromDateTime(stopTime),
                TrainId = request.TrainId,
                Train = getTrain,
            };
            var stationMap = _mapper.Map<Station>(model);

            if (!_stationRep.Create(request.TrainId, stationMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Sucessfully created");
        }

        [HttpPut("{stationId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateStation(int stationId, [FromBody] StationDto request)
        {
            if (request == null)
                return BadRequest(ModelState);

            if (stationId != request.Id)
                return BadRequest(ModelState);

            if (!_stationRep.DoesItExist(request.Id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            
            var stopTime = DateTime.Parse(request.ArrivalTime);

            var model = new Station
            {
                Name = request.Name,
                ArrivalTime = TimeOnly.FromDateTime(stopTime),
            };

           
            var stationMap = _mapper.Map<Station>(model);


            if (!_stationRep.Update(stationMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


        [HttpDelete("{stationId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteStation(int stationId)
        {
            if (!_stationRep.DoesItExist(stationId))
            {
                return NotFound();
            }

            var model = _stationRep.GetStation(stationId);


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_stationRep.Delete(model))
            {
                ModelState.AddModelError("", "An error occured, can't delete at the moment");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
