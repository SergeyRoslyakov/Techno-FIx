namespace Techno_FIx.Models.DTOs
{
    public class CreateServiceDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
