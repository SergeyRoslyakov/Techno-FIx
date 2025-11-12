using Techno_FIx.Models.DTOs;

namespace Techno_FIx.Services
{
    public interface ITechnicianService
    {
        Task<IEnumerable<TechnicianDTO>> GetAllTechniciansAsync();
        Task<TechnicianDTO?> GetTechnicianByIdAsync(int id);
        Task<TechnicianDTO> CreateTechnicianAsync(CreateTechnicianDTO technicianDto);
        Task<TechnicianDTO?> UpdateTechnicianAsync(int id, UpdateTechnicianDTO technicianDto);
        Task<bool> DeleteTechnicianAsync(int id);
        Task<IEnumerable<TechnicianDTO>> GetActiveTechniciansAsync();
        Task<IEnumerable<TechnicianDTO>> GetTechniciansBySpecializationAsync(string specialization);
        Task<IEnumerable<RepairOrderDTO>> GetTechnicianOrdersAsync(int technicianId);
    }
}
