using Techno_FIx.Models;
using Techno_FIx.Models.DTOs;

namespace Techno_FIx.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> RegisterAsync(RegisterRequestDTO registerDto);
        Task<AuthResponseDTO> LoginAsync(LoginRequestDTO loginDto);
        Task<bool> UserExistsAsync(string email);
        string GenerateJwtToken(User user);
    }
}
