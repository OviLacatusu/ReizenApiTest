using Microsoft.AspNetCore.Mvc;
using Reizen.Data.Models;
using Reizen.Data.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReizenApi.Controllers
{
    [Route ("[controller]")]
    [ApiController]
    public class WerelddelenController(ILandenWerelddelenRepository service) : ControllerBase
    {
        // GET: api/<Werelddeel>
        [HttpGet]
        public async Task<ActionResult<ICollection<Werelddeel>?>> GetWerelddelenAsync ()
        {
            var result = await service.GetWerelddelenAsync ();
            if (result is null)
            {
                return NotFound ();
            }
            return Ok (result);
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
