using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Reizen.Data.Models;
using Reizen.Data.Services;
using TripDAL = Reizen.Data.Models.TripDAL;
using DestinationDAL = Reizen.Data.Models.DestinationDAL;
using Reizen.CommonClasses.DTOs;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReizenApi.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class TripsController(

        ITripsRepository _service, 
        IMapper _mapper,
        ILogger<TripsController> _logger) : ControllerBase
    {
        // GET: api/<TripsController>/BANGK
        [HttpGet ("{destinationCode}")]
        public async Task<ActionResult> Get (string destinationCode)
        {
            try
            {
                if (string.IsNullOrEmpty (destinationCode))
                {
                    _logger.LogWarning ("Invalid value provided");
                    return BadRequest ();
                }
                var result = await _service.GetTripsToDestinationAsync (destinationCode);
                if (!result.IsSuccessful)
                {
                    _logger.LogInformation ($"No trips found: {result.Error}");
                    return NotFound (result.Error);
                }
                var dtos = _mapper.Map<ICollection<TripDTO>> (result.Value);
                return Ok (dtos);
            }
            catch (Exception ex) 
            {
                _logger.LogError (ex, $"Error while fetching trips: {ex.Message}");
                return StatusCode (500, $"An error occurred while processing your request: {ex.Message}");
            }
        }

        // GET: api/<TripsController>/5
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
                var result = await _service.GetTripWithIdAsync (id);
                if (!result.IsSuccessful)
                {
                    _logger.LogInformation ($"No trips found with id {id}: {result.Error}");
                    return NotFound (result.Error);
                }
                var dto = _mapper.Map<TripDTO> (result.Value);
                return Ok (dto);
            }
            catch (Exception ex)
            {
                _logger.LogError (ex, $"Error fetching trip: {ex.Message}");
                return StatusCode (500, $"An error occurred while processing your request: {ex.Message}");
            }
        }

        //POST: api/<TripsController>
        [HttpPost]
        public async Task<ActionResult> Post ([FromBody] (TripDTO, DestinationDTO) tripDestinationDto) // TO DO : beter name for param
        {
            try
            {
                if (tripDestinationDto.Item1 is null || tripDestinationDto.Item2 is null)
                {
                    _logger.LogWarning ("Provided value is invalid");
                    return BadRequest ();
                }
                var trip = _mapper.Map<TripDAL> (tripDestinationDto.Item1);

                var Destination = _mapper.Map<DestinationDAL> (tripDestinationDto.Item2);
                var result = await _service.AddTripToDestinationAsync (trip, Destination);

                if (!result.IsSuccessful)
                {
                    _logger.LogError ($"Error occurred while adding trip: {result.Error}");
                    return StatusCode (500, "An error occurred while processing your request");
                }
                var dto = _mapper.Map<TripDTO> (result.Value);
                return CreatedAtAction (nameof(Post), dto);
            }
            catch (Exception ex)
            {
                _logger.LogError ($"Error adding trip");
                return StatusCode (500, "An error occurred while processing your request");
            }
        }

        // PUT: api/<TripsController>/5
        [HttpPut ("{id}")]
        public void Put (int id, [FromBody] string value)
        {
        }

        // DELETE: api/<TripsController>/5
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
                var existingTrip = await _service.GetTripWithIdAsync (id);
                if (!existingTrip.IsSuccessful)
                {
                    _logger.LogWarning ($"Trip with id={id} not found: {existingTrip.Error}");
                    return NotFound (existingTrip.Error);
                }
                var result = await _service.DeleteTripWithIdAsync (id);

                if (!result.IsSuccessful)
                {
                    _logger.LogError ($"Error occurred while deleting trip: {result.Error}");
                    return StatusCode (500, $"An error occurred while processing your request");
                }
                var dto = _mapper.Map<BookingDTO> (result.Value);
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
