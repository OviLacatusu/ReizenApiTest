using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Reizen.Data.Models;
using Reizen.Data.Repositories;
using Reizen.Domain.Models;
using Reizen.Domain.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReizenApi.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class KlantenController(
        IKlantenRepository _service, 
        IMapper _mapper,
        ILogger<KlantenController> _logger): ControllerBase
    {
        // GET: <ValuesController>
        [HttpGet]
        public async Task<ActionResult<ICollection<KlantDTO>?>> GetKlantenAsync ()
        {
            try
            {
                var result = await _service.GetKlantenAsync ();
                if (result == null || !result.Any ())
                {
                    _logger.LogInformation ("No clients found");
                    return NotFound ();
                }
                var dtos = _mapper.Map<ICollection<KlantDTO>> (result);
                return Ok (dtos);
            }
            catch (Exception ex) 
            {
                _logger.LogError (ex, "Error while fetching clients");
                return StatusCode (500, ex);
            }
        }
        // GET: <ValuesController>/van
        [HttpGet ("{naam}")]
        public async Task<ActionResult<ICollection<KlantDTO>?>> GetMetNaam (string naam)
        {
            try
            {
                if (string.IsNullOrEmpty(naam))
                {
                    _logger.LogWarning ("Invalid name provided");
                    return BadRequest ();
                }
                var result = await _service.GetKlantenMetNaamAsync (naam);

                if (result == null || !result.Any())
                {
                    _logger.LogInformation ("Clients not found");
                    return NotFound ();
                }
                var dtos = _mapper.Map<ICollection<KlantDTO>> (result);
                return Ok (dtos);
            }
            catch (Exception ex) 
            {
                _logger.LogError (ex, $"Error while fetching clients with {naam}");
                return StatusCode (500, ex);
            }
            
        }
        // GET <ValuesController>/5
        [HttpGet ("{id:int}")]
        public async Task<ActionResult<KlantDTO?>> GetMetId (int id)
        {
            try
            {
                if (id < 0)
                {
                    _logger.LogWarning ("Invalid id");
                    return BadRequest ();
                }
                var result = await _service.GetKlantMetIdAsync (id);
                if (result is null)
                {
                    _logger.LogInformation ("Client not found");
                    return NotFound ();
                }
                var dto = _mapper.Map<KlantDTO> (result);
                return Ok (dto);
            }
            catch (Exception ex) 
            {
                _logger.LogError (ex, $"Error while fetching client with id {id}");
                return StatusCode (500, ex);
            }
        }

        // POST <ValuesController>
        [HttpPost]
        public async Task<ActionResult> Post ([FromBody] KlantDTO klantDto)
        {
            try
            {
                if (klantDto is null)
                {
                    _logger.LogWarning ("Invalid data provided");
                    return BadRequest ();
                }
                var klant = _mapper.Map<Klant> (klantDto);
                var result = await _service.AddKlantAsync (klant);

                if (result is null)
                {
                    _logger.LogError ("Error while trying to add Client");
                    return StatusCode (500, "An error occurred while processing your request");
                }
                var dto = _mapper.Map<KlantDTO> (klant);
                return CreatedAtAction(nameof(GetMetId), new{ klantVoornaam = dto.Voornaam, klantFamilinaam = dto.Familienaam }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError ("Error while trying to add Client");
                return StatusCode (500, "An error occurred while processing your request");
            }
        }

        // PUT <ValuesController>/5
        [HttpPut ("{id}")]
        public void Put (int id, [FromBody] string value)
        {
        }

        // DELETE <ValuesController>/5
        [HttpDelete ("{id}")]
        public void Delete (int id)
        {
        }
    }
}
