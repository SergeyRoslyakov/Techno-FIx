namespace Techno_FIx.Models.DTOs
{
    public class CreateRepairOrderDTO
    {
        public int DeviceId { get; set; }
        public int ServiceId { get; set; }
        public int TechnicianId { get; set; }
        public string ProblemDescription { get; set; } = string.Empty;
    }
}
