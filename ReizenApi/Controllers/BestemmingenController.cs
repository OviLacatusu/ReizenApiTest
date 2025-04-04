using Microsoft.AspNetCore.Mvc;
using Reizen.Data.Models;
using Reizen.Data.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReizenApi.Controllers
{
    [Route ("[controller]")]
    [ApiController]
    public class BestemmingenController (ILandenWerelddelenRepository service) : ControllerBase
    {
        // GET: api/<BestemmingenController>
        [HttpGet ("{naam}")]
        public async Task<ActionResult<ICollection<Bestemming>>> Get (string naam)
        {
            if (naam is null || naam == "")
            {
                return BadRequest ();
            }
            var result = service.GetBestemmingenVanLandAsync (naam);

            if (result == null)
            {
                return NotFound ();
            }
            return Ok(result);

        }

        // GET api/<BestemmingenController>/5
        [HttpGet ("{id:int}")]
        public string Get (int id)
        {
            return "value";
        }

        // POST api/<BestemmingenController>
        [HttpPost]
        public void Post ([FromBody] string value)
        {
        }

        // PUT api/<BestemmingenController>/5
        [HttpPut ("{id}")]
        public void Put (int id, [FromBody] string value)
        {
        }

        // DELETE api/<BestemmingenController>/5
        [HttpDelete ("{id}")]
        public void Delete (int id)
        {
        }
    }
}
