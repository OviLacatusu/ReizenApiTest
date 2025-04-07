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
    public class BestemmingenController (ILandenWerelddelenRepository service, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ICollection<Bestemming>>> Get ()
        {
            var result = await service.GetBestemmingenAsync ();

            if (result.Count == 0)
            {
                return NotFound ();
            }
            var dtos = mapper.Map<ICollection<Bestemming>> (result);
            return Ok (dtos);
        }
        // GET: <BestemmingenController>
        [HttpGet ("{naam}")]
        public async Task<ActionResult<ICollection<Bestemming>>> Get (string naam)
        {
            if (naam is null || naam == "")
            {
                return BadRequest ();
            }
            var result = await service.GetBestemmingenVanLandAsync (naam);

            if (result.Count == 0)
            {
                return NotFound ();
            }
            var dtos = mapper.Map<ICollection<Bestemming>> (result);
            return Ok(dtos);
        }

        // GET <BestemmingenController>/5
        [HttpGet ("{id:int}")]
        public string Get (int id)
        {
            return "value";
        }

        // POST <BestemmingenController>
        [HttpPost]
        public void Post ([FromBody] string value)
        {
        }

        // PUT <BestemmingenController>/5
        [HttpPut ("{id}")]
        public void Put (int id, [FromBody] string value)
        {
        }

        // DELETE <BestemmingenController>/5
        [HttpDelete ("{id}")]
        public void Delete (int id)
        {
        }
    }
}
