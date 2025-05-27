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
                    _logger.LogInformation ($"No boekings found: {boekingen.Error}");
                    return NotFound ();
                }
                var dtos = _mapper.Map<ICollection<Boeking>> (boekingen.Value);
                return Ok (dtos);
            }
            catch (Exception ex) 
            {
                _logger.LogError ("Error fetching Boekingen");
                return StatusCode (500, "An error occurred while processing your request");
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
                    return NotFound ();
                }
                var dto = _mapper.Map<Boeking> (boeking.Value);
                return Ok (dto);

            }
            catch (Exception ex) {
                _logger.LogError ($"Error fetching Boeking with id={id}");
                return StatusCode (500, "An error occurred while processing your request");
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
                    _logger.LogWarning ("Provided boeking is null");
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
                _logger.LogError ($"Error adding boeking");
                return StatusCode (500, "An error occurred while processing your request");
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
                    _logger.LogWarning ($"Boeking with id={id} not found: {existingBoeking.Error}");
                    return BadRequest ();
                }
                var boeking = _mapper.Map<Boeking> (boekingDto);
                var result = await _service.UpdateBoekingAsync (boeking, id);

                if (!result.IsSuccessful)
                {
                    _logger.LogError ($"Error occurred while updating boeking: {result.Error}");
                    return StatusCode (500, "An error occurred while processing your request");
                }
                var dto = _mapper.Map<BoekingDTO> (result.Value);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError ($"Error adding boeking");
                return StatusCode (500, "An error occurred while processing your request");
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
                    _logger.LogWarning ($"Boeking with id={id} not found");
                    return BadRequest ();
                }
                var result = await _service.DeleteBoekingAsync (id);

                if (result is null)
                {
                    _logger.LogError ("Error occurred while updating boeking");
                    return StatusCode (500, "An error occurred while processing your request");
                }
                var dto = _mapper.Map<BoekingDTO> (result);
                return Ok (dto);
            }
            catch (Exception ex)
            {
                _logger.LogError ($"Error adding boeking");
                return StatusCode (500, "An error occurred while processing your request");
            }
        }
    }
}
