using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Techno_Fix.Services;
using Techno_FIx.Models;
using Techno_FIx.Models.DTOs;
using Techno_FIx.Services;

namespace Techno_Fix.Controllers
{
    /// <summary>
    /// Контроллер для управления техниками сервисного центра
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TechniciansController : ControllerBase
    {
        private readonly ITechnicianService _technicianService;

        public TechniciansController(ITechnicianService technicianService)
        {
            _technicianService = technicianService;
        }

        [HttpGet]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Technician},{UserRoles.User}")]
        public async Task<ActionResult<IEnumerable<TechnicianDTO>>> GetTechnicians()
        {
            var technicians = await _technicianService.GetAllTechniciansAsync();
            return Ok(technicians);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Technician},{UserRoles.User}")]
        public async Task<ActionResult<TechnicianDTO>> GetTechnician(int id)
        {
            var technician = await _technicianService.GetTechnicianByIdAsync(id);
            if (technician == null)
            {
                return NotFound(new { message = $"Техник с ID {id} не найден" });
            }
            return Ok(technician);
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult<TechnicianDTO>> CreateTechnician(CreateTechnicianDTO technicianDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdTechnician = await _technicianService.CreateTechnicianAsync(technicianDto);
                return CreatedAtAction(nameof(GetTechnician), new { id = createdTechnician.Id }, createdTechnician);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> UpdateTechnician(int id, UpdateTechnicianDTO technicianDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updated = await _technicianService.UpdateTechnicianAsync(id, technicianDto);
                if (!updated)
                {
                    return NotFound(new { message = $"Техник с ID {id} не найден" });
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
        public async Task<IActionResult> DeleteTechnician(int id)
        {
            var deleted = await _technicianService.DeleteTechnicianAsync(id);
            if (!deleted)
            {
                return NotFound(new { message = $"Техник с ID {id} не найден" });
            }
            return NoContent();
        }

        [HttpGet("active")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Technician}")]
        public async Task<ActionResult<IEnumerable<TechnicianDTO>>> GetActiveTechnicians()
        {
            var activeTechnicians = await _technicianService.GetActiveTechniciansAsync();
            return Ok(activeTechnicians);
        }

        [HttpGet("specialization/{specialization}")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Technician},{UserRoles.User}")]
        public async Task<ActionResult<IEnumerable<TechnicianDTO>>> GetTechniciansBySpecialization(string specialization)
        {
            var technicians = await _technicianService.GetTechniciansBySpecializationAsync(specialization);
            return Ok(technicians);
        }

        [HttpGet("{id}/orders")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Technician}")]
        public async Task<ActionResult<IEnumerable<RepairOrderDTO>>> GetTechnicianOrders(int id)
        {
            var orders = await _technicianService.GetTechnicianOrdersAsync(id);
            return Ok(orders);
        }
    }
}