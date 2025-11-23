using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Techno_Fix.Services;
using Techno_FIx.Models.DTOs;

namespace Techno_Fix.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceService _deviceService;
        private readonly ILogger<DevicesController> _logger;

        public DevicesController(IDeviceService deviceService, ILogger<DevicesController> logger)
        {
            _deviceService = deviceService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Technician")]
        public async Task<ActionResult<IEnumerable<DeviceDTO>>> GetDevices()
        {
            var devices = await _deviceService.GetAllDevicesAsync();
            return Ok(devices);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Technician")]
        public async Task<ActionResult<DeviceDTO>> GetDevice(int id)
        {
            var device = await _deviceService.GetDeviceByIdAsync(id);
            if (device == null)
                return NotFound(new { message = "Устройство не найдено" });

            return Ok(device);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Technician")]
        public async Task<ActionResult<DeviceDTO>> CreateDevice(CreateDeviceDTO deviceDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var device = await _deviceService.CreateDeviceAsync(deviceDto);

            // УБРАТЬ оператор ! отсюда - строка 41
            return CreatedAtAction(nameof(GetDevice), new { id = device.Id }, device);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Technician")]
        public async Task<IActionResult> UpdateDevice(int id, CreateDeviceDTO deviceDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _deviceService.UpdateDeviceAsync(id, deviceDto);
            if (updated == null)
                return NotFound(new { message = "Устройство не найдено" });

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            var deleted = await _deviceService.DeleteDeviceAsync(id);
            if (!deleted)
                return NotFound(new { message = "Устройство не найдено" });

            return NoContent();
        }
    }
}