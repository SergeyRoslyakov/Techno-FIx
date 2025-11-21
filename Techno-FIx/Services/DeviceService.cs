using Techno_Fix.Services;
using Techno_FIx.Models;
using Techno_FIx.Models.DTOs;
using Techno_FIx.Repositories;

namespace Techno_FIx.Services
{
    namespace Techno_Fix.Services
    {
        /// <summary>
        /// Сервис для работы с устройствами
        /// </summary>
        public class DeviceService : IDeviceService
        {
            private readonly IDeviceRepository _deviceRepository;

            public DeviceService(IDeviceRepository deviceRepository)
            {
                _deviceRepository = deviceRepository;
            }

            public async Task<IEnumerable<DeviceDTO>> GetAllDevicesAsync()
            {
                var devices = await _deviceRepository.GetAllAsync();
                return devices.Select(MapToDeviceDTO);
            }

            public async Task<DeviceDTO?> GetDeviceByIdAsync(int id)
            {
                var device = await _deviceRepository.GetByIdAsync(id);
                return device == null ? null : MapToDeviceDTO(device);
            }

            public async Task<DeviceDTO> CreateDeviceAsync(CreateDeviceDTO deviceDto)
            {
                var device = new Device
                {
                    Type = deviceDto.Type,
                    Brand = deviceDto.Brand,
                    Model = deviceDto.Model,
                    SerialNumber = deviceDto.SerialNumber,
                    ProblemDescription = deviceDto.ProblemDescription,
                    ClientId = deviceDto.ClientId
                };

                var created = await _deviceRepository.CreateAsync(device);
                return MapToDeviceDTO(created);
            }

            public async Task<bool> UpdateDeviceAsync(int id, CreateDeviceDTO deviceDto)
            {
                var existing = await _deviceRepository.GetByIdAsync(id);
                if (existing == null) return false;

                existing.Type = deviceDto.Type;
                existing.Brand = deviceDto.Brand;
                existing.Model = deviceDto.Model;
                existing.SerialNumber = deviceDto.SerialNumber;
                existing.ProblemDescription = deviceDto.ProblemDescription;
                existing.ClientId = deviceDto.ClientId;

                await _deviceRepository.UpdateAsync(existing);
                return true;
            }

            public async Task<bool> DeleteDeviceAsync(int id)
            {
                return await _deviceRepository.DeleteAsync(id);
            }

            public async Task<IEnumerable<DeviceDTO>> GetDevicesByClientAsync(int clientId)
            {
                var devices = await _deviceRepository.GetAllAsync();
                var clientDevices = devices.Where(d => d.ClientId == clientId);
                return clientDevices.Select(MapToDeviceDTO);
            }

            private DeviceDTO MapToDeviceDTO(Device device)
            {
                return new DeviceDTO
                {
                    Id = device.Id,
                    Type = device.Type,
                    Brand = device.Brand,
                    Model = device.Model,
                    SerialNumber = device.SerialNumber,
                    ProblemDescription = device.ProblemDescription,
                    ClientName = $"{device.Client?.FirstName} {device.Client?.LastName}",
                    ClientId = device.ClientId
                };
            }
        }
    }
}