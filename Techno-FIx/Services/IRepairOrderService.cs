using Techno_FIx.Models.DTOs;

namespace Techno_Fix.Services
{
    /// <summary>
    /// Сервис для работы с заказами на ремонт
    /// </summary>
    public interface IRepairOrderService
    {
        /// <summary>
        /// Получить все заказы с деталями
        /// </summary>
        Task<IEnumerable<RepairOrderDTO>> GetAllOrdersAsync();

        /// <summary>
        /// Получить заказ по ID с деталями
        /// </summary>
        Task<RepairOrderDTO?> GetOrderByIdAsync(int id);

        /// <summary>
        /// Создать новый заказ на ремонт
        /// </summary>
        Task<RepairOrderDTO> CreateOrderAsync(CreateRepairOrderDTO orderDto);

        /// <summary>
        /// Обновить статус заказа
        /// </summary>
        Task<RepairOrderDTO?> UpdateOrderStatusAsync(int id, UpdateRepairOrderDTO orderDto);

        /// <summary>
        /// Удалить заказ на ремонт
        /// </summary>
        Task<bool> DeleteOrderAsync(int id);

        /// <summary>
        /// Получить заказы по статусу
        /// </summary>
        Task<IEnumerable<RepairOrderDTO>> GetOrdersByStatusAsync(string status);

        /// <summary>
        /// Получить статистику по заказам
        /// </summary>
        Task<object> GetOrdersStatisticsAsync();
    }
}