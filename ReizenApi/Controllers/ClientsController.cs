using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Reizen.Data.Models;
using Reizen.Data.Services;
using ClientDAL = Reizen.Data.Models.ClientDAL;
using Reizen.CommonClasses.DTOs;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReizenApi.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class ClientsController(

        IClientsRepository _service, 
        IMapper _mapper,
        ILogger<ClientsController> _logger): ControllerBase
    {
        // GET: api/<ClientsController>
        [HttpGet]
        public async Task<ActionResult> GetClientsAsync ()
        {
            try
            {
                var result = await _service.GetClientsAsync ();
                if (!result.IsSuccessful)
                {
                    _logger.LogInformation ($"No clients found: {result.Error}");
                    return NotFound (result.Error);
                }
                var dtos = _mapper.Map<ICollection<ClientDTO>> (result.Value);
                return Ok (dtos);
            }
            catch (Exception ex) 
            {
                _logger.LogError (ex, $"Error while fetching clients: {ex.Message}");
                return StatusCode (500, $"An error occurred while processing your request: {ex.Message}");
            }
        }
        // GET: api/<ClientsController>/van
        [HttpGet ("{name}")]
        public async Task<ActionResult> GetMetName (string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    _logger.LogWarning ("Invalid name provided");
                    return BadRequest ();
                }
                var result = await _service.GetClientsWithNameAsync (name);

                if (!result.IsSuccessful)
                {
                    _logger.LogInformation ($"Clients not found: {result.Error}");
                    return NotFound (result.Error);
                }
                var dtos = _mapper.Map<ICollection<ClientDTO>> (result.Value);
                return Ok (dtos);
            }
            catch (Exception ex) 
            {
                _logger.LogError (ex, $"Error while fetching clients with {name}");
                return StatusCode (500, $"An error occurred while processing your request: {ex.Message}");
            }
            
        }
        // GET: api/<ClientsController>/5
        [HttpGet ("{id:int}")]
        public async Task<ActionResult> GetMetId (int id)
        {
            try
            {
                if (id < 0)
                {
                    _logger.LogWarning ("Invalid id");
                    return BadRequest ();
                }
                var result = await _service.GetClientWithIdAsync (id);
                if (!result.IsSuccessful)
                {
                    _logger.LogInformation ($"Client not found {result.Error}");
                    return NotFound (result.Error);
                }
                var dto = _mapper.Map<ClientDTO> (result.Value);
                return Ok (dto);
            }
            catch (Exception ex) 
            {
                _logger.LogError (ex, $"Error while fetching client with id {id}: {ex.Message}");
                return StatusCode (500, $"An error occurred while processing your request: {ex.Message}");
            }
        }

        // POST: <ClientsController>
        [HttpPost]
        public async Task<ActionResult> Post ([FromBody] ClientDTO klantDto)
        {
            try
            {
                if (klantDto is null)
                {
                    _logger.LogWarning ("Invalid data provided");
                    return BadRequest ();
                }
                var klant = _mapper.Map<ClientDAL> (klantDto);
                var result = await _service.AddClientAsync (klant);

                if (!result.IsSuccessful)
                {
                    _logger.LogError ($"Error while trying to add Client: {result.Error}");
                    return StatusCode (500, $"An error occurred while processing your request");
                }
                var dto = _mapper.Map<ClientDTO> (klant);
                return CreatedAtAction(nameof(Post), new{ klantFirstName = dto.FirstName, klantFamiliname = dto.FamilyName }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError ($"Error while trying to add Client: {ex.Message}");
                return StatusCode (500, $"An error occurred while processing your request: {ex.Message}");
            }
        }

        // PUT: api/<ClientsController>/5
        [HttpPut ("{id}")]
        public async Task<ActionResult> Put ([FromBody] ClientDTO klantDto, int id, CancellationToken token)
        {
            try
            {
                if (klantDto is null)
                {
                    _logger.LogWarning ("Invalid data provided");
                    return BadRequest ();
                }
                var klant = _mapper.Map<ClientDAL> (klantDto);
                var existingClient = await _service.GetClientWithIdAsync (id);
                
                if (!existingClient.IsSuccessful)
                {
                    _logger.LogWarning ($"Invalid data provided - client does not exist: {existingClient.Error}");
                    return NotFound (existingClient.Error);
                }
                var result = await _service.UpdateClientAsync (id, klant);
                if (!result.IsSuccessful)
                {
                    _logger.LogError ($"Error while trying to update Client: {result.Error}");
                    return StatusCode (500, $"An error occurred while processing your request:");
                }
                var dto = _mapper.Map<ClientDTO> (klant);
                return Ok (dto);
            }
            catch (Exception ex)
            {
                _logger.LogError ($"Error while trying to update Client: {ex.Message}");
                return StatusCode (500, $"An error occurred while processing request: {ex.Message}");
            }
        }

        // DELETE: api/<ClientsController>/5
        [HttpDelete ("{id}")]
        public async Task<ActionResult> Delete (int id)
        {
            try
            {
                if (id < 0)
                {
                    _logger.LogWarning ("Invalid data provided");
                    return BadRequest ();
                }
                var existingClient = await _service.GetClientWithIdAsync (id);

                if (!existingClient.IsSuccessful)
                {
                    _logger.LogWarning ($"Invalid data - client does not exist: {existingClient.Error}");
                    return NotFound (existingClient.Error);
                }
                var result = await _service.DeleteClientAsync (id);
                if (!result.IsSuccessful)
                {
                    _logger.LogError ($"Error while trying to delete customer: {result.Error}");
                    return StatusCode (500, "An error occurred while processing your request");
                }
                var dto = _mapper.Map<ClientDTO> (existingClient.Value);
                return Ok (dto);
            }
            catch (Exception ex) 
            {
                _logger.LogError ($"Error while trying to delete client: {ex.Message}");
                return StatusCode (500, $"An error occured while processing your request: {ex.Message}");
            }
        }
    }
}
