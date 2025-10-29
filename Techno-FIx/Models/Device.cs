namespace Techno_FIx.Models
{
    public class Device
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string ProblemDescription { get; set; } = string.Empty;

        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;

        public List<RepairOrder> RepairOrders { get; set; } = new();
    }
}
