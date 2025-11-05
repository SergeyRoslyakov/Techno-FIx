using Microsoft.EntityFrameworkCore;
using Techno_FIx.Data;
using Techno_FIx.Models;

namespace Techno_FIx.Repositories
{
    public class DeviceRepository : Repository<Device>, IDeviceRepository
    {
        public DeviceRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Device>> GetDevicesWithClientAsync()
        {
            return await _context.Devices
                .Include(d => d.Client)
                .ToListAsync();
        }

        public async Task<Device> GetDeviceWithClientAsync(int id)
        {
            return await _context.Devices
                .Include(d => d.Client)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<Device>> GetDevicesByClientIdAsync(int clientId)
        {
            return await _context.Devices
                .Where(d => d.ClientId == clientId)
                .ToListAsync();
        }
    }
}
