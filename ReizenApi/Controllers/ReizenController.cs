using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Reizen.Data.Models;
using Reizen.Domain.Services;
using Reizen.Domain.Models;
using Reis = Reizen.Data.Models.Reis;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReizenApi.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class ReizenController(

        IReizenRepository _service, 
        IMapper _mapper,
        ILogger<ReizenController> _logger) : ControllerBase
    {
        // GET: api/<ReizenController>
        [HttpGet ("{code}")]
        public async Task<ActionResult<ICollection<ReisDTO>>> Get (string code)
        {
            try
            {
                if (string.IsNullOrEmpty (code))
                {
                    _logger.LogWarning ("Invalid value provided");
                    return BadRequest ();
                }
                var result = await _service.GetReizenMetBestemmingAsync (code);
                if (result==null || !result.Any())
                {
                    _logger.LogInformation ("No trips found");
                    return NotFound ();
                }
                var dtos = _mapper.Map<ICollection<Reis>> (result);
                return Ok (dtos);
            }
            catch (Exception ex) 
            {
                _logger.LogError (ex, "Error while fetching trips");
                return StatusCode (500, "An error occurred while processing your request");
            }
        }

        // GET api/<ReizenController>/5
        [HttpGet ("{id:int}")]
        public async Task<ActionResult<ReisDTO>> Get (int id)
        {
            try
            {
                if (id < 0)
                {
                    _logger.LogWarning ("Invalid value provided");
                    return BadRequest ();
                }
                var result = await _service.GetReisMetIdAsync (id);
                if (result is null)
                {
                    _logger.LogInformation ($"No trips found with id {id}");
                    return NotFound ();
                }
                var dto = _mapper.Map<Reis> (result);
                return Ok (dto);
            }
            catch (Exception ex)
            {
                _logger.LogError (ex, "Error fetching trip");
                return StatusCode (500, "An error occurred while processing your request");
            }
        }

        // POST api/<ReizenController>
        [HttpPost]
        //public async Task<ActionResult> Post ([FromBody] ReisDTO reisDto, BestemmingDTO bestemmingDto)
        //{
        //    try
        //    {
        //        if (reisDto is null || bestemmingDto is null)
        //        {
        //            _logger.LogWarning ("Provided value is invalid");
        //            return BadRequest ();
        //        }
        //        var reis = _mapper.Map<Reis> (reisDto);

        //        var bestemming = _mapper.Map<Bestemming> (bestemmingDto);
        //        var result = await _service.AddReisToBestemmingAsync (reis, bestemming);

        //        if (result is null)
        //        {
        //            _logger.LogError ("Error occurred while adding trip");
        //            return StatusCode (500, "An error occurred while processing your request");
        //        }
        //        var dto = _mapper.Map<ReisDTO> (result);
        //        return Ok (dto);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError ($"Error adding trip");
        //        return StatusCode (500, "An error occurred while processing your request");
        //    }
        //}

        // PUT api/<ReizenController>/5
        [HttpPut ("{id}")]
        public void Put (int id, [FromBody] string value)
        {
        }

        // DELETE api/<ReizenController>/5
        [HttpDelete ("{id}")]
        public async Task<ActionResult> Delete (int id)
        {
            try
            {
                if (id < 0)
                {
                    _logger.LogWarning ("Provided value is invalid");
                    return BadRequest ();
                }
                var existingReis = await _service.GetReisMetIdAsync (id);
                if (existingReis is null)
                {
                    _logger.LogWarning ($"Trip with id={id} not found");
                    return BadRequest ();
                }
                var result = _service.DeleteReisMetIdAsync (id);

                if (result is null)
                {
                    _logger.LogError ("Error occurred while deleting trip");
                    return StatusCode (500, "An error occurred while processing your request");
                }
                var dto = _mapper.Map<BoekingDTO> (result);
                return Ok (dto);
            }
            catch (Exception ex)
            {
                _logger.LogError ($"Error adding boeking");
                return StatusCode (500, "An error occurred while processing your request");
            }
        }
    }
}
