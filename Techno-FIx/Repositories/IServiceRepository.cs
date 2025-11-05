using Techno_FIx.Models;

namespace Techno_FIx.Repositories
{
    public interface IServiceRepository : IRepository<Service>
    {
        Task<IEnumerable<Service>> GetServicesByPriceRangeAsync(decimal minPrice, decimal maxPrice);
    }
}
