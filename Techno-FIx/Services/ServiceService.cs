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

        /// <summary>
        /// Получить все услуги
        /// </summary>
        public async Task<IEnumerable<ServiceDTO>> GetAllServicesAsync()
        {
            var services = await _serviceRepository.GetAllAsync();
            return services.Select(s => new ServiceDTO
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Price = s.Price,
                OrdersCount = s.RepairOrders.Count
            });
        }

        /// <summary>
        /// Получить услугу по ID
        /// </summary>
        public async Task<ServiceDTO?> GetServiceByIdAsync(int id)
        {
            var service = await _serviceRepository.GetByIdAsync(id);
            if (service == null) return null;

            return new ServiceDTO
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description,
                Price = service.Price,
                OrdersCount = service.RepairOrders.Count
            };
        }

        /// <summary>
        /// Создать новую услугу
        /// </summary>
        public async Task<ServiceDTO> CreateServiceAsync(CreateServiceDTO serviceDto)
        {
            var service = new Service
            {
                Name = serviceDto.Name,
                Description = serviceDto.Description,
                Price = serviceDto.Price
            };

            var createdService = await _serviceRepository.CreateAsync(service);

            return new ServiceDTO
            {
                Id = createdService.Id,
                Name = createdService.Name,
                Description = createdService.Description,
                Price = createdService.Price,
                OrdersCount = 0
            };
        }

        /// <summary>
        /// Обновить данные услуги
        /// </summary>
        public async Task<ServiceDTO?> UpdateServiceAsync(int id, CreateServiceDTO serviceDto)
        {
            var existingService = await _serviceRepository.GetByIdAsync(id);
            if (existingService == null) return null;

            existingService.Name = serviceDto.Name;
            existingService.Description = serviceDto.Description;
            existingService.Price = serviceDto.Price;

            var updatedService = await _serviceRepository.UpdateAsync(existingService);

            return new ServiceDTO
            {
                Id = updatedService.Id,
                Name = updatedService.Name,
                Description = updatedService.Description,
                Price = updatedService.Price,
                OrdersCount = updatedService.RepairOrders.Count
            };
        }

        /// <summary>
        /// Удалить услугу
        /// </summary>
        public async Task<bool> DeleteServiceAsync(int id)
        {
            return await _serviceRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Получить популярные услуги (с наибольшим количеством заказов)
        /// </summary>
        public async Task<IEnumerable<ServiceDTO>> GetPopularServicesAsync()
        {
            var services = await _serviceRepository.GetAllAsync();
            return services
                .OrderByDescending(s => s.RepairOrders.Count)
                .Take(5)
                .Select(s => new ServiceDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Price = s.Price,
                    OrdersCount = s.RepairOrders.Count
                });
        }

        /// <summary>
        /// Получить услуги по диапазону цен
        /// </summary>
        public async Task<IEnumerable<ServiceDTO>> GetServicesByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            var services = await _serviceRepository.GetServicesByPriceRangeAsync(minPrice, maxPrice);
            return services.Select(s => new ServiceDTO
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Price = s.Price,
                OrdersCount = s.RepairOrders.Count
            });
        }
    }
}
