using Microsoft.EntityFrameworkCore;
using Techno_FIx.Data;
using Techno_FIx.Models.DTOs;
using Techno_FIx.Models;
using Techno_FIx.Repositories;

namespace Techno_Fix.Services
{
    public class RepairOrderService : IRepairOrderService
    {
        private readonly IRepairOrderRepository _repairOrderRepository;
        private readonly AppDbContext _context;

        public RepairOrderService(IRepairOrderRepository repairOrderRepository, AppDbContext context)
        {
            _repairOrderRepository = repairOrderRepository;
            _context = context;
        }

        public async Task<IEnumerable<RepairOrderDTO>> GetAllRepairOrdersAsync()
        {
            var orders = await _context.RepairOrders
                .Include(ro => ro.Device)
                .ThenInclude(d => d.Client)
                .Include(ro => ro.Service)
                .Include(ro => ro.Technician)
                .ToListAsync();

            return orders.Select(MapToRepairOrderDTO);
        }

        public async Task<RepairOrderDTO?> GetRepairOrderByIdAsync(int id)
        {
            var order = await _context.RepairOrders
                .Include(ro => ro.Device)
                .ThenInclude(d => d.Client)
                .Include(ro => ro.Service)
                .Include(ro => ro.Technician)
                .FirstOrDefaultAsync(ro => ro.Id == id);

            return order == null ? null : MapToRepairOrderDTO(order);
        }

        public async Task<RepairOrderDTO> CreateRepairOrderAsync(CreateRepairOrderDTO orderDto)
        {
            var order = new RepairOrder
            {
                DeviceId = orderDto.DeviceId,
                ServiceId = orderDto.ServiceId,
                TechnicianId = orderDto.TechnicianId,
                CreatedDate = DateTime.UtcNow,
                Status = "Received",
                TotalCost = 0
            };

            var created = await _repairOrderRepository.CreateAsync(order);
            return MapToRepairOrderDTO(created);
        }

        public async Task<bool> UpdateRepairOrderAsync(int id, UpdateRepairOrderDTO orderDto)
        {
            var existing = await _repairOrderRepository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.Status = orderDto.Status;
            existing.TotalCost = orderDto.TotalCost;

            if (orderDto.Status == "Completed")
                existing.CompletedDate = DateTime.UtcNow;

            await _repairOrderRepository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteRepairOrderAsync(int id)
        {
            return await _repairOrderRepository.DeleteAsync(id);
        }

        public async Task<bool> UpdateOrderStatusAsync(int id, string status)
        {
            var existing = await _repairOrderRepository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.Status = status;
            if (status == "Completed")
                existing.CompletedDate = DateTime.UtcNow;

            await _repairOrderRepository.UpdateAsync(existing);
            return true;
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