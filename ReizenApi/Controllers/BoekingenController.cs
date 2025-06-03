using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Reizen.Data.Models;
using Reizen.Data.Services;
using System.Runtime.CompilerServices;
using Boeking = Reizen.Data.Models.Boeking;
using Reizen.Domain.DTOs;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReizenApi.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class BoekingenController(        
        IBoekingenRepository _service,
        IMapper _mapper,
        ILogger<BestemmingenController> _logger) : ControllerBase
    {
        // GET: api/<BoekingController>
        [HttpGet]
        public async Task<ActionResult> Get ()
        {
            try
            {
                var boekingen = await _service.GetBoekingenAsync ();
                if (!boekingen.IsSuccessful)
                {
                    _logger.LogInformation ($"No bookings found: {boekingen.Error}");
                    return NotFound (boekingen.Error);
                }
                var dtos = _mapper.Map<ICollection<Boeking>> (boekingen.Value);
                return Ok (dtos);
            }
            catch (Exception ex) 
            {
                _logger.LogError ($"Error fetching bookings: {ex.Message}");
                return StatusCode (500, $"An error occurred while processing your request: {ex.Message}");
            }
        }

        // GET api/<BoekingController>/5
        [HttpGet ("{id}")]
        public async Task<ActionResult> GetMetId (int id)
        {
            try
            {
                if (id<0)
                {
                    _logger.LogWarning ("Invalid id");
                    return BadRequest ();
                }
                var boeking = await _service.GetBoekingMetIdAsync (id);
                if (!boeking.IsSuccessful)
                {
                    _logger.LogWarning ($"Error occurred: {boeking.Error}"); 
                    return NotFound (boeking.Error);
                }
                var dto = _mapper.Map<Boeking> (boeking.Value);
                return Ok (dto);
            }
            catch (Exception ex) {
                _logger.LogError ($"Error fetching booking with id={id}: {ex.Message}");
                return StatusCode (500, $"An error occurred while processing your request: {ex.Message}");
            }
        }

        // POST api/<BoekingController>
        [HttpPost]
        public async Task<ActionResult> Post ([FromBody] BoekingDTO boekingDto)
        {
            try
            {
                if (boekingDto is null)
                {
                    _logger.LogWarning ("Provided booking is null");
                    return BadRequest ();
                }
                var boeking = _mapper.Map<Boeking> (boekingDto);
                var result = await _service.AddBoekingAsync (boeking);
                
                if (!result.IsSuccessful)
                {
                    _logger.LogError ($"Error occurred while adding boeking: {result.Error}");
                    return StatusCode (500, "An error occurred while processing your request");
                }
                var dto = _mapper.Map <BoekingDTO> (result.Value);
                return CreatedAtAction (nameof (Post), dto);
            }
            catch (Exception ex) 
            {
                _logger.LogError ($"Error adding booking: {ex.Message}");
                return StatusCode (500, $"An error occurred while processing your request: {ex.Message}");
            }
        }

        // PUT api/<BoekingController>/5
        [HttpPut ("{id}")]
        public async Task<ActionResult> Put (int id, [FromBody] BoekingDTO boekingDto)
        {
            try
            {
                if (boekingDto is null || id < 0)
                {
                    _logger.LogWarning ("Provided value is invalid");
                    return BadRequest ();
                }
                var existingBoeking = await _service.GetBoekingMetIdAsync (id);
                if (!existingBoeking.IsSuccessful)
                {
                    _logger.LogWarning ($"Booking with id={id} not found: {existingBoeking.Error}");
                    return NotFound (existingBoeking.Error);
                }
                var boeking = _mapper.Map<Boeking> (boekingDto);
                var result = await _service.UpdateBoekingAsync (boeking, id);

                if (!result.IsSuccessful)
                {
                    _logger.LogError ($"Error occurred while updating boeking: {result.Error}");
                    return StatusCode (500, $"An error occurred while processing your request");
                }
                var dto = _mapper.Map<BoekingDTO> (result.Value);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError ($"Error adding booking: {ex.Message}");
                return StatusCode (500, $"An error occurred while processing your request: {ex.Message}");
            }
        }

        // DELETE api/<BoekingController>/5
        [HttpDelete ("{id}")]
        public async Task<ActionResult> Delete (int id)
        {
            try
            {
                if (id < 0)
                {
                    _logger.LogWarning ("Provided value is invalid");
                    return BadRequest ();
                }
                var existingBoeking = await _service.GetBoekingMetIdAsync (id);
                if (existingBoeking is null)
                {
                    _logger.LogWarning ($"Booking with id={id} not found");
                    return NotFound (existingBoeking.Error);
                }
                var result = await _service.DeleteBoekingAsync (id);

                if (result is null)
                {
                    _logger.LogError ($"Error occurred while updating booking: {result.Error}");
                    return StatusCode (500, $"An error occurred while processing your request");
                }
                var dto = _mapper.Map<BoekingDTO> (result);
                return Ok (dto);
            }
            catch (Exception ex)
            {
                _logger.LogError ($"Error adding booking: {ex.Message}");
                return StatusCode (500, $"An error occurred while processing your request: {ex.Message}");
            }
        }
    }
}
