using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Reizen.Data.Models;
using Reizen.Data.Services;
using ReisDAL = Reizen.Data.Models.ReisDAL;
using BestemmingDAL = Reizen.Data.Models.BestemmingDAL;
using Reizen.Domain.DTOs;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReizenApi.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class ReizenController(

        IReizenRepository _service, 
        IMapper _mapper,
        ILogger<ReizenController> _logger) : ControllerBase
    {
        // GET: api/<ReizenController>
        [HttpGet ("{code}")]
        public async Task<ActionResult> Get (string code)
        {
            try
            {
                if (string.IsNullOrEmpty (code))
                {
                    _logger.LogWarning ("Invalid value provided");
                    return BadRequest ();
                }
                var result = await _service.GetReizenMetBestemmingAsync (code);
                if (!result.IsSuccessful)
                {
                    _logger.LogInformation ($"No trips found: {result.Error}");
                    return NotFound (result.Error);
                }
                var dtos = _mapper.Map<ICollection<ReisDAL>> (result.Value);
                return Ok (dtos);
            }
            catch (Exception ex) 
            {
                _logger.LogError (ex, $"Error while fetching trips: {ex.Message}");
                return StatusCode (500, $"An error occurred while processing your request: {ex.Message}");
            }
        }

        // GET api/<ReizenController>/5
        [HttpGet ("{id:int}")]
        public async Task<ActionResult> Get (int id)
        {
            try
            {
                if (id < 0)
                {
                    _logger.LogWarning ("Invalid value provided");
                    return BadRequest ();
                }
                var result = await _service.GetReisMetIdAsync (id);
                if (!result.IsSuccessful)
                {
                    _logger.LogInformation ($"No trips found with id {id}: {result.Error}");
                    return NotFound (result.Error);
                }
                var dto = _mapper.Map<ReisDAL> (result.Value);
                return Ok (dto);
            }
            catch (Exception ex)
            {
                _logger.LogError (ex, $"Error fetching trip: {ex.Message}");
                return StatusCode (500, $"An error occurred while processing your request: {ex.Message}");
            }
        }

        //POST api/<ReizenController>
        [HttpPost]
        public async Task<ActionResult> Post ([FromBody] (ReisDTO, BestemmingDTO) reisBestemmingDto)
        {
            try
            {
                if (reisBestemmingDto.Item1 is null || reisBestemmingDto.Item2 is null)
                {
                    _logger.LogWarning ("Provided value is invalid");
                    return BadRequest ();
                }
                var reis = _mapper.Map<ReisDAL> (reisBestemmingDto.Item1);

                var bestemming = _mapper.Map<BestemmingDAL> (reisBestemmingDto.Item2);
                var result = await _service.AddReisToBestemmingAsync (reis, bestemming);

                if (!result.IsSuccessful)
                {
                    _logger.LogError ($"Error occurred while adding trip: {result.Error}");
                    return StatusCode (500, "An error occurred while processing your request");
                }
                var dto = _mapper.Map<ReisDTO> (result.Value);
                return Ok (dto);
            }
            catch (Exception ex)
            {
                _logger.LogError ($"Error adding trip");
                return StatusCode (500, "An error occurred while processing your request");
            }
        }

        // PUT api/<ReizenController>/5
        [HttpPut ("{id}")]
        public void Put (int id, [FromBody] string value)
        {
        }

        // DELETE api/<ReizenController>/5
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
                var existingReis = await _service.GetReisMetIdAsync (id);
                if (!existingReis.IsSuccessful)
                {
                    _logger.LogWarning ($"Trip with id={id} not found: {existingReis.Error}");
                    return NotFound (existingReis.Error);
                }
                var result = await _service.DeleteReisMetIdAsync (id);

                if (!result.IsSuccessful)
                {
                    _logger.LogError ($"Error occurred while deleting trip: {result.Error}");
                    return StatusCode (500, $"An error occurred while processing your request");
                }
                var dto = _mapper.Map<BoekingDTO> (result.Value);
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
