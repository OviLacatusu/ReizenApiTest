using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Reizen.Data.Models;
using Reizen.Data.Repositories;
using Reizen.Domain.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReizenWebBlazor.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class LandenController(
        ILandenWerelddelenRepository _service, 
        IMapper _mapper,
        ILogger<LandenController> _logger) : ControllerBase
    {
        // GET: <LandenController>

        [HttpGet ("{naam}")]
        public async Task<ActionResult<ICollection<LandDTO>>> GetVanWerelddeel ( string naam)
        {
            try
            {
                if (string.IsNullOrEmpty(naam))
                {
                    _logger.LogWarning ("Invalid value provided");
                    return BadRequest ();
                }
                var result = await _service.GetLandenVanWerelddeelAsync (naam);
                if (result == null || !result.Any())
                {
                    _logger.LogInformation ("No countries found");
                    return NotFound ();
                }
                var dtos = _mapper.Map<ICollection<Land>> (result);
                return Ok (dtos);
            }
            catch (Exception ex) {
                _logger.LogError (ex, "Error while fetching countries");
                return StatusCode (500, "An error occurred while processing your request");
            }
        }

        // GET <LandenController>/5
        [HttpGet ("{id:int}")]
        public string Get (int id)
        {
            return "value";
        }

        // POST <LandenController>
        [HttpPost]
        public void Post ([FromBody] string value)
        {
        }

        // PUT <LandenController>/5
        [HttpPut ("{id}")]
        public void Put (int id, [FromBody] string value)
        {
        }

        // DELETE <LandenController>/5
        [HttpDelete ("{id}")]
        public void Delete (int id)
        {
        }
    }
}
