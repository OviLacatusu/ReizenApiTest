using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Reizen.Data.Models;
using Reizen.Data.Services;
using ContinentDAL = Reizen.Data.Models.ContinentDAL;
using Reizen.CommonClasses.DTOs;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReizenApi.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class ContinentsController(

        ICountriesContinentsRepository _service, 
        IMapper _mapper,
        ILogger<ContinentsController> _logger) : ControllerBase
    {
        // GET: api/<Continent>
        [HttpGet]
        public async Task<ActionResult> GetContinentsAsync ()
        {
            try
            {
                var result = await _service.GetContinentsAsync ();
                if (!result.IsSuccessful)
                {
                    _logger.LogInformation ($"No continents found: {result.Error}");
                    return NotFound (result.Error);
                }
                var dtos = _mapper.Map<ICollection<ContinentDTO>> (result.Value);
                return Ok (dtos);
            }
            catch (Exception ex) {
                _logger.LogError (ex, $"Error while fetching destinationsc: {ex.Message}");
                return StatusCode (500, $"An error occurred while processing your request");
            }
        }

        // GET: api/<ContinentsController>/Europa
        [HttpGet ("{continentName}")]
        public async Task<ActionResult> GetCountriesAsync (string continentName)
        {
            try
            {
                if (string.IsNullOrEmpty (continentName))
                {
                    _logger.LogWarning ("Invalid continent name provided");
                    return BadRequest ();
                }
                var result = await _service.GetCountriesOfContinentAsync (continentName);

                if (!result.IsSuccessful)
                {
                    _logger.LogInformation ($"No countries found for continent: {continentName}" );
                    return NotFound ();
                }
                var dtos = _mapper.Map<ICollection<CountryDTO>> (result.Value);
                return Ok (dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError (ex, $"Error occurred while fetching countries for continent: {continentName}");
                return StatusCode (500, $"An error occurred while processing your request");
            }
        }

        // POST: api/<ContinentsController>
        [HttpPost]
        public void Post ([FromBody] string value)
        {
        }

        // PUT: api/<ContinentsController>/5
        [HttpPut ("{id}")]
        public void Put (int id, [FromBody] string value)
        {
        }

        // DELETE: api/<ContinentsController>/5
        [HttpDelete ("{id}")]
        public void Delete (int id)
        {
        }
    }
}
