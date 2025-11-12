using Techno_FIx.Models;
using Techno_FIx.Models.DTOs;
using Techno_FIx.Repositories;
using Techno_FIx.Services;

namespace Techno_Fix.Services
{
    /// <summary>
    /// Сервис для работы с клиентами
    /// </summary>
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IDeviceRepository _deviceRepository;

        public ClientService(IClientRepository clientRepository, IDeviceRepository deviceRepository)
        {
            _clientRepository = clientRepository;
            _deviceRepository = deviceRepository;
        }

        /// <summary>
        /// Получить всех клиентов
        /// </summary>
        public async Task<IEnumerable<ClientDTO>> GetAllClientsAsync()
        {
            var clients = await _clientRepository.GetAllAsync();
            return clients.Select(c => new ClientDTO
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Phone = c.Phone,
                Email = c.Email,
                DevicesCount = c.Devices.Count
            });
        }

        /// <summary>
        /// Получить клиента по ID
        /// </summary>
        public async Task<ClientDTO?> GetClientByIdAsync(int id)
        {
            var client = await _clientRepository.GetByIdAsync(id);
            if (client == null) return null;

            return new ClientDTO
            {
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Phone = client.Phone,
                Email = client.Email,
                DevicesCount = client.Devices.Count
            };
        }

        /// <summary>
        /// Создать нового клиента
        /// </summary>
        public async Task<ClientDTO> CreateClientAsync(CreateClientDTO clientDto)
        {
            var client = new Client
            {
                FirstName = clientDto.FirstName,
                LastName = clientDto.LastName,
                Phone = clientDto.Phone,
                Email = clientDto.Email
            };

            var createdClient = await _clientRepository.CreateAsync(client);

            return new ClientDTO
            {
                Id = createdClient.Id,
                FirstName = createdClient.FirstName,
                LastName = createdClient.LastName,
                Phone = createdClient.Phone,
                Email = createdClient.Email,
                DevicesCount = 0
            };
        }

        /// <summary>
        /// Обновить данные клиента
        /// </summary>
        public async Task<ClientDTO?> UpdateClientAsync(int id, UpdateClientDTO clientDto)
        {
            var existingClient = await _clientRepository.GetByIdAsync(id);
            if (existingClient == null) return null;

            existingClient.FirstName = clientDto.FirstName;
            existingClient.LastName = clientDto.LastName;
            existingClient.Phone = clientDto.Phone;
            existingClient.Email = clientDto.Email;

            var updatedClient = await _clientRepository.UpdateAsync(existingClient);

            return new ClientDTO
            {
                Id = updatedClient.Id,
                FirstName = updatedClient.FirstName,
                LastName = updatedClient.LastName,
                Phone = updatedClient.Phone,
                Email = updatedClient.Email,
                DevicesCount = updatedClient.Devices.Count
            };
        }

        /// <summary>
        /// Удалить клиента
        /// </summary>
        public async Task<bool> DeleteClientAsync(int id)
        {
            return await _clientRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Получить клиентов с количеством устройств
        /// </summary>
        public async Task<IEnumerable<ClientDTO>> GetClientsWithDeviceCountAsync()
        {
            var clients = await _clientRepository.GetClientsWithDevicesAsync();
            return clients.Select(c => new ClientDTO
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Phone = c.Phone,
                Email = c.Email,
                DevicesCount = c.Devices.Count
            });
        }

        /// <summary>
        /// Получить устройства клиента
        /// </summary>
        public async Task<IEnumerable<DeviceDTO>> GetClientDevicesAsync(int clientId)
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