using Techno_FIx.Models.DTOs;

namespace Techno_Fix.Services
{
    /// <summary>
    /// Сервис для работы с услугами
    /// </summary>
    public interface IServiceService
    {
        Task<IEnumerable<ServiceDTO>> GetAllServicesAsync();
        Task<ServiceDTO?> GetServiceByIdAsync(int id);
        Task<ServiceDTO> CreateServiceAsync(CreateServiceDTO serviceDto);
        Task<bool> UpdateServiceAsync(int id, CreateServiceDTO serviceDto);
        Task<bool> DeleteServiceAsync(int id);
        Task<IEnumerable<ServiceDTO>> GetPopularServicesAsync(int topN = 5);
    }
}