using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Techno_Fix.Services;
using Techno_FIx.Models.DTOs;
using Techno_FIx.Services;

namespace Techno_Fix.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly ILogger<ClientsController> _logger;

        public ClientsController(IClientService clientService, ILogger<ClientsController> logger)
        {
            _clientService = clientService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Technician")]
        public async Task<ActionResult<IEnumerable<ClientDTO>>> GetClients()
        {
            try
            {
                _logger.LogInformation("Запрос списка клиентов пользователем: {UserName}", User.Identity?.Name);
                var clients = await _clientService.GetAllClientsAsync();
                return Ok(clients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка клиентов");
                return StatusCode(500, new { error = "Внутренняя ошибка сервера" });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Technician")]
        public async Task<ActionResult<ClientDTO>> GetClient(int id)
        {
            try
            {
                _logger.LogInformation("Запрос клиента {ClientId} пользователем: {UserName}", id, User.Identity?.Name);
                var client = await _clientService.GetClientByIdAsync(id);
                if (client == null)
                {
                    _logger.LogWarning("Клиент с ID {ClientId} не найден", id);
                    return NotFound(new { message = $"Клиент с ID {id} не найден" });
                }
                return Ok(client);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении клиента {ClientId}", id);
                return StatusCode(500, new { error = "Внутренняя ошибка сервера" });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ClientDTO>> CreateClient(CreateClientDTO clientDto)
        {
            try
            {
                _logger.LogInformation("Создание клиента пользователем: {UserName}", User.Identity?.Name);

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Невалидные данные при создании клиента");
                    return BadRequest(ModelState);
                }

                var createdClient = await _clientService.CreateClientAsync(clientDto);

                _logger.LogInformation("Клиент успешно создан с ID: {ClientId}", createdClient.Id);

                return CreatedAtAction(nameof(GetClient), new { id = createdClient.Id }, createdClient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании клиента");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateClient(int id, UpdateClientDTO clientDto)
        {
            try
            {
                _logger.LogInformation("Обновление клиента {ClientId} пользователем: {UserName}", id, User.Identity?.Name);

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Невалидные данные при обновлении клиента {ClientId}", id);
                    return BadRequest(ModelState);
                }

                var updated = await _clientService.UpdateClientAsync(id, clientDto);
                if (updated == null)
                {
                    _logger.LogWarning("Клиент с ID {ClientId} не найден для обновления", id);
                    return NotFound(new { message = $"Клиент с ID {id} не найден" });
                }

                _logger.LogInformation("Клиент {ClientId} успешно обновлен", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении клиента {ClientId}", id);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            try
            {
                var userName = User.Identity?.Name;
                var userRoles = string.Join(", ", User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value));

                _logger.LogInformation("Удаление клиента {ClientId} пользователем: {UserName} с ролями: {Roles}",
                    id, userName, userRoles);

                var deleted = await _clientService.DeleteClientAsync(id);
                if (!deleted)
                {
                    _logger.LogWarning("Клиент с ID {ClientId} не найден для удаления", id);
                    return NotFound(new { message = $"Клиент с ID {id} не найден" });
                }

                _logger.LogInformation("Клиент {ClientId} успешно удален пользователем {UserName}", id, userName);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении клиента {ClientId}", id);
                return StatusCode(500, new { error = "Внутренняя ошибка сервера при удалении клиента" });
            }
        }
    }
}