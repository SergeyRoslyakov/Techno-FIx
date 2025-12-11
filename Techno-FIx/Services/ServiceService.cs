using Techno_FIx.Models;
using Techno_FIx.Models.DTOs;
using Techno_FIx.Repositories;
using Techno_FIx.Services;

namespace Techno_Fix.Services
{
    /// <summary>
    /// Сервис для работы с услугами
    /// </summary>
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;

        public ServiceService(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<IEnumerable<ServiceDTO>> GetAllServicesAsync()
        {
            var services = await _serviceRepository.GetAllAsync();
            return services.Select(MapToServiceDTO);
        }

        public async Task<ServiceDTO?> GetServiceByIdAsync(int id)
        {
            var service = await _serviceRepository.GetByIdAsync(id);
            return service == null ? null : MapToServiceDTO(service);
        }

        public async Task<ServiceDTO> CreateServiceAsync(CreateServiceDTO serviceDto)
        {
            var service = new Service
            {
                Name = serviceDto.Name,
                Description = serviceDto.Description,
                Price = serviceDto.Price
            };

            var created = await _serviceRepository.CreateAsync(service);
            return MapToServiceDTO(created);
        }

        public async Task<bool> UpdateServiceAsync(int id, CreateServiceDTO serviceDto)
        {
            var existing = await _serviceRepository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.Name = serviceDto.Name;
            existing.Description = serviceDto.Description;
            existing.Price = serviceDto.Price;

            await _serviceRepository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteServiceAsync(int id)
        {
            return await _serviceRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ServiceDTO>> GetPopularServicesAsync(int topN = 5)
        {
            var services = await _serviceRepository.GetAllAsync();
            var popularServices = services
                .OrderByDescending(s => s.RepairOrders?.Count ?? 0)
                .Take(topN);

            return popularServices.Select(MapToServiceDTO);
        }

        private ServiceDTO MapToServiceDTO(Service service)
        {
            return new ServiceDTO
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description,
                Price = service.Price,
                OrdersCount = service.RepairOrders?.Count ?? 0
            };
        }
    }
}

