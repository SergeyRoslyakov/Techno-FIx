using Microsoft.AspNetCore.Mvc;
using Techno_FIx.Models;
using Techno_FIx.Models.DTOs;
using Techno_FIx.Repositories;
using AutoMapper;

namespace Techno_Fix.Services
{
    /// <summary>
    /// Сервис для работы с заказами на ремонт
    /// </summary>
    public class RepairOrderService : IRepairOrderService
    {
        private readonly IRepairOrderRepository _repairOrderRepository;

        public RepairOrderService(IRepairOrderRepository repairOrderRepository)
        {
            _repairOrderRepository = repairOrderRepository;
        }

        public async Task<IEnumerable<RepairOrderDTO>> GetAllRepairOrdersAsync()
        {
            var orders = await _repairOrderRepository.GetAllAsync();
            return orders.Select(MapToRepairOrderDTO);
        }

        public async Task<RepairOrderDTO?> GetRepairOrderByIdAsync(int id)
        {
            var order = await _repairOrderRepository.GetByIdAsync(id);
            return order == null ? null : MapToRepairOrderDTO(order);
        }

        public async Task<RepairOrderDTO> CreateRepairOrderAsync(CreateRepairOrderDTO repairOrderDto)
        {
            var repairOrder = new RepairOrder
            {
                DeviceId = repairOrderDto.DeviceId,
                ServiceId = repairOrderDto.ServiceId,
                TechnicianId = repairOrderDto.TechnicianId,
                TotalCost = 0,
                Status = "Received",
                CreatedDate = DateTime.UtcNow
            };

            var created = await _repairOrderRepository.CreateAsync(repairOrder);
            return MapToRepairOrderDTO(created);
        }

        public async Task<bool> UpdateRepairOrderAsync(int id, UpdateRepairOrderDTO repairOrderDto)
        {
            var existing = await _repairOrderRepository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.Status = repairOrderDto.Status;
            existing.TotalCost = repairOrderDto.TotalCost;

            if (repairOrderDto.Status == "Completed")
                existing.CompletedDate = DateTime.UtcNow;

            await _repairOrderRepository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteRepairOrderAsync(int id)
        {
            return await _repairOrderRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<RepairOrderDTO>> GetOrdersByTechnicianAsync(int technicianId)
        {
            var orders = await _repairOrderRepository.GetAllAsync();
            var technicianOrders = orders.Where(o => o.TechnicianId == technicianId);
            return technicianOrders.Select(MapToRepairOrderDTO);
        }

        public async Task<IEnumerable<RepairOrderDTO>> GetOrdersByClientAsync(int clientId)
        {
            var orders = await _repairOrderRepository.GetAllAsync();
            var clientOrders = orders.Where(o => o.Device.ClientId == clientId);
            return clientOrders.Select(MapToRepairOrderDTO);
        }

        private RepairOrderDTO MapToRepairOrderDTO(RepairOrder order)
        {
            return new RepairOrderDTO
            {
                Id = order.Id,
                CreatedDate = order.CreatedDate,
                CompletedDate = order.CompletedDate,
                TotalCost = order.TotalCost,
                Status = order.Status,
                DeviceInfo = $"{order.Device?.Brand} {order.Device?.Model}",
                ServiceName = order.Service?.Name ?? "",
                TechnicianName = $"{order.Technician?.FirstName} {order.Technician?.LastName}",
                ClientName = $"{order.Device?.Client?.FirstName} {order.Device?.Client?.LastName}"
            };
        }
    }
}