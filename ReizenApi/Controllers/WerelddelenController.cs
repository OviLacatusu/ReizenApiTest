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
    public class WerelddelenController(ILandenWerelddelenRepository service, IMapper mapper) : ControllerBase
    {
        // GET: api/<Werelddeel>
        [HttpGet]
        public async Task<ActionResult<ICollection<WerelddeelDTO>?>> GetWerelddelenAsync ()
        {
            var result = await service.GetWerelddelenAsync ();
            if (result is null)
            {
                return NotFound ();
            }
            var dtos = mapper.Map<ICollection<WerelddeelDTO>> (result);
            return Ok (dtos);
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
