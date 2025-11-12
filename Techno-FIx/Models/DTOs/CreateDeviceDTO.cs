namespace Techno_FIx.Models.DTOs
{
    public class CreateDeviceDTO
    {
        public string Type { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string ProblemDescription { get; set; } = string.Empty;
        public int ClientId { get; set; }
    }
}
