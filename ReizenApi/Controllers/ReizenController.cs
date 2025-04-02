using Microsoft.AspNetCore.Mvc;
using Reizen.Data.Models;
using Reizen.Data.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReizenApi.Controllers
{
    [Route ("[controller]")]
    [ApiController]
    public class ReizenController(IReizenRepository service) : ControllerBase
    {
        // GET: api/<ReizenController>
        [HttpGet ("{code}")]
        public async Task<ICollection<Reis>> Get (string code)
        {
            return await service.GetReizenMetBestemmingAsync(code);
        }

        // GET api/<ReizenController>/5
        [HttpGet ("{id:int}")]
        public string Get (int id)
        {
            return "value";
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
