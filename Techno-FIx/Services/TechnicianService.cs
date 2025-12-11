using Techno_FIx.Models;
using Techno_FIx.Models.DTOs;
using Techno_FIx.Repositories;
using Techno_FIx.Services;

namespace Techno_Fix.Services
{
    /// <summary>
    /// Сервис для работы с техниками
    /// </summary>
    public class TechnicianService : ITechnicianService
    {
        private readonly ITechnicianRepository _technicianRepository;
        private readonly IRepairOrderRepository _repairOrderRepository;

        public TechnicianService(ITechnicianRepository technicianRepository, IRepairOrderRepository repairOrderRepository)
        {
            _technicianRepository = technicianRepository;
            _repairOrderRepository = repairOrderRepository;
        }

        public async Task<IEnumerable<TechnicianDTO>> GetAllTechniciansAsync()
        {
            var technicians = await _technicianRepository.GetAllAsync();
            return technicians.Select(MapToTechnicianDTO);
        }

        public async Task<TechnicianDTO?> GetTechnicianByIdAsync(int id)
        {
            var technician = await _technicianRepository.GetByIdAsync(id);
            return technician == null ? null : MapToTechnicianDTO(technician);
        }

        public async Task<TechnicianDTO> CreateTechnicianAsync(CreateTechnicianDTO technicianDto)
        {
            var technician = new Technician
            {
                FirstName = technicianDto.FirstName,
                LastName = technicianDto.LastName,
                Specialization = technicianDto.Specialization,
                Phone = technicianDto.Phone
            };

            var created = await _technicianRepository.CreateAsync(technician);
            return MapToTechnicianDTO(created);
        }

        public async Task<bool> UpdateTechnicianAsync(int id, UpdateTechnicianDTO technicianDto)
        {
            var existing = await _technicianRepository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.FirstName = technicianDto.FirstName;
            existing.LastName = technicianDto.LastName;
            existing.Specialization = technicianDto.Specialization;
            existing.Phone = technicianDto.Phone;

            await _technicianRepository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteTechnicianAsync(int id)
        {
            return await _technicianRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<TechnicianDTO>> GetActiveTechniciansAsync()
        {
            var technicians = await _technicianRepository.GetAllAsync();
            var activeTechnicians = technicians.Where(t => t.RepairOrders?.Any() == true);
            return activeTechnicians.Select(MapToTechnicianDTO);
        }

        public async Task<IEnumerable<TechnicianDTO>> GetTechniciansBySpecializationAsync(string specialization)
        {
            var technicians = await _technicianRepository.GetAllAsync();
            var specializedTechnicians = technicians
                .Where(t => t.Specialization.Contains(specialization, StringComparison.OrdinalIgnoreCase));
            return specializedTechnicians.Select(MapToTechnicianDTO);
        }

        public async Task<IEnumerable<RepairOrderDTO>> GetTechnicianOrdersAsync(int technicianId)
        {
            var orders = await _repairOrderRepository.GetAllAsync();
            var technicianOrders = orders.Where(o => o.TechnicianId == technicianId);
            return technicianOrders.Select(MapToRepairOrderDTO);
        }

        private TechnicianDTO MapToTechnicianDTO(Technician technician)
        {
            return new TechnicianDTO
            {
                Id = technician.Id,
                FirstName = technician.FirstName,
                LastName = technician.LastName,
                Specialization = technician.Specialization,
                Phone = technician.Phone,
                ActiveOrdersCount = technician.RepairOrders?.Count ?? 0
            };
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