using Techno_FIx.Models.DTOs;

namespace Techno_FIx.Services
{
    public interface IServiceService
    {
        Task<IEnumerable<ServiceDTO>> GetAllServicesAsync();
        Task<ServiceDTO?> GetServiceByIdAsync(int id);
        Task<ServiceDTO> CreateServiceAsync(CreateServiceDTO serviceDto);
        Task<ServiceDTO?> UpdateServiceAsync(int id, CreateServiceDTO serviceDto);
        Task<bool> DeleteServiceAsync(int id);
        Task<IEnumerable<ServiceDTO>> GetPopularServicesAsync();
        Task<IEnumerable<ServiceDTO>> GetServicesByPriceRangeAsync(decimal minPrice, decimal maxPrice);
    }
}
