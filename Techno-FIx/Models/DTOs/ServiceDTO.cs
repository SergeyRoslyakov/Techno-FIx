namespace Techno_FIx.Models.DTOs
{
    public class ServiceDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int OrdersCount { get; set; }
    }
}
