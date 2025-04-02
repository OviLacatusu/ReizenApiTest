using Microsoft.AspNetCore.Mvc;
using Reizen.Data.Models;
using Reizen.Data.Repositories;
using Reizen.Domain.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReizenApi.Controllers
{
    [Route ("[controller]")]
    [ApiController]
    public class KlantenController(IKlantenRepository service): ControllerBase
    {
        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<ICollection<Klant>?> GetKlantenAsync ()
        {
            return await service.GetKlantenAsync ();
        }

        [HttpGet ("{naam}")]
        public async Task<ICollection<Klant>?> GetMetNaam (string naam)
        {
            return await service.GetKlantenMetNaamAsync (naam);
        }
        // GET api/<ValuesController>/5
        [HttpGet ("{id:int}")]
        public async Task<Klant?> Get (int id)
        {
            return await service.GetKlantMetIdAsync(id);
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post ([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut ("{id}")]
        public void Put (int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete ("{id}")]
        public void Delete (int id)
        {
        }
    }
}
