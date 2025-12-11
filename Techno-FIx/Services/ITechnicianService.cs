using Techno_FIx.Models.DTOs;

namespace Techno_Fix.Services
{
    /// <summary>
    /// Сервис для работы с техниками
    /// </summary>
    public interface ITechnicianService
    {
        Task<IEnumerable<TechnicianDTO>> GetAllTechniciansAsync();
        Task<TechnicianDTO?> GetTechnicianByIdAsync(int id);
        Task<TechnicianDTO> CreateTechnicianAsync(CreateTechnicianDTO technicianDto);
        Task<bool> UpdateTechnicianAsync(int id, UpdateTechnicianDTO technicianDto);
        Task<bool> DeleteTechnicianAsync(int id);
        Task<IEnumerable<TechnicianDTO>> GetActiveTechniciansAsync();
        Task<IEnumerable<TechnicianDTO>> GetTechniciansBySpecializationAsync(string specialization);
        Task<IEnumerable<RepairOrderDTO>> GetTechnicianOrdersAsync(int technicianId);
    }
}