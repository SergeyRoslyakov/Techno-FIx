namespace Techno_FIx.Models.DTOs
{
    public class UpdateRepairOrderDTO
    {
        public string Status { get; set; } = string.Empty;
        public decimal TotalCost { get; set; }
        public string TechnicianNotes { get; set; } = string.Empty;
    }
}
