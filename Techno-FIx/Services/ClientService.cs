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

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<IEnumerable<ClientDTO>> GetAllClientsAsync()
        {
            var clients = await _clientRepository.GetAllAsync();
            return clients.Select(MapToClientDTO);
        }

        public async Task<ClientDTO?> GetClientByIdAsync(int id)
        {
            var client = await _clientRepository.GetByIdAsync(id);
            return client == null ? null : MapToClientDTO(client);
        }

        public async Task<ClientDTO> CreateClientAsync(CreateClientDTO clientDto)
        {
            var client = new Client
            {
                FirstName = clientDto.FirstName,
                LastName = clientDto.LastName,
                Phone = clientDto.Phone,
                Email = clientDto.Email
            };

            var created = await _clientRepository.CreateAsync(client);
            return MapToClientDTO(created);
        }

        public async Task<bool> UpdateClientAsync(int id, UpdateClientDTO clientDto)
        {
            var existing = await _clientRepository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.FirstName = clientDto.FirstName;
            existing.LastName = clientDto.LastName;
            existing.Phone = clientDto.Phone;
            existing.Email = clientDto.Email;

            await _clientRepository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteClientAsync(int id)
        {
            return await _clientRepository.DeleteAsync(id);
        }

        private ClientDTO MapToClientDTO(Client client)
        {
            return new ClientDTO
            {
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Phone = client.Phone,
                Email = client.Email,
                DevicesCount = client.Devices?.Count ?? 0
            };
        }
    }
}