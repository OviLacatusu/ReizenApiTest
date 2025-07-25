﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Reizen.Data.Models;
using Reizen.Data.Services;
using DestinationDAL = Reizen.Data.Models.DestinationDAL;
using Reizen.CommonClasses.DTOs;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReizenApi.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class DestinationsController (
        ICountriesContinentsRepository _service, 
        IMapper _mapper,
        ILogger<DestinationsController> _logger) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetAll ()
        {
            try
            {
                var result = await _service.GetDestinationsAsync ();

                if (!result.IsSuccessful)
                {
                    _logger.LogInformation ("No destinations found");
                    return NotFound (result.Error);
                }
                var dtos = _mapper.Map<ICollection<DestinationDTO>> (result.Value);
                return Ok (dtos);
            }
            catch (Exception ex) {
                _logger.LogError (ex, $"Error while fetching destinations: {ex.Message}");
                return StatusCode (500, $"An error occurred while processing your request: {ex.Message}");
            }
        }
        // GET: api/<DestinationsController>/Thailand
        [HttpGet ("{CountryName}")]
        public async Task<ActionResult> GetByCountry (string countryName)
        {
            try
            {
                if (string.IsNullOrEmpty(countryName))
                {
                    _logger.LogWarning ("Invalid country name provided");
                    return BadRequest ();
                }
                var result = await _service.GetDestinationsOfCountryAsync (countryName);

                if (!result.IsSuccessful)
                {
                    _logger.LogInformation ("No destinations found for country: {countryName}", countryName);
                    return NotFound ();
                }
                var dtos = _mapper.Map<ICollection<DestinationDTO>> (result.Value);
                return Ok (dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError (ex, "Error occurred while fetching destinations for country: {countryName}", countryName);
                return StatusCode (500, $"An error occurred while processing your request");
            }
        }

        // POST: api/<DestinationsController>
        [HttpPost]
        public async Task<ActionResult> Post ([FromBody] DestinationDTO destinationDto)
        {
            try
            {
                if (destinationDto == null)
                {
                    _logger.LogWarning ("Invalid data provided");
                    return BadRequest ("Invalid data");
                }
                var destination = _mapper.Map<DestinationDAL> (destinationDto);
                var result = await _service.AddDestinationAsync (destination);

                if (!result.IsSuccessful)
                {
                    _logger.LogError ($"Error while trying to add destination");
                    return StatusCode (500, $"An error occurred while processing your request");
                }
                var createdDto = _mapper.Map<DestinationDTO> (result.Value);
                return CreatedAtAction (nameof (GetByCountry), new { CountryName = createdDto.Country.Name  }, createdDto);
            }
            catch (Exception ex) 
            {
                _logger.LogError ("Error while trying to add destination");
                return StatusCode (500, "An error occurred while processing your request");
            }
        }

        // Won't be implemented
        // PUT: api/<DestinationsController>/5
        [HttpPut ("{id}")]
        public void Put (int id, [FromBody] string value)
        {
        }

        // DELETE: api/<DestinationsController>/5
        [HttpDelete ("{id}")]
        public void Delete (int id)
        {
        }
    }
}
