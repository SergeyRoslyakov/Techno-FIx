using Microsoft.EntityFrameworkCore;
using Techno_FIx.Data;
using Techno_FIx.Models;

namespace Techno_FIx.Repositories
{
    public class RepairOrderRepository : Repository<RepairOrder>, IRepairOrderRepository
    {
        public RepairOrderRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<RepairOrder>> GetRepairOrdersWithDetailsAsync()
        {
            return await _context.RepairOrders
                .Include(ro => ro.Device)
                    .ThenInclude(d => d.Client)
                .Include(ro => ro.Service)
                .Include(ro => ro.Technician)
                .ToListAsync();
        }

        public async Task<RepairOrder> GetRepairOrderWithDetailsAsync(int id)
        {
            return await _context.RepairOrders
                .Include(ro => ro.Device)
                    .ThenInclude(d => d.Client)
                .Include(ro => ro.Service)
                .Include(ro => ro.Technician)
                .FirstOrDefaultAsync(ro => ro.Id == id);
        }

        public async Task<IEnumerable<RepairOrder>> GetRepairOrdersByStatusAsync(string status)
        {
            return await _context.RepairOrders
                .Where(ro => ro.Status == status)
                .Include(ro => ro.Device)
                .Include(ro => ro.Service)
                .ToListAsync();
        }

        public async Task<IEnumerable<RepairOrder>> GetRepairOrdersByTechnicianAsync(int technicianId)
        {
            return await _context.RepairOrders
                .Where(ro => ro.TechnicianId == technicianId)
                .Include(ro => ro.Device)
                .Include(ro => ro.Service)
                .ToListAsync();
        }
    }
}
