using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Techno_Fix.Services;
using Techno_FIx.Models;
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
        /// Получить все заказы (доступно администраторам и техникам)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Technician}")]
        public async Task<ActionResult<IEnumerable<RepairOrderDTO>>> GetAllOrders()
        {
            var orders = await _repairOrderService.GetAllRepairOrdersAsync();
            return Ok(orders);
        }

        /// <summary>
        /// Получить мои заказы (для техников - их заказы, для пользователей - заказы их устройств)
        /// </summary>
        [HttpGet("my-orders")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Technician},{UserRoles.User}")]
        public async Task<ActionResult<IEnumerable<RepairOrderDTO>>> GetMyOrders()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole == UserRoles.Technician)
            {
                // Для техников - заказы, назначенные на них
                var technicianId = User.FindFirst("technicianId")?.Value;
                if (int.TryParse(technicianId, out int techId))
                {
                    var orders = await _repairOrderService.GetOrdersByTechnicianAsync(techId);
                    return Ok(orders);
                }
            }

            // Для пользователей - заказы их устройств
            var userOrders = await _repairOrderService.GetOrdersByClientAsync(userId);
            return Ok(userOrders);
        }

        /// <summary>
        /// Получить заказ по ID (доступно администраторам, техникам и владельцам заказа)
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Technician},{UserRoles.User}")]
        public async Task<ActionResult<RepairOrderDTO>> GetOrder(int id)
        {
            var order = await _repairOrderService.GetRepairOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound(new { message = $"Заказ с ID {id} не найден" });
            }

            // Проверка прав доступа для пользователей
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole == UserRoles.User)
            {
                // Пользователь может видеть только свои заказы
                var userOrders = await _repairOrderService.GetOrdersByClientAsync(userId);
                if (!userOrders.Any(o => o.Id == id))
                {
                    return Forbid();
                }
            }

            return Ok(order);
        }

        /// <summary>
        /// Создать новый заказ (доступно администраторам и техникам)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Technician}")]
        public async Task<ActionResult<RepairOrderDTO>> CreateOrder(CreateRepairOrderDTO orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdOrder = await _repairOrderService.CreateRepairOrderAsync(orderDto);
                return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.Id }, createdOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Обновить заказ (доступно администраторам и назначенным техникам)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Technician}")]
        public async Task<IActionResult> UpdateOrder(int id, UpdateRepairOrderDTO orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updated = await _repairOrderService.UpdateRepairOrderAsync(id, orderDto);
                if (!updated)
                {
                    return NotFound(new { message = $"Заказ с ID {id} не найден" });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Удалить заказ (доступно только администраторам)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var deleted = await _repairOrderService.DeleteRepairOrderAsync(id);
            if (!deleted)
            {
                return NotFound(new { message = $"Заказ с ID {id} не найден" });
            }
            return NoContent();
        }
    }
}