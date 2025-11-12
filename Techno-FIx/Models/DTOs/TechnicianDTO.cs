namespace Techno_FIx.Models.DTOs
{
    public class TechnicianDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int ActiveOrdersCount { get; set; }
    }
}
