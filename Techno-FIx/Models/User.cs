using System.ComponentModel.DataAnnotations;

namespace Techno_FIx.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "User"; 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int? TechnicianId { get; set; }
        public Technician? Technician { get; set; }
    }
    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string Technician = "Technician";
        public const string User = "User";
    }
}