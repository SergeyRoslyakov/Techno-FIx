using Microsoft.AspNetCore.Mvc;
using Techno_FIx.Models.DTOs;

namespace Techno_Fix.Controllers
{
    /// <summary>
    /// Контроллер для управления заказами на ремонт
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RepairOrdersController : ControllerBase
    {
        public readonly IRepairOrderService _repairOrderService;

        public RepairOrdersController(IRepairOrderService repairOrderService)
        {
            _repairOrderService = repairOrderService;
        }

        /// <summary>
        /// Получить список всех заказов на ремонт
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RepairOrderDTO>>> GetRepairOrders()
        {
            var orders = await _repairOrderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        /// <summary>
        /// Получить заказ на ремонт по ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<RepairOrderDTO>> GetRepairOrder(int id)
        {
            var order = await _repairOrderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    detail = $"Заказ на ремонт с ID {id} не найден.",
                    instance = $"/api/repairorders/{id}"
                });
            }

            return Ok(order);
        }

        /// <summary>
        /// Создать новый заказ на ремонт
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<RepairOrderDTO>> CreateRepairOrder(CreateRepairOrderDTO orderDto)
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

            var order = await _repairOrderService.CreateOrderAsync(orderDto);
            return CreatedAtAction(nameof(GetRepairOrder), new { id = order.Id }, order);
        }

        /// <summary>
        /// Обновить заказ на ремонт
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<RepairOrderDTO>> UpdateRepairOrder(int id, UpdateRepairOrderDTO orderDto)
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

            var order = await _repairOrderService.UpdateOrderAsync(id, orderDto);
            if (order == null)
            {
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    detail = $"Заказ на ремонт с ID {id} не найден.",
                    instance = $"/api/repairorders/{id}"
                });
            }

            return Ok(order);
        }

        /// <summary>
        /// Удалить заказ на ремонт
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRepairOrder(int id)
        {
            var result = await _repairOrderService.DeleteOrderAsync(id);
            if (!result)
            {
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    detail = $"Заказ на ремонт с ID {id} не найден.",
                    instance = $"/api/repairorders/{id}"
                });
            }

            return NoContent();
        }

        /// <summary>
        /// Получить заказы по статусу
        /// </summary>
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<RepairOrderDTO>>> GetRepairOrdersByStatus(string status)
        {
            var orders = await _repairOrderService.GetOrdersByStatusAsync(status);
            return Ok(orders);
        }

        /// <summary>
        /// Получить заказы по технику
        /// </summary>
        [HttpGet("technician/{technicianId}")]
        public async Task<ActionResult<IEnumerable<RepairOrderDTO>>> GetRepairOrdersByTechnician(int technicianId)
        {
            var orders = await _repairOrderService.GetOrdersByTechnicianAsync(technicianId);
            return Ok(orders);
        }

        /// <summary>
        /// Получить статистику по заказам
        /// </summary>
        [HttpGet("statistics")]
        public async Task<ActionResult<object>> GetRepairOrdersStatistics()
        {
            var statistics = await _repairOrderService.GetOrdersStatisticsAsync();
            return Ok(statistics);
        }

        /// <summary>
        /// Изменить статус заказа
        /// </summary>
        [HttpPatch("{id}/status")]
        public async Task<ActionResult<RepairOrderDTO>> ChangeOrderStatus(int id, [FromBody] string status)
        {
            var order = await _repairOrderService.ChangeOrderStatusAsync(id, status);
            if (order == null)
            {
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    detail = $"Заказ на ремонт с ID {id} не найден.",
                    instance = $"/api/repairorders/{id}/status"
                });
            }

            return Ok(order);
        }
    }
}
