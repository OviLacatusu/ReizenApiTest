using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Reizen.Data.Models;
using Reizen.Data.Repositories;
using Reizen.Domain.Models;
using Reizen.Domain.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReizenApi.Controllers
{
    [Route ("[controller]")]
    [ApiController]
    public class KlantenController(IKlantenRepository service, IMapper mapper): ControllerBase
    {
        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<ICollection<KlantDTO>?>> GetKlantenAsync ()
        {
            var result = await service.GetKlantenAsync ();

            if (result == null)
            {
                return NotFound ();
            }
            var dtos = mapper.Map<ICollection<KlantDTO>> (result);

            return Ok(dtos);
        }

        [HttpGet ("{naam}")]
        public async Task<ActionResult<ICollection<KlantDTO>?>> GetMetNaam (string naam)
        {
            if (naam == "" || naam is null)
            {
                return BadRequest ();
            }
            var result = await service.GetKlantenMetNaamAsync (naam);

            if (result == null)
            {
                return NotFound ();
            }
            var dtos = mapper.Map<ICollection<KlantDTO>> (result);
            return Ok(dtos);
            
        }
        // GET api/<ValuesController>/5
        [HttpGet ("{id:int}")]
        public async Task<ActionResult<KlantDTO?>> Get (int id)
        {
            if (id < 0)
            {
                return BadRequest ();
            }
            var result = await service.GetKlantMetIdAsync (id);
            if (result is null)
            {
                return NotFound (id);
            }
            var dto = mapper.Map<KlantDTO> (result);
            return Ok (dto);
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
