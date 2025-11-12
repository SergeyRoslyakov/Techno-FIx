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
            private readonly IClientRepository _clientRepository;

            public DeviceService(IDeviceRepository deviceRepository, IClientRepository clientRepository)
            {
                _deviceRepository = deviceRepository;
                _clientRepository = clientRepository;
            }

            /// <summary>
            /// Получить все устройства
            /// </summary>
            public async Task<IEnumerable<DeviceDTO>> GetAllDevicesAsync()
            {
                var devices = await _deviceRepository.GetDevicesWithClientAsync();
                return devices.Select(d => new DeviceDTO
                {
                    Id = d.Id,
                    Type = d.Type,
                    Brand = d.Brand,
                    Model = d.Model,
                    SerialNumber = d.SerialNumber,
                    ProblemDescription = d.ProblemDescription,
                    ClientName = $"{d.Client.FirstName} {d.Client.LastName}",
                    ClientId = d.ClientId
                });
            }

            /// <summary>
            /// Получить устройство по ID
            /// </summary>
            public async Task<DeviceDTO?> GetDeviceByIdAsync(int id)
            {
                var device = await _deviceRepository.GetDeviceWithClientAsync(id);
                if (device == null) return null;

                return new DeviceDTO
                {
                    Id = device.Id,
                    Type = device.Type,
                    Brand = device.Brand,
                    Model = device.Model,
                    SerialNumber = device.SerialNumber,
                    ProblemDescription = device.ProblemDescription,
                    ClientName = $"{device.Client.FirstName} {device.Client.LastName}",
                    ClientId = device.ClientId
                };
            }

            /// <summary>
            /// Создать новое устройство
            /// </summary>
            public async Task<DeviceDTO> CreateDeviceAsync(CreateDeviceDTO deviceDto)
            {
                var client = await _clientRepository.GetByIdAsync(deviceDto.ClientId);
                if (client == null)
                    throw new ArgumentException("Client not found");

                var device = new Device
                {
                    Type = deviceDto.Type,
                    Brand = deviceDto.Brand,
                    Model = deviceDto.Model,
                    SerialNumber = deviceDto.SerialNumber,
                    ProblemDescription = deviceDto.ProblemDescription,
                    ClientId = deviceDto.ClientId
                };

                var createdDevice = await _deviceRepository.CreateAsync(device);
                var deviceWithClient = await _deviceRepository.GetDeviceWithClientAsync(createdDevice.Id);

                return new DeviceDTO
                {
                    Id = deviceWithClient.Id,
                    Type = deviceWithClient.Type,
                    Brand = deviceWithClient.Brand,
                    Model = deviceWithClient.Model,
                    SerialNumber = deviceWithClient.SerialNumber,
                    ProblemDescription = deviceWithClient.ProblemDescription,
                    ClientName = $"{deviceWithClient.Client.FirstName} {deviceWithClient.Client.LastName}",
                    ClientId = deviceWithClient.ClientId
                };
            }

            /// <summary>
            /// Обновить данные устройства
            /// </summary>
            public async Task<DeviceDTO?> UpdateDeviceAsync(int id, CreateDeviceDTO deviceDto)
            {
                var existingDevice = await _deviceRepository.GetByIdAsync(id);
                if (existingDevice == null)
                    return null;

                existingDevice.Type = deviceDto.Type;
                existingDevice.Brand = deviceDto.Brand;
                existingDevice.Model = deviceDto.Model;
                existingDevice.SerialNumber = deviceDto.SerialNumber;
                existingDevice.ProblemDescription = deviceDto.ProblemDescription;
                existingDevice.ClientId = deviceDto.ClientId;

                var updatedDevice = await _deviceRepository.UpdateAsync(existingDevice);
                var deviceWithClient = await _deviceRepository.GetDeviceWithClientAsync(updatedDevice.Id);

                return new DeviceDTO
                {
                    Id = deviceWithClient.Id,
                    Type = deviceWithClient.Type,
                    Brand = deviceWithClient.Brand,
                    Model = deviceWithClient.Model,
                    SerialNumber = deviceWithClient.SerialNumber,
                    ProblemDescription = deviceWithClient.ProblemDescription,
                    ClientName = $"{deviceWithClient.Client.FirstName} {deviceWithClient.Client.LastName}",
                    ClientId = deviceWithClient.ClientId
                };
            }

            /// <summary>
            /// Удалить устройство
            /// </summary>
            public async Task<bool> DeleteDeviceAsync(int id)
            {
                return await _deviceRepository.DeleteAsync(id);
            }

            /// <summary>
            /// Получить устройства по клиенту
            /// </summary>
            public async Task<IEnumerable<DeviceDTO>> GetDevicesByClientAsync(int clientId)
            {
                var devices = await _deviceRepository.GetDevicesByClientIdAsync(clientId);
                return devices.Select(d => new DeviceDTO
                {
                    Id = d.Id,
                    Type = d.Type,
                    Brand = d.Brand,
                    Model = d.Model,
                    SerialNumber = d.SerialNumber,
                    ProblemDescription = d.ProblemDescription,
                    ClientName = $"{d.Client.FirstName} {d.Client.LastName}",
                    ClientId = d.ClientId
                });
            }
        }
    }
}