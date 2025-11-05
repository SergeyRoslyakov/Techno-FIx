using Techno_FIx.Models;

namespace Techno_FIx.Repositories
{
    public interface IDeviceRepository : IRepository<Device>
    {
        Task<IEnumerable<Device>> GetDevicesWithClientAsync();
        Task<Device> GetDeviceWithClientAsync(int id);
        Task<IEnumerable<Device>> GetDevicesByClientIdAsync(int clientId);
    }
}