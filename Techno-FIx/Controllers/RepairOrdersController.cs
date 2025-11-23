using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Techno_Fix.Services;
using Techno_FIx.Models.DTOs;
using Techno_FIx.Models;

namespace Techno_Fix.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RepairOrdersController : ControllerBase
    {
        private readonly IRepairOrderService _repairOrderService;
        private readonly ILogger<RepairOrdersController> _logger;

        public RepairOrdersController(IRepairOrderService repairOrderService, ILogger<RepairOrdersController> logger)
        {
            _repairOrderService = repairOrderService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Technician}")]
        public async Task<ActionResult<IEnumerable<RepairOrderDTO>>> GetRepairOrders()
        {
            var orders = await _repairOrderService.GetAllRepairOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("my-orders")]
        [Authorize(Roles = UserRoles.User)] // Только User может видеть свои заказы
        public async Task<ActionResult<IEnumerable<RepairOrderDTO>>> GetMyOrders()
        {
            // Временное решение - возвращаем все заказы
            // В реальном приложении нужно фильтровать по пользователю
            var orders = await _repairOrderService.GetAllRepairOrdersAsync();
            return Ok(orders.Take(3)); // Возвращаем только 3 заказа для примера
        }

        [HttpGet("{id}")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Technician},{UserRoles.User}")]
        public async Task<ActionResult<RepairOrderDTO>> GetRepairOrder(int id)
        {
            var order = await _repairOrderService.GetRepairOrderByIdAsync(id);
            if (order == null)
                return NotFound(new { message = "Заказ не найден" });

            return Ok(order);
        }

        [HttpPost]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Technician}")]
        public async Task<ActionResult<RepairOrderDTO>> CreateRepairOrder(CreateRepairOrderDTO orderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = await _repairOrderService.CreateRepairOrderAsync(orderDto);
            return CreatedAtAction(nameof(GetRepairOrder), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Technician}")]
        public async Task<IActionResult> UpdateRepairOrder(int id, UpdateRepairOrderDTO orderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _repairOrderService.UpdateRepairOrderAsync(id, orderDto);
            if (!updated)
                return NotFound(new { message = "Заказ не найден" });

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.Admin)] // Только админ может удалять
        public async Task<IActionResult> DeleteRepairOrder(int id)
        {
            var deleted = await _repairOrderService.DeleteRepairOrderAsync(id);
            if (!deleted)
                return NotFound(new { message = "Заказ не найден" });

            return NoContent();
        }

        [HttpPatch("{id}/status")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Technician}")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] string status)
        {
            var updated = await _repairOrderService.UpdateOrderStatusAsync(id, status);
            if (!updated)
                return NotFound(new { message = "Заказ не найден" });

            return NoContent();
        }
    }
}