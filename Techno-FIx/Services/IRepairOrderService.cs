using Techno_FIx.Models.DTOs;

namespace Techno_Fix.Services
{
    /// <summary>
    /// Сервис для работы с заказами на ремонт
    /// </summary>
    public interface IRepairOrderService
    {
        Task<IEnumerable<RepairOrderDTO>> GetAllRepairOrdersAsync();
        Task<RepairOrderDTO?> GetRepairOrderByIdAsync(int id);
        Task<RepairOrderDTO> CreateRepairOrderAsync(CreateRepairOrderDTO repairOrderDto);
        Task<bool> UpdateRepairOrderAsync(int id, UpdateRepairOrderDTO repairOrderDto);
        Task<bool> DeleteRepairOrderAsync(int id);
        Task<IEnumerable<RepairOrderDTO>> GetOrdersByTechnicianAsync(int technicianId);
        Task<IEnumerable<RepairOrderDTO>> GetOrdersByClientAsync(int clientId);
    }
}