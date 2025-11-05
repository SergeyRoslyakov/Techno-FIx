using Microsoft.EntityFrameworkCore;
using Techno_FIx.Data;
using Techno_FIx.Models;

namespace Techno_FIx.Repositories
{
    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
        public ServiceRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Service>> GetServicesByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _context.Services
                .Where(s => s.Price >= minPrice && s.Price <= maxPrice)
                .OrderBy(s => s.Price)
                .ToListAsync();
        }
    }
}
