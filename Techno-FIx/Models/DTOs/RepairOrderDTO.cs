namespace Techno_FIx.Models.DTOs
{
    public class RepairOrderDTO
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public decimal TotalCost { get; set; }
        public string Status { get; set; } = string.Empty;
        public string DeviceInfo { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public string TechnicianName { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
    }
}
