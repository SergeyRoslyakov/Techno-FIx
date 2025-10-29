namespace Techno_FIx.Models
{
    public class RepairOrder
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedDate { get; set; }
        public decimal TotalCost { get; set; }
        public string Status { get; set; } = "Received";

        public int DeviceId { get; set; }
        public int ServiceId { get; set; }
        public int TechnicianId { get; set; }

        public Device Device { get; set; } = null!;
        public Service Service { get; set; } = null!;
        public Technician Technician { get; set; } = null!;
    }
}