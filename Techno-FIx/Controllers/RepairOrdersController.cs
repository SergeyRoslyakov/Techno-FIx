using Microsoft.AspNetCore.Mvc;
using Techno_Fix.Services;
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
        private readonly IRepairOrderService _repairOrderService;

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
        /// Обновить статус заказа
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<RepairOrderDTO>> UpdateRepairOrderStatus(int id, UpdateRepairOrderDTO orderDto)
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

            var order = await _repairOrderService.UpdateOrderStatusAsync(id, orderDto);
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
        /// Получить статистику по заказам
        /// </summary>
        [HttpGet("statistics")]
        public async Task<ActionResult<object>> GetRepairOrdersStatistics()
        {
            var statistics = await _repairOrderService.GetOrdersStatisticsAsync();
            return Ok(statistics);
        }
    }
}