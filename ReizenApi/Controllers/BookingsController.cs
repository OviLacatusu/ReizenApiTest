using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Reizen.Data.Models;
using Reizen.Data.Services;
using System.Runtime.CompilerServices;
using BookingDAL = Reizen.Data.Models.BookingDAL;
using Reizen.CommonClasses.DTOs;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReizenApi.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class BookingsController(        
        IBookingsRepository _service,
        IMapper _mapper,
        ILogger<DestinationsController> _logger) : ControllerBase
    {
        // GET: api/<BookingController>
        [HttpGet]
        public async Task<ActionResult> Get ()
        {
            try
            {
                var bookings = await _service.GetBookingsAsync ();
                if (!bookings.IsSuccessful)
                {
                    _logger.LogInformation ($"No bookings found: {bookings.Error}");
                    return NotFound (bookings.Error);
                }
                var dtos = _mapper.Map<ICollection<BookingDTO>> (bookings.Value);
                return Ok (dtos);
            }
            catch (Exception ex) 
            {
                _logger.LogError ($"Error fetching bookings: {ex.Message}");
                return StatusCode (500, $"An error occurred while processing your request: {ex.Message}");
            }
        }

        // GET: api/<BookingController>/5
        [HttpGet ("{id}")]
        public async Task<ActionResult> GetWithId (int id)
        {
            try
            {
                if (id<0)
                {
                    _logger.LogWarning ("Invalid id");
                    return BadRequest ();
                }
                var booking = await _service.GetBookingWithIdAsync (id);
                if (!booking.IsSuccessful)
                {
                    _logger.LogWarning ($"Error occurred: {booking.Error}"); 
                    return NotFound (booking.Error);
                }
                var dto = _mapper.Map<BookingDTO> (booking.Value);
                return Ok (dto);
            }
            catch (Exception ex) {
                _logger.LogError ($"Error fetching booking with id={id}: {ex.Message}");
                return StatusCode (500, $"An error occurred while processing your request: {ex.Message}");
            }
        }

        // POST: api/<BookingController>
        [HttpPost]
        public async Task<ActionResult> Post ([FromBody] BookingDTO bookingDto)
        {
            try
            {
                if (bookingDto is null)
                {
                    _logger.LogWarning ("Provided booking is null");
                    return BadRequest ();
                }
                if (bookingDto.NumberOfAdults <= 0 && bookingDto.NumberOfMinors <= 0)
                {
                    _logger.LogWarning ("Provided booking is null");
                    return BadRequest ("Please enter the number of persons");
                }
                var booking = _mapper.Map<BookingDAL> (bookingDto);
                var result = await _service.AddBookingAsync (booking);
                
                if (!result.IsSuccessful)
                {
                    _logger.LogError ($"Error occurred while adding booking: {result.Error}");
                    return StatusCode (500, "An error occurred while processing your request");
                }
                var dto = _mapper.Map <BookingDTO> (result.Value);
                return CreatedAtAction (nameof (Post), dto);
            }
            catch (Exception ex) 
            {
                _logger.LogError ($"Error adding booking: {ex.Message}");
                return StatusCode (500, $"An error occurred while processing your request: {ex.Message}");
            }
        }

        // PUT: api/<BookingController>/5
        [HttpPut ("{id}")]
        public async Task<ActionResult> Put (int id, [FromBody] BookingDTO bookingDto)
        {
            try
            {
                if (bookingDto is null || id < 0)
                {
                    _logger.LogWarning ("Provided value is invalid");
                    return BadRequest ();
                }
                var existingBooking = await _service.GetBookingWithIdAsync (id);
                if (!existingBooking.IsSuccessful)
                {
                    _logger.LogWarning ($"Booking with id={id} not found: {existingBooking.Error}");
                    return NotFound (existingBooking.Error);
                }
                var booking = _mapper.Map<BookingDAL> (bookingDto);
                var result = await _service.UpdateBookingAsync (booking, id);

                if (!result.IsSuccessful)
                {
                    _logger.LogError ($"Error occurred while updating booking: {result.Error}");
                    return StatusCode (500, $"An error occurred while processing your request");
                }
                var dto = _mapper.Map<BookingDTO> (result.Value);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError ($"Error adding booking: {ex.Message}");
                return StatusCode (500, $"An error occurred while processing your request: {ex.Message}");
            }
        }

        // DELETE: api/<BookingController>/5
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
                var existingBooking = await _service.GetBookingWithIdAsync (id);
                if (!existingBooking.IsSuccessful)
                {
                    _logger.LogWarning ($"Booking with id={id} not found");
                    return NotFound (existingBooking.Error);
                }
                var result = await _service.DeleteBookingAsync (id);

                if (!result.IsSuccessful)
                {
                    _logger.LogError ($"Error occurred while updating booking: {result.Error}");
                    return StatusCode (500, $"An error occurred while processing your request");
                }
                var dto = _mapper.Map<BookingDTO> (result);
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
