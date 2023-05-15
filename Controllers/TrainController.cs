using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Railway.Data;
using Railway.Dtos;
using Railway.Interface;
using Railway.Repository;
using System.Data;

namespace Railway.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
      public class TrainController : ControllerBase
    {
        private ITrain _trainRep;
        private IMapper _mapper;
        public TrainController(ITrain trainRep, IMapper mapper)
        {

            _trainRep = trainRep;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(ICollection<Train>))]
        [ProducesResponseType(400)]
        public IActionResult GetAllTrains()
        {
            var trains = _mapper.Map<List<TrainDto>>(_trainRep.GetTrains());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(trains);
        }

        [HttpGet("{trainId}")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(Train))]
        [ProducesResponseType(400)]
        public IActionResult GetTrain(int trainId)
        {

            if (!_trainRep.DoesItExist(trainId))
            {
                return NotFound();
            }

            var train = _mapper.Map<TrainDto>(_trainRep.GetTrain(trainId));


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(train);

        }

        [HttpPost("Create")]
        [Authorize(Roles ="Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateTrain([FromBody] TrainDto createTrain)
        {
            if (createTrain == null)
                return BadRequest(ModelState);

            var train = _trainRep.GetTrains().Where(r => r.Name.Trim().ToUpper() == createTrain.Name.TrimEnd().ToUpper()).FirstOrDefault();

            if (train != null)
            {
                ModelState.AddModelError(" ", "Train already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var rand = new Random();
            var trainMap = _mapper.Map<Train>(createTrain);
            trainMap.TrainNumber = $"GP{rand.Next(100,250)}";

            if (!_trainRep.Create(trainMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Sucessfully created");
        }

        [HttpPut("{trainId}")]
        [Authorize(Roles ="Admin,Supervisor")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateTrain(int trainId, [FromBody] TrainDto request)
        {
            if (request == null)
                return BadRequest(ModelState);

            if (!_trainRep.DoesItExist(request.Id))
                return NotFound();

            if (trainId != request.Id)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest();

            var trainMap = _mapper.Map<Train>(request);

            if (!_trainRep.Update(trainMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


        [HttpDelete("{trainId}")]
        [Authorize(Roles="Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteTrain(int trainId)
        {
            if (!_trainRep.DoesItExist(trainId))
            {
                return NotFound();
            }

            var currentTrain = _trainRep.GetTrain(trainId);


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_trainRep.Delete(currentTrain))
            {
                ModelState.AddModelError("", "An error occured, can't delete at the moment");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


    }


}
