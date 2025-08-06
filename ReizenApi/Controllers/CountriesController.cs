using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Reizen.Data.Models;
using Reizen.Data.Services;
using CountryDAL = Reizen.Data.Models.CountryDAL;
using Reizen.CommonClasses.DTOs;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReizenApi.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class CountriesController(

        ICountriesContinentsRepository _service, 
        IMapper _mapper,
        ILogger<CountriesController> _logger) : ControllerBase
    {
        // GET: api/<CountriesController>
        [HttpGet]
        public async Task<ActionResult> GetCountries ()
        {
            try
            {
                var result = await _service.GetCountriesAsync();
                if (!result.IsSuccessful)
                {
                    _logger.LogInformation ($"No countries found: {result.Error}");
                    return NotFound (result.Error);
                }
                var dtos = _mapper.Map<ICollection<CountryDTO>> (result.Value);
                return Ok (dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError (ex, "Error while fetching countries");
                return StatusCode (500, "An error occurred while processing your request");
            }
        }
        // GET: api/<CountriesController>/Asia

        [HttpGet ("{name}")]
        public async Task<ActionResult> GetCountriesOfContinent ( string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    _logger.LogWarning ("Invalid value provided");
                    return BadRequest ("Invalid name");
                }
                var result = await _service.GetCountriesOfContinentAsync (name);
                if (!result.IsSuccessful)
                {
                    _logger.LogInformation ($"No countries found: {result.Error}");
                    return NotFound (result.Error);
                }
                var dtos = _mapper.Map<ICollection<CountryDTO>> (result.Value);
                return Ok (dtos);
            }
            catch (Exception ex) {
                _logger.LogError (ex, "Error while fetching countries");
                return StatusCode (500, "An error occurred while processing your request");
            }
        }

        // GET: api/<CountriesController>/5
        [HttpGet ("{id:int}")]
        public async Task<ActionResult> GetCountryWithId (int id)
        {
            try
            {
                if (id < 0)
                {
                    _logger.LogWarning ("Invalid value provided");
                    return BadRequest ("Invalid Id");
                }
                var result = await _service.GetCountryWithIdAsync (id);
                if (!result.IsSuccessful)
                {
                    _logger.LogInformation ($"No countries with an id = {id} found: {result.Error}");
                    return NotFound (result.Error);
                }
                var dtos = _mapper.Map<CountryDTO?> (result.Value);
                return Ok (dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError (ex, $"Error while fetching country: {ex.Message}");
                return StatusCode (500, $"An error occurred while processing your request: {ex.Message}");
            }
        }

        // POST: api/<CountriesController>
        [HttpPost]
        public async Task<ActionResult> Post ([FromBody] CountryDTO CountryDto)
        {
            try
            {
                if (CountryDto is null)
                {
                    _logger.LogWarning ("Invalid data provided");
                    return BadRequest ("Invalid country data");
                }
                var Country = _mapper.Map<CountryDAL> (CountryDto);
                var result = await _service.AddCountryAsync (Country);

                if (!result.IsSuccessful)
                {
                    _logger.LogError ($"Error while trying to add Country: {result.Error}");
                    return StatusCode (500, $"An error occurred while processing your request");
                }
                var dto = _mapper.Map<CountryDTO> (result.Value);
                return CreatedAtAction (nameof (Post), dto);
            }
            catch (Exception ex)
            {
                _logger.LogError ($"Error while trying to add Country: {ex.Message}");
                return StatusCode (500, $"An error occurred while processing your request: {ex.Message}");
            }
        }

        // PUT: api/<CountriesController>/5
        [HttpPut ("{id}")]
        public async Task<ActionResult> Put (int id, [FromBody] CountryDTO CountryDto)
        {
            try
            {
                if (CountryDto is null || id < 0)
                {
                    _logger.LogWarning ("Invalid data provided");
                    return BadRequest ();
                }
                var existingCountry = await _service.GetCountryWithIdAsync (id);
                if (!existingCountry.IsSuccessful)
                {
                    _logger.LogWarning ($"Invalid data provided - Country with id={id} was not found: {existingCountry.Error}");
                    return NotFound (existingCountry.Error);
                }
                var Country = _mapper.Map<CountryDAL> (CountryDto);
                var result = await _service.UpdateCountryWithIdAsync (id, Country);

                if (!result.IsSuccessful)
                {
                    _logger.LogError ($"Error while updating Country: {result.Error}");
                    return StatusCode (500, "An error occurred while processing your request");
                }
                var dto = _mapper.Map<CountryDTO> (result.Value);
                return Ok (dto);
            }
            catch (Exception ex) 
            {
                _logger.LogError ($"Error while trying to update Country: {ex.Message}");
                return StatusCode (500, $"An error occurred while processing your request: {ex.Message}");
            }
        }

        // DELETE: api/<CountriesController>/5
        [HttpDelete ("{id}")]
        public async Task<ActionResult> Delete (int id)
        {
            try
            {
                if (id < 0)
                {
                    _logger.LogWarning ("Invalid data provided - id");
                    return BadRequest ();
                }
                var existingCountry = await _service.GetCountryWithIdAsync (id);
                if (!existingCountry.IsSuccessful)
                {
                    _logger.LogWarning ($"Invalid data provided - Country with id={id} was not found: {existingCountry.Error}");
                    return NotFound (existingCountry.Error) ;
                }
                var result = await _service.DeleteCountryWithIdAsync (id);

                if (!result.IsSuccessful)
                {
                    _logger.LogError ($"Error while trying to delete Country: {result.Error}");
                    return StatusCode (500, "An error occurred while processiong your request");
                }
                var dto = _mapper.Map<CountryDTO> (result.Value);
                return Ok (dto);
            }
            catch (Exception ex) 
            {
                _logger.LogError ($"Error while trying to delete Country: {ex.Message}");
                return StatusCode (500, $"An error occurred while processing your request: {ex.Message}");
            }
        }
    }
}
