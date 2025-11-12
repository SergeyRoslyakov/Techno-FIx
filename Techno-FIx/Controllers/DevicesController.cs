using Microsoft.AspNetCore.Mvc;
using Techno_Fix.Services;
using Techno_FIx.Models.DTOs;

namespace Techno_Fix.Controllers
{
    /// <summary>
    /// Контроллер для управления устройствами
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        public DevicesController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        /// <summary>
        /// Получить список всех устройств
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeviceDTO>>> GetDevices()
        {
            var devices = await _deviceService.GetAllDevicesAsync();
            return Ok(devices);
        }

        /// <summary>
        /// Получить устройство по ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<DeviceDTO>> GetDevice(int id)
        {
            var device = await _deviceService.GetDeviceByIdAsync(id);
            if (device == null)
            {
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    detail = $"Устройство с ID {id} не найдено.",
                    instance = $"/api/devices/{id}"
                });
            }

            return Ok(device);
        }

        /// <summary>
        /// Создать новое устройство
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<DeviceDTO>> CreateDevice(CreateDeviceDTO deviceDto)
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

            var device = await _deviceService.CreateDeviceAsync(deviceDto);
            return CreatedAtAction(nameof(GetDevice), new { id = device.Id }, device);
        }

        /// <summary>
        /// Обновить данные устройства
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<DeviceDTO>> UpdateDevice(int id, CreateDeviceDTO deviceDto)
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

            var device = await _deviceService.UpdateDeviceAsync(id, deviceDto);
            if (device == null)
            {
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    detail = $"Устройство с ID {id} не найдено.",
                    instance = $"/api/devices/{id}"
                });
            }

            return Ok(device);
        }

        /// <summary>
        /// Удалить устройство
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            var result = await _deviceService.DeleteDeviceAsync(id);
            if (!result)
            {
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    detail = $"Устройство с ID {id} не найдено.",
                    instance = $"/api/devices/{id}"
                });
            }

            return NoContent();
        }

        /// <summary>
        /// Получить устройства по клиенту
        /// </summary>
        [HttpGet("client/{clientId}")]
        public async Task<ActionResult<IEnumerable<DeviceDTO>>> GetDevicesByClient(int clientId)
        {
            var devices = await _deviceService.GetDevicesByClientAsync(clientId);
            return Ok(devices);
        }
    }
}
