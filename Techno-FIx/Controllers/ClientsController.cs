using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Techno_FIx.Models;
using Techno_FIx.Models.DTOs;
using Techno_FIx.Services;

namespace Techno_Fix.Controllers
{
    /// <summary>
    /// Контроллер для управления клиентами
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Technician}")]
        public async Task<ActionResult<IEnumerable<ClientDTO>>> GetClients()
        {
            var clients = await _clientService.GetAllClientsAsync();
            return Ok(clients);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Technician}")]
        public async Task<ActionResult<ClientDTO>> GetClient(int id)
        {
            var client = await _clientService.GetClientByIdAsync(id);
            if (client == null)
            {
                return NotFound(new { message = $"Клиент с ID {id} не найден" });
            }
            return Ok(client);
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult<ClientDTO>> CreateClient(CreateClientDTO clientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdClient = await _clientService.CreateClientAsync(clientDto);
                return CreatedAtAction(nameof(GetClient), new { id = createdClient.Id }, createdClient);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> UpdateClient(int id, UpdateClientDTO clientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updated = await _clientService.UpdateClientAsync(id, clientDto);
                if (!updated)
                {
                    return NotFound(new { message = $"Клиент с ID {id} не найден" });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var deleted = await _clientService.DeleteClientAsync(id);
            if (!deleted)
            {
                return NotFound(new { message = $"Клиент с ID {id} не найден" });
            }
            return NoContent();
        }
    }
}
