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

        public TechnicianService(ITechnicianRepository technicianRepository,
                               IRepairOrderRepository repairOrderRepository)
        {
            _technicianRepository = technicianRepository;
            _repairOrderRepository = repairOrderRepository;
        }

        /// <summary>
        /// Получить всех техников
        /// </summary>
        public async Task<IEnumerable<TechnicianDTO>> GetAllTechniciansAsync()
        {
            var technicians = await _technicianRepository.GetAllAsync();
            return technicians.Select(t => new TechnicianDTO
            {
                Id = t.Id,
                FirstName = t.FirstName,
                LastName = t.LastName,
                Specialization = t.Specialization,
                Phone = t.Phone,
                ActiveOrdersCount = t.RepairOrders.Count
            });
        }

        /// <summary>
        /// Получить техника по ID
        /// </summary>
        public async Task<TechnicianDTO?> GetTechnicianByIdAsync(int id)
        {
            var technician = await _technicianRepository.GetByIdAsync(id);
            if (technician == null) return null;

            return new TechnicianDTO
            {
                Id = technician.Id,
                FirstName = technician.FirstName,
                LastName = technician.LastName,
                Specialization = technician.Specialization,
                Phone = technician.Phone,
                ActiveOrdersCount = technician.RepairOrders.Count
            };
        }

        /// <summary>
        /// Создать нового техника
        /// </summary>
        public async Task<TechnicianDTO> CreateTechnicianAsync(CreateTechnicianDTO technicianDto)
        {
            var technician = new Technician
            {
                FirstName = technicianDto.FirstName,
                LastName = technicianDto.LastName,
                Specialization = technicianDto.Specialization,
                Phone = technicianDto.Phone
            };

            var createdTechnician = await _technicianRepository.CreateAsync(technician);

            return new TechnicianDTO
            {
                Id = createdTechnician.Id,
                FirstName = createdTechnician.FirstName,
                LastName = createdTechnician.LastName,
                Specialization = createdTechnician.Specialization,
                Phone = createdTechnician.Phone,
                ActiveOrdersCount = 0
            };
        }

        /// <summary>
        /// Обновить данные техника
        /// </summary>
        public async Task<TechnicianDTO?> UpdateTechnicianAsync(int id, UpdateTechnicianDTO technicianDto)
        {
            var existingTechnician = await _technicianRepository.GetByIdAsync(id);
            if (existingTechnician == null) return null;

            existingTechnician.FirstName = technicianDto.FirstName;
            existingTechnician.LastName = technicianDto.LastName;
            existingTechnician.Specialization = technicianDto.Specialization;
            existingTechnician.Phone = technicianDto.Phone;

            var updatedTechnician = await _technicianRepository.UpdateAsync(existingTechnician);

            return new TechnicianDTO
            {
                Id = updatedTechnician.Id,
                FirstName = updatedTechnician.FirstName,
                LastName = updatedTechnician.LastName,
                Specialization = updatedTechnician.Specialization,
                Phone = updatedTechnician.Phone,
                ActiveOrdersCount = updatedTechnician.RepairOrders.Count
            };
        }

        /// <summary>
        /// Удалить техника
        /// </summary>
        public async Task<bool> DeleteTechnicianAsync(int id)
        {
            return await _technicianRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Получить активных техников
        /// </summary>
        public async Task<IEnumerable<TechnicianDTO>> GetActiveTechniciansAsync()
        {
            var technicians = await _technicianRepository.GetActiveTechniciansAsync();
            return technicians.Select(t => new TechnicianDTO
            {
                Id = t.Id,
                FirstName = t.FirstName,
                LastName = t.LastName,
                Specialization = t.Specialization,
                Phone = t.Phone,
                ActiveOrdersCount = t.RepairOrders.Count
            });
        }

        /// <summary>
        /// Получить техников по специализации
        /// </summary>
        public async Task<IEnumerable<TechnicianDTO>> GetTechniciansBySpecializationAsync(string specialization)
        {
            var technicians = await _technicianRepository.GetTechniciansBySpecializationAsync(specialization);
            return technicians.Select(t => new TechnicianDTO
            {
                Id = t.Id,
                FirstName = t.FirstName,
                LastName = t.LastName,
                Specialization = t.Specialization,
                Phone = t.Phone,
                ActiveOrdersCount = t.RepairOrders.Count
            });
        }

        /// <summary>
        /// Получить заказы техника
        /// </summary>
        public async Task<IEnumerable<RepairOrderDTO>> GetTechnicianOrdersAsync(int technicianId)
        {
            var orders = await _repairOrderRepository.GetRepairOrdersByTechnicianAsync(technicianId);
            return orders.Select(o => new RepairOrderDTO
            {
                Id = o.Id,
                CreatedDate = o.CreatedDate,
                CompletedDate = o.CompletedDate,
                TotalCost = o.TotalCost,
                Status = o.Status,
                DeviceInfo = $"{o.Device.Brand} {o.Device.Model}",
                ServiceName = o.Service.Name,
                TechnicianName = $"{o.Technician.FirstName} {o.Technician.LastName}",
                ClientName = $"{o.Device.Client.FirstName} {o.Device.Client.LastName}"
            });
        }
    }
}
