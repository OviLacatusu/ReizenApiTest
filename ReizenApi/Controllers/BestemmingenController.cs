using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Reizen.Data.Models;
using Reizen.Data.Services;
using Bestemming = Reizen.Data.Models.Bestemming;
using Reizen.Domain.DTOs;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReizenApi.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class BestemmingenController (

        ILandenWerelddelenRepository _service, 
        IMapper _mapper,
        ILogger<BestemmingenController> _logger) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetAll ()
        {
            try
            {
                var result = await _service.GetBestemmingenAsync ();

                if (!result.IsSuccessful)
                {
                    _logger.LogInformation ("No destinations found");
                    return NotFound (result.Error);
                }
                var dtos = _mapper.Map<ICollection<Bestemming>> (result.Value);
                return Ok (dtos);
            }
            catch (Exception ex) {
                _logger.LogError (ex, $"Error while fetching destinations: {ex.Message}");
                return StatusCode (500, $"An error occurred while processing your request: {ex.Message}");
            }
        }
        // GET: <BestemmingenController>
        [HttpGet ("{landNaam}")]
        public async Task<ActionResult> GetByCountry (string landNaam)
        {
            try
            {
                if (string.IsNullOrEmpty(landNaam))
                {
                    _logger.LogWarning ("Invalid country name provided");
                    return BadRequest ();
                }
                var result = await _service.GetBestemmingenVanLandAsync (landNaam);

                if (!result.IsSuccessful)
                {
                    _logger.LogInformation ("No destinations found for country: {CountryName}", landNaam);
                    return NotFound ();
                }
                var dtos = _mapper.Map<ICollection<Bestemming>> (result.Value);
                return Ok (dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError (ex, "Error occurred while fetching destinations for country: {CountryName}", landNaam);
                return StatusCode (500, $"An error occurred while processing your request");
            }
        }

        // POST <BestemmingenController>
        [HttpPost]
        public async Task<ActionResult> Post ([FromBody] BestemmingDTO bestemmingDto)
        {
            try
            {
                if (bestemmingDto == null)
                {
                    _logger.LogWarning ("Invalid data provided");
                    return BadRequest ("Invalid data");
                }
                var bestemming = _mapper.Map<Bestemming> (bestemmingDto);
                var result = await _service.AddBestemmingAsync (bestemming);

                if (!result.IsSuccessful)
                {
                    _logger.LogError ($"Error while trying to add destination");
                    return StatusCode (500, $"An error occurred while processing your request");
                }
                var createdDto = _mapper.Map<BestemmingDTO> (result.Value);
                return CreatedAtAction (nameof (GetByCountry), new { landNaam = createdDto.Land.Naam  }, createdDto);
            }
            catch (Exception ex) 
            {
                _logger.LogError ("Error while trying to add destination");
                return StatusCode (500, "An error occurred while processing your request");
            }
        }

        // Won't be implemented
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
