using Techno_FIx.Models;

namespace Techno_FIx.Repositories
{
    public interface ITechnicianRepository : IRepository<Technician>
    {
        Task<IEnumerable<Technician>> GetActiveTechniciansAsync();
        Task<IEnumerable<Technician>> GetTechniciansBySpecializationAsync(string specialization);
    }
}
