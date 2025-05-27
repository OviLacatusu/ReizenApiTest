using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Reizen.Data.Models;
using Reizen.Data.Services;
using Werelddeel = Reizen.Data.Models.Werelddeel;
using Reizen.Domain.DTOs;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReizenApi.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class WerelddelenController(

        ILandenWerelddelenRepository _service, 
        IMapper _mapper,
        ILogger<WerelddelenController> _logger) : ControllerBase
    {
        // GET: api/<Werelddeel>
        [HttpGet]
        public async Task<ActionResult> GetWerelddelenAsync ()
        {
            try
            {
                var result = await _service.GetWerelddelenAsync ();
                if (!result.IsSuccessful)
                {
                    _logger.LogInformation ($"No continents found: {result.Error}");
                    return NotFound ();
                }
                var dtos = _mapper.Map<ICollection<WerelddeelDTO>> (result.Value);
                return Ok (dtos);
            }
            catch (Exception ex) {
                _logger.LogError (ex, "Error while fetching destinations");
                return StatusCode (500, "An error occurred while processing your request");
            }
        }

        // GET api/<Werelddeel>/5
        [HttpGet ("{id}")]
        public string Get (int id)
        {
            return "value";
        }

        // POST api/<Werelddeel>
        [HttpPost]
        public void Post ([FromBody] string value)
        {
        }

        // PUT api/<Werelddeel>/5
        [HttpPut ("{id}")]
        public void Put (int id, [FromBody] string value)
        {
        }

        // DELETE api/<Werelddeel>/5
        [HttpDelete ("{id}")]
        public void Delete (int id)
        {
        }
    }
}
