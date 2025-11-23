using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Techno_Fix.Services;
using Techno_FIx.Models.DTOs;

namespace Techno_Fix.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TechniciansController : ControllerBase
    {
        private readonly ITechnicianService _technicianService;
        private readonly ILogger<TechniciansController> _logger;

        public TechniciansController(ITechnicianService technicianService, ILogger<TechniciansController> logger)
        {
            _technicianService = technicianService;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<TechnicianDTO>>> GetTechnicians()
        {
            var technicians = await _technicianService.GetAllTechniciansAsync();
            return Ok(technicians);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<TechnicianDTO>> GetTechnician(int id)
        {
            var technician = await _technicianService.GetTechnicianByIdAsync(id);
            if (technician == null)
                return NotFound(new { message = "Техник не найден" });

            return Ok(technician);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<TechnicianDTO>> CreateTechnician(CreateTechnicianDTO technicianDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var technician = await _technicianService.CreateTechnicianAsync(technicianDto);
            return CreatedAtAction(nameof(GetTechnician), new { id = technician.Id }, technician);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Technician")]
        public async Task<IActionResult> UpdateTechnician(int id, UpdateTechnicianDTO technicianDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Техник может обновлять только свои данные
            if (User.IsInRole("Technician"))
            {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userTechnicianId = int.Parse(User.FindFirst("technicianId")?.Value ?? "0");

                if (userTechnicianId != id)
                {
                    return Forbid("Вы можете обновлять только свой профиль");
                }
            }

            var updated = await _technicianService.UpdateTechnicianAsync(id, technicianDto);
            if (updated == null) 
                return NotFound(new { message = "Техник не найден" });

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTechnician(int id)
        {
            var deleted = await _technicianService.DeleteTechnicianAsync(id);
            if (!deleted)
                return NotFound(new { message = "Техник не найден" });

            return NoContent();
        }
    }
}