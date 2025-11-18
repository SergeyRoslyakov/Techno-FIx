using Microsoft.AspNetCore.Mvc;
using Techno_Fix.Services;
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

        /// <summary>
        /// Получить список всех техников
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TechnicianDTO>>> GetTechnicians()
        {
            var technicians = await _technicianService.GetAllTechniciansAsync();
            return Ok(technicians);
        }

        /// <summary>
        /// Получить техника по ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<TechnicianDTO>> GetTechnician(int id)
        {
            var technician = await _technicianService.GetTechnicianByIdAsync(id);
            if (technician == null)
            {
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    detail = $"Техник с ID {id} не найден.",
                    instance = $"/api/technicians/{id}"
                });
            }

            return Ok(technician);
        }

        /// <summary>
        /// Создать нового техника
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<TechnicianDTO>> CreateTechnician(CreateTechnicianDTO technicianDto)
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

            var technician = await _technicianService.CreateTechnicianAsync(technicianDto);
            return CreatedAtAction(nameof(GetTechnician), new { id = technician.Id }, technician);
        }

        /// <summary>
        /// Обновить данные техника
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<TechnicianDTO>> UpdateTechnician(int id, UpdateTechnicianDTO technicianDto)
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

            var technician = await _technicianService.UpdateTechnicianAsync(id, technicianDto);
            if (technician == null)
            {
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    detail = $"Техник с ID {id} не найден.",
                    instance = $"/api/technicians/{id}"
                });
            }

            return Ok(technician);
        }

        /// <summary>
        /// Удалить техника
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTechnician(int id)
        {
            var result = await _technicianService.DeleteTechnicianAsync(id);
            if (!result)
            {
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    detail = $"Техник с ID {id} не найден.",
                    instance = $"/api/technicians/{id}"
                });
            }

            return NoContent();
        }

        /// <summary>
        /// Получить активных техников
        /// </summary>
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<TechnicianDTO>>> GetActiveTechnicians()
        {
            var technicians = await _technicianService.GetActiveTechniciansAsync();
            return Ok(technicians);
        }

        /// <summary>
        /// Получить техников по специализации
        /// </summary>
        [HttpGet("specialization/{specialization}")]
        public async Task<ActionResult<IEnumerable<TechnicianDTO>>> GetTechniciansBySpecialization(string specialization)
        {
            var technicians = await _technicianService.GetTechniciansBySpecializationAsync(specialization);
            return Ok(technicians);
        }

        /// <summary>
        /// Получить заказы техника
        /// </summary>
        [HttpGet("{id}/orders")]
        public async Task<ActionResult<IEnumerable<RepairOrderDTO>>> GetTechnicianOrders(int id)
        {
            var orders = await _technicianService.GetTechnicianOrdersAsync(id);
            return Ok(orders);
        }
    }
}