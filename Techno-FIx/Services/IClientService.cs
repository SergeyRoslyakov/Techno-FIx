using Techno_FIx.Models.DTOs;

namespace Techno_FIx.Services
{
    public interface IClientService
    {
        Task<IEnumerable<ClientDTO>> GetAllClientsAsync();
        Task<ClientDTO?> GetClientByIdAsync(int id);
        Task<ClientDTO> CreateClientAsync(CreateClientDTO clientDto);
        Task<ClientDTO?> UpdateClientAsync(int id, UpdateClientDTO clientDto);
        Task<bool> DeleteClientAsync(int id);
        Task<IEnumerable<ClientDTO>> GetClientsWithDeviceCountAsync();
        Task<IEnumerable<DeviceDTO>> GetClientDevicesAsync(int clientId);
    }
}
