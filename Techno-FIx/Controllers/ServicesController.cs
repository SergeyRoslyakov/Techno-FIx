using Microsoft.AspNetCore.Mvc;
using Techno_Fix.Services;
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

        /// <summary>
        /// Получить список всех услуг
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceDTO>>> GetServices()
        {
            var services = await _serviceService.GetAllServicesAsync();
            return Ok(services);
        }

        /// <summary>
        /// Получить услугу по ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceDTO>> GetService(int id)
        {
            var service = await _serviceService.GetServiceByIdAsync(id);
            if (service == null)
            {
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    detail = $"Услуга с ID {id} не найдена.",
                    instance = $"/api/services/{id}"
                });
            }

            return Ok(service);
        }

        /// <summary>
        /// Создать новую услугу
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ServiceDTO>> CreateService(CreateServiceDTO serviceDto)
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

            var service = await _serviceService.CreateServiceAsync(serviceDto);
            return CreatedAtAction(nameof(GetService), new { id = service.Id }, service);
        }

        /// <summary>
        /// Обновить данные услуги
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceDTO>> UpdateService(int id, CreateServiceDTO serviceDto)
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

            var service = await _serviceService.UpdateServiceAsync(id, serviceDto);
            if (service == null)
            {
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    detail = $"Услуга с ID {id} не найдена.",
                    instance = $"/api/services/{id}"
                });
            }

            return Ok(service);
        }

        /// <summary>
        /// Удалить услугу
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var result = await _serviceService.DeleteServiceAsync(id);
            if (!result)
            {
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    detail = $"Услуга с ID {id} не найдена.",
                    instance = $"/api/services/{id}"
                });
            }

            return NoContent();
        }

        /// <summary>
        /// Получить популярные услуги (с наибольшим количеством заказов)
        /// </summary>
        [HttpGet("popular")]
        public async Task<ActionResult<IEnumerable<ServiceDTO>>> GetPopularServices()
        {
            var services = await _serviceService.GetPopularServicesAsync();
            return Ok(services);
        }

        /// <summary>
        /// Получить услуги по диапазону цен
        /// </summary>
        [HttpGet("price-range")]
        public async Task<ActionResult<IEnumerable<ServiceDTO>>> GetServicesByPriceRange(
            [FromQuery] decimal minPrice,
            [FromQuery] decimal maxPrice)
        {
            var services = await _serviceService.GetServicesByPriceRangeAsync(minPrice, maxPrice);
            return Ok(services);
        }
    }
}