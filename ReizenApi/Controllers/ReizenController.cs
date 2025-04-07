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
    public class ReizenController(IReizenRepository service, IMapper mapper) : ControllerBase
    {
        // GET: api/<ReizenController>
        [HttpGet ("{code}")]
        public async Task<ActionResult<ICollection<ReisDTO>>> Get (string code)
        {
            if (code == null || code == "")
            {
                return BadRequest ();
            }
            var result = await service.GetReizenMetBestemmingAsync (code);
            if (result.Count == 0)
            {
                return NotFound ();
            }
            var dtos = mapper.Map<ICollection<Reis>>(result);
            return Ok (dtos);
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
