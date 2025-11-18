using Techno_FIx.Models.DTOs;

namespace Techno_Fix.Services
{
    /// <summary>
    /// Сервис для работы с техниками
    /// </summary>
    public interface ITechnicianService
    {
        /// <summary>
        /// Получить всех техников
        /// </summary>
        Task<IEnumerable<TechnicianDTO>> GetAllTechniciansAsync();

        /// <summary>
        /// Получить техника по ID
        /// </summary>
        Task<TechnicianDTO?> GetTechnicianByIdAsync(int id);

        /// <summary>
        /// Создать нового техника
        /// </summary>
        Task<TechnicianDTO> CreateTechnicianAsync(CreateTechnicianDTO technicianDto);

        /// <summary>
        /// Обновить данные техника
        /// </summary>
        Task<TechnicianDTO?> UpdateTechnicianAsync(int id, UpdateTechnicianDTO technicianDto);

        /// <summary>
        /// Удалить техника
        /// </summary>
        Task<bool> DeleteTechnicianAsync(int id);

        /// <summary>
        /// Получить активных техников
        /// </summary>
        Task<IEnumerable<TechnicianDTO>> GetActiveTechniciansAsync();

        /// <summary>
        /// Получить техников по специализации
        /// </summary>
        Task<IEnumerable<TechnicianDTO>> GetTechniciansBySpecializationAsync(string specialization);

        /// <summary>
        /// Получить заказы техника
        /// </summary>
        Task<IEnumerable<RepairOrderDTO>> GetTechnicianOrdersAsync(int technicianId);
    }
}