namespace Techno_FIx.Models
{
    public class Technician
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public List<RepairOrder> RepairOrders { get; set; } = new();
    }
}
