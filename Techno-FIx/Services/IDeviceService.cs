using Techno_FIx.Models.DTOs;

namespace Techno_Fix.Services
{
    /// <summary>
    /// Сервис для работы с устройствами
    /// </summary>
    public interface IDeviceService
    {
        Task<IEnumerable<DeviceDTO>> GetAllDevicesAsync();
        Task<DeviceDTO?> GetDeviceByIdAsync(int id);
        Task<DeviceDTO> CreateDeviceAsync(CreateDeviceDTO deviceDto);
        Task<DeviceDTO?> UpdateDeviceAsync(int id, CreateDeviceDTO deviceDto);
        Task<bool> DeleteDeviceAsync(int id);
        Task<IEnumerable<DeviceDTO>> GetDevicesByClientAsync(int clientId);
    }
}
