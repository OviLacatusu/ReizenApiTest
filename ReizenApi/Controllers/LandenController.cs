using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Reizen.Data.Models;
using Reizen.Data.Repositories;
using Reizen.Domain.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReizenApi.Controllers
{
    [Route ("[controller]")]
    [ApiController]
    public class LandenController(ILandenWerelddelenRepository service, IMapper mapper) : ControllerBase
    {
        // GET: <LandenController>

        [HttpGet ("{naam}")]
        public async Task<ActionResult<ICollection<LandDTO>>> Get ( string naam)
        {
            if (naam == null || naam == "")
            {
                return BadRequest ();
            }
            var result = await service.GetLandenVanWerelddeelAsync (naam);
            if (result.Count == 0)
            {
                return NotFound ();
            }
            var dtos = mapper.Map<ICollection<Land>>(result);
            return Ok (dtos);
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
