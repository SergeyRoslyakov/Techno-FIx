using Techno_FIx.Models.DTOs;

namespace Techno_Fix.Services
{
    /// <summary>
    /// Сервис для работы с услугами
    /// </summary>
    public interface IServiceService
    {
        /// <summary>
        /// Получить все услуги
        /// </summary>
        Task<IEnumerable<ServiceDTO>> GetAllServicesAsync();

        /// <summary>
        /// Получить услугу по ID
        /// </summary>
        Task<ServiceDTO?> GetServiceByIdAsync(int id);

        /// <summary>
        /// Создать новую услугу
        /// </summary>
        Task<ServiceDTO> CreateServiceAsync(CreateServiceDTO serviceDto);

        /// <summary>
        /// Обновить данные услуги
        /// </summary>
        Task<ServiceDTO?> UpdateServiceAsync(int id, CreateServiceDTO serviceDto);

        /// <summary>
        /// Удалить услугу
        /// </summary>
        Task<bool> DeleteServiceAsync(int id);

        /// <summary>
        /// Получить популярные услуги (с наибольшим количеством заказов)
        /// </summary>
        Task<IEnumerable<ServiceDTO>> GetPopularServicesAsync();

        /// <summary>
        /// Получить услуги по диапазону цен
        /// </summary>
        Task<IEnumerable<ServiceDTO>> GetServicesByPriceRangeAsync(decimal minPrice, decimal maxPrice);
    }
}