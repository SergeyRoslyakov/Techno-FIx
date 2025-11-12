using Microsoft.AspNetCore.Mvc;
using Techno_FIx.Models.DTOs;
using Techno_FIx.Services;

namespace Techno_Fix.Controllers
{
    /// <summary>
    /// Контроллер для управления клиентами
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        /// <summary>
        /// Получить список всех клиентов
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientDTO>>> GetClients()
        {
            var clients = await _clientService.GetAllClientsAsync();
            return Ok(clients);
        }

        /// <summary>
        /// Получить клиента по ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDTO>> GetClient(int id)
        {
            var client = await _clientService.GetClientByIdAsync(id);
            if (client == null)
            {
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    detail = $"Клиент с ID {id} не найден.",
                    instance = $"/api/clients/{id}"
                });
            }

            return Ok(client);
        }

        /// <summary>
        /// Создать нового клиента
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ClientDTO>> CreateClient(CreateClientDTO clientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    title = "Bad Request",
                    status = 400,
                    detail = "Ошибки валидации",
                    errors = ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                });
            }

            var client = await _clientService.CreateClientAsync(clientDto);
            return CreatedAtAction(nameof(GetClient), new { id = client.Id }, client);
        }

        /// <summary>
        /// Обновить данные клиента
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ClientDTO>> UpdateClient(int id, UpdateClientDTO clientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    title = "Bad Request",
                    status = 400,
                    detail = "Ошибки валидации",
                    errors = ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                });
            }

            var client = await _clientService.UpdateClientAsync(id, clientDto);
            if (client == null)
            {
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    detail = $"Клиент с ID {id} не найден.",
                    instance = $"/api/clients/{id}"
                });
            }

            return Ok(client);
        }

        /// <summary>
        /// Удалить клиента
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var result = await _clientService.DeleteClientAsync(id);
            if (!result)
            {
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    detail = $"Клиент с ID {id} не найден.",
                    instance = $"/api/clients/{id}"
                });
            }

            return NoContent();
        }

        /// <summary>
        /// Получить клиентов с количеством устройств
        /// </summary>
        [HttpGet("with-devices")]
        public async Task<ActionResult<IEnumerable<ClientDTO>>> GetClientsWithDeviceCount()
        {
            var clients = await _clientService.GetClientsWithDeviceCountAsync();
            return Ok(clients);
        }
    }
}
