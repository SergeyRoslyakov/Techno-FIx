using Techno_FIx.Models;

namespace Techno_FIx.Repositories
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<IEnumerable<Client>> GetClientsWithDevicesAsync();
        Task<Client> GetClientWithDevicesAsync(int id);
    }
}