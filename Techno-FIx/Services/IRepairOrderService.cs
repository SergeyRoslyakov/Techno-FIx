using Techno_FIx.Models.DTOs;

namespace Techno_Fix.Services
{
    public interface IRepairOrderService
    {
        Task<IEnumerable<RepairOrderDTO>> GetAllRepairOrdersAsync();
        Task<RepairOrderDTO?> GetRepairOrderByIdAsync(int id); 
        Task<RepairOrderDTO> CreateRepairOrderAsync(CreateRepairOrderDTO orderDto);
        Task<bool> UpdateRepairOrderAsync(int id, UpdateRepairOrderDTO orderDto);
        Task<bool> DeleteRepairOrderAsync(int id);
        Task<bool> UpdateOrderStatusAsync(int id, string status);
    }
}