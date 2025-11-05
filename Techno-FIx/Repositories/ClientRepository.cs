using Microsoft.EntityFrameworkCore;
using Techno_FIx.Data;
using Techno_FIx.Models;

namespace Techno_FIx.Repositories
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        public ClientRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Client>> GetClientsWithDevicesAsync()
        {
            return await _context.Clients
                .Include(c => c.Devices)
                .ToListAsync();
        }

        public async Task<Client> GetClientWithDevicesAsync(int id)
        {
            return await _context.Clients
                .Include(c => c.Devices)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
