using Microsoft.AspNetCore.Mvc;
using Techno_FIx.Models;
using Techno_FIx.Models.DTOs;
using Techno_FIx.Repositories;

namespace Techno_Fix.Services
{
    /// <summary>
    /// Сервис для работы с заказами на ремонт
    /// </summary>
    public class RepairOrderService : IRepairOrderService
    {
        private readonly IRepairOrderRepository _orderRepository;
        private readonly IDeviceRepository _deviceRepository;
        private readonly IMapper _mapper;

        public RepairOrderService(IRepairOrderRepository orderRepository,
                               IDeviceRepository deviceRepository,
                               IMapper mapper)
        {
            _orderRepository = orderRepository;
            _deviceRepository = deviceRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Получить все заказы с деталями
        /// </summary>
        public async Task<IEnumerable<RepairOrderDTO>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetRepairOrdersWithDetailsAsync();
            return _mapper.Map<IEnumerable<RepairOrderDTO>>(orders);
        }

        /// <summary>
        /// Получить заказ по ID с деталями
        /// </summary>
        public async Task<RepairOrderDTO?> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetRepairOrderWithDetailsAsync(id);
            return _mapper.Map<RepairOrderDTO>(order);
        }

        /// <summary>
        /// Создать новый заказ на ремонт
        /// </summary>
        public async Task<RepairOrderDTO> CreateOrderAsync(CreateRepairOrderDTO orderDto)
        {
            var device = await _deviceRepository.GetByIdAsync(orderDto.DeviceId);
            if (device == null)
                throw new ArgumentException("Device not found");

            var order = _mapper.Map<RepairOrder>(orderDto);
            order.CreatedDate = DateTime.UtcNow;
            order.Status = "Received";

            var createdOrder = await _orderRepository.CreateAsync(order);
            var orderWithDetails = await _orderRepository.GetRepairOrderWithDetailsAsync(createdOrder.Id);

            return _mapper.Map<RepairOrderDTO>(orderWithDetails);
        }

        /// <summary>
        /// Обновить статус заказа
        /// </summary>
        public async Task<RepairOrderDTO?> UpdateOrderStatusAsync(int id, UpdateRepairOrderDTO orderDto)
        {
            var existingOrder = await _orderRepository.GetByIdAsync(id);
            if (existingOrder == null)
                return null;

            _mapper.Map(orderDto, existingOrder);

            if (orderDto.Status == "Completed")
                existingOrder.CompletedDate = DateTime.UtcNow;

            var updatedOrder = await _orderRepository.UpdateAsync(existingOrder);
            var orderWithDetails = await _orderRepository.GetRepairOrderWithDetailsAsync(updatedOrder.Id);

            return _mapper.Map<RepairOrderDTO>(orderWithDetails);
        }

        /// <summary>
        /// Удалить заказ на ремонт
        /// </summary>
        public async Task<bool> DeleteOrderAsync(int id)
        {
            return await _orderRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Получить заказы по статусу
        /// </summary>
        public async Task<IEnumerable<RepairOrderDTO>> GetOrdersByStatusAsync(string status)
        {
            var orders = await _orderRepository.GetRepairOrdersByStatusAsync(status);
            return _mapper.Map<IEnumerable<RepairOrderDTO>>(orders);
        }

        /// <summary>
        /// Получить статистику по заказам
        /// </summary>
        public async Task<object> GetOrdersStatisticsAsync()
        {
            var orders = await _orderRepository.GetAllAsync();

            return new
            {
                TotalOrders = orders.Count(),
                CompletedOrders = orders.Count(o => o.Status == "Completed"),
                InProgressOrders = orders.Count(o => o.Status == "InProgress"),
                TotalRevenue = orders.Where(o => o.Status == "Completed").Sum(o => o.TotalCost)
            };
        }
    }
}
