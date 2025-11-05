using Microsoft.EntityFrameworkCore;
using Techno_FIx.Data;
using Techno_FIx.Models;

namespace Techno_FIx.Repositories
{
    public class TechnicianRepository : Repository<Technician>, ITechnicianRepository
    {
        public TechnicianRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Technician>> GetActiveTechniciansAsync()
        {
            return await _context.Technicians
                .Where(t => string.IsNullOrEmpty(t.Phone) || t.Phone != "inactive")
                .ToListAsync();
        }

        public async Task<IEnumerable<Technician>> GetTechniciansBySpecializationAsync(string specialization)
        {
            return await _context.Technicians
                .Where(t => t.Specialization.Contains(specialization))
                .ToListAsync();
        }
    }
}