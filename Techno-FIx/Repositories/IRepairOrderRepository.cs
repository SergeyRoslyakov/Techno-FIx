using Techno_FIx.Models;

namespace Techno_FIx.Repositories
{
    public interface IRepairOrderRepository : IRepository<RepairOrder>
    {
        Task<IEnumerable<RepairOrder>> GetRepairOrdersWithDetailsAsync();
        Task<RepairOrder> GetRepairOrderWithDetailsAsync(int id);
        Task<IEnumerable<RepairOrder>> GetRepairOrdersByStatusAsync(string status);
        Task<IEnumerable<RepairOrder>> GetRepairOrdersByTechnicianAsync(int technicianId);
    }
}
