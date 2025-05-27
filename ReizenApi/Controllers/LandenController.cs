using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Reizen.Data.Models;
using Reizen.Data.Services;
using Land = Reizen.Data.Models.Land;
using Reizen.Domain.DTOs;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReizenApi.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class LandenController(

        ILandenWerelddelenRepository _service, 
        IMapper _mapper,
        ILogger<LandenController> _logger) : ControllerBase
    {
        // GET: <LandenController>

        [HttpGet ("{naam}")]
        public async Task<ActionResult> GetVanWerelddeel ( string naam)
        {
            try
            {
                if (string.IsNullOrEmpty(naam))
                {
                    _logger.LogWarning ("Invalid value provided");
                    return BadRequest ();
                }
                var result = await _service.GetLandenVanWerelddeelAsync (naam);
                if (!result.IsSuccessful)
                {
                    _logger.LogInformation ($"No countries found: {result.Error}");
                    return NotFound ();
                }
                var dtos = _mapper.Map<ICollection<Land>> (result.Value);
                return Ok (dtos);
            }
            catch (Exception ex) {
                _logger.LogError (ex, "Error while fetching countries");
                return StatusCode (500, "An error occurred while processing your request");
            }
        }

        // GET <LandenController>/5
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
                var result = await _service.GetLandMetIdAsync (id);
                if (!result.IsSuccessful)
                {
                    _logger.LogInformation ($"No countries with an id = {id} found: {result.Error}");
                    return NotFound ();
                }
                var dtos = _mapper.Map<Land?> (result.Value);
                return Ok (dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError (ex, "Error while fetching country");
                return StatusCode (500, "An error occurred while processing your request");
            }
        }

        // POST <LandenController>
        [HttpPost]
        public async Task<ActionResult> Post ([FromBody] LandDTO landDto)
        {
            try
            {
                if (landDto is null)
                {
                    _logger.LogWarning ("Invalid data provided");
                    return BadRequest ();
                }
                var land = _mapper.Map<Land> (landDto);
                var result = await _service.AddLandAsync (land);

                if (!result.IsSuccessful)
                {
                    _logger.LogError ($"Error while trying to add Land: {result.Error}");
                    return StatusCode (500, "An error occurred while processing your request");
                }
                var dto = _mapper.Map<LandDTO> (land);
                return CreatedAtAction (nameof (Post), new 
                {
                    Naam = dto.Naam,
                    Werelddeel = dto.Werelddeel,
                }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError ("Error while trying to add Land");
                return StatusCode (500, "An error occurred while processing your request");
            }
        }

        // PUT <LandenController>/5
        [HttpPut ("{id}")]
        public async Task<ActionResult> Put (int id, [FromBody] LandDTO landDto)
        {
            try
            {
                if (landDto is null || id < 0)
                {
                    _logger.LogWarning ("Invalid data provided");
                    return BadRequest ();
                }
                var existingLand = await _service.GetLandMetIdAsync (id);
                if (!existingLand.IsSuccessful)
                {
                    _logger.LogWarning ($"Invalid data provided - land with id={id} was not found: {existingLand.Error}");
                    return BadRequest ();
                }
                var land = _mapper.Map<Land> (landDto);
                var result = await _service.UpdateLandMetIdAsync (id, land);

                if (!result.IsSuccessful)
                {
                    _logger.LogError ($"Error while updating Land: {result.Error}");
                    return StatusCode (500, "An error occurred while processing your request");
                }
                var dto = _mapper.Map<LandDTO> (land);
                return Ok (dto);
            }
            catch (Exception ex) 
            {
                _logger.LogError ("Error while trying to update Land");
                return StatusCode (500, "An error occurred while processing your request");
            }
        }

        // DELETE <LandenController>/5
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
                var existingLand = await _service.GetLandMetIdAsync (id);
                if (!existingLand.IsSuccessful)
                {
                    _logger.LogWarning ($"Invalid data provided - land with id={id} was not found: {existingLand.Error}");
                    return BadRequest () ;
                }
                var result = await _service.DeleteLandMetIdAsync (id);

                if (!result.IsSuccessful)
                {
                    _logger.LogError ($"Error while deleting Land: {result.Error}");
                    return StatusCode (500, "An error occurred while processiong your request");
                }
                var dto = _mapper.Map<LandDTO> (result.Value);
                return Ok (dto);
            }
            catch (Exception ex) 
            {
                _logger.LogError ("Error while trying to delete Land");
                return StatusCode (500, "An error occurred while processing your request");
            }
        }
    }
}
