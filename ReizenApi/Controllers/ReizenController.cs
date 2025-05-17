using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Reizen.Data.Models;
using Reizen.Data.Repositories;
using Reizen.Domain.Models;

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
                if (id <= 0)
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
        public void Post ([FromBody] string value)
        {
        }

        // PUT api/<ReizenController>/5
        [HttpPut ("{id}")]
        public void Put (int id, [FromBody] string value)
        {
        }

        // DELETE api/<ReizenController>/5
        [HttpDelete ("{id}")]
        public void Delete (int id)
        {
        }
    }
}
