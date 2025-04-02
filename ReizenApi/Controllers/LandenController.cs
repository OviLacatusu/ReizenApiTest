using Microsoft.AspNetCore.Mvc;
using Reizen.Data.Models;
using Reizen.Data.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReizenApi.Controllers
{
    [Route ("[controller]")]
    [ApiController]
    public class LandenController(ILandenWerelddelenRepository service) : ControllerBase
    {
        // GET: api/<LandenController>
        [HttpGet ("{naam}")]
        public async Task<ICollection<Land>> Get ( string naam)
        {
            return await service.GetLandenVanWerelddeelAsync (naam);
        }

        // GET api/<LandenController>/5
        [HttpGet ("{id:int}")]
        public string Get (int id)
        {
            return "value";
        }

        // POST api/<LandenController>
        [HttpPost]
        public void Post ([FromBody] string value)
        {
        }

        // PUT api/<LandenController>/5
        [HttpPut ("{id}")]
        public void Put (int id, [FromBody] string value)
        {
        }

        // DELETE api/<LandenController>/5
        [HttpDelete ("{id}")]
        public void Delete (int id)
        {
        }
    }
}
