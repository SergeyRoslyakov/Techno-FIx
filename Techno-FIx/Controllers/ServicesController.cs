using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Techno_Fix.Services;
using Techno_FIx.Models;
using Techno_FIx.Models.DTOs;

namespace Techno_Fix.Controllers
{
    /// <summary>
    /// Контроллер для управления услугами сервисного центра
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServicesController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpGet]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Technician},{UserRoles.User}")]
        public async Task<ActionResult<IEnumerable<ServiceDTO>>> GetServices()
        {
            var services = await _serviceService.GetAllServicesAsync();
            return Ok(services);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Technician},{UserRoles.User}")]
        public async Task<ActionResult<ServiceDTO>> GetService(int id)
        {
            var service = await _serviceService.GetServiceByIdAsync(id);
            if (service == null)
            {
                return NotFound(new { message = $"Услуга с ID {id} не найдена" });
            }
            return Ok(service);
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult<ServiceDTO>> CreateService(CreateServiceDTO serviceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdService = await _serviceService.CreateServiceAsync(serviceDto);
                return CreatedAtAction(nameof(GetService), new { id = createdService.Id }, createdService);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> UpdateService(int id, CreateServiceDTO serviceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updated = await _serviceService.UpdateServiceAsync(id, serviceDto);
                if (!updated)
                {
                    return NotFound(new { message = $"Услуга с ID {id} не найдена" });
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
        public async Task<IActionResult> DeleteService(int id)
        {
            var deleted = await _serviceService.DeleteServiceAsync(id);
            if (!deleted)
            {
                return NotFound(new { message = $"Услуга с ID {id} не найдена" });
            }
            return NoContent();
        }

        [HttpGet("popular")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Technician},{UserRoles.User}")]
        public async Task<ActionResult<IEnumerable<ServiceDTO>>> GetPopularServices([FromQuery] int top = 5)
        {
            var popularServices = await _serviceService.GetPopularServicesAsync(top);
            return Ok(popularServices);
        }
    }
}