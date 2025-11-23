using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Techno_FIx.Models;
using Techno_FIx.Models.DTOs;
using Techno_FIx.Repositories;

namespace Techno_FIx.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IRepository<User> userRepository, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<AuthResponseDTO> RegisterAsync(RegisterRequestDTO registerDto)
        {
            try
            {
                _logger.LogInformation("Регистрация пользователя: {Email}", registerDto.Email);

                if (await UserExistsAsync(registerDto.Email))
                    throw new ArgumentException("Пользователь с таким email уже существует");

                var allowedRoles = new[] { "User", "Technician", "Admin" };
                if (!allowedRoles.Contains(registerDto.Role))
                {
                    throw new ArgumentException($"Недопустимая роль: {registerDto.Role}");
                }

                var user = new User
                {
                    Username = registerDto.Username,
                    Email = registerDto.Email,
                    Password = registerDto.Password,
                    Role = registerDto.Role ?? "User",
                    TechnicianId = registerDto.TechnicianId,
                    CreatedAt = DateTime.UtcNow
                };

                var createdUser = await _userRepository.CreateAsync(user);
                var token = GenerateJwtToken(createdUser);

                _logger.LogInformation("Пользователь успешно зарегистрирован: {Email} (ID: {UserId})",
                    registerDto.Email, createdUser.Id);

                return new AuthResponseDTO
                {
                    Token = token,
                    Expires = DateTime.UtcNow.AddHours(24),
                    User = MapToUserDTO(createdUser)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при регистрации пользователя: {Email}", registerDto.Email);
                throw;
            }
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginRequestDTO loginDto)
        {
            try
            {
                _logger.LogInformation("Вход пользователя: {Email}", loginDto.Email);

                var users = await _userRepository.GetAllAsync();
                var user = users.FirstOrDefault(u => u.Email == loginDto.Email);

                if (user == null)
                {
                    _logger.LogWarning("Пользователь с email {Email} не найден", loginDto.Email);
                    throw new UnauthorizedAccessException("Неверный email или пароль");
                }

                // ПРОСТАЯ ПРОВЕРКА ПАРОЛЯ БЕЗ ХЕШИРОВАНИЯ
                if (user.Password != loginDto.Password)
                {
                    _logger.LogWarning("Неверный пароль для пользователя: {Email}", loginDto.Email);
                    throw new UnauthorizedAccessException("Неверный email или пароль");
                }

                var token = GenerateJwtToken(user);

                _logger.LogInformation("Успешный вход пользователя: {Email} (ID: {UserId})", loginDto.Email, user.Id);
                _logger.LogInformation("Сгенерированный токен: {Token}", token); // Логируем токен для отладки

                return new AuthResponseDTO
                {
                    Token = token,
                    Expires = DateTime.UtcNow.AddHours(24),
                    User = MapToUserDTO(user)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при входе пользователя: {Email}", loginDto.Email);
                throw;
            }
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            var users = await _userRepository.GetAllAsync();
            return users.Any(u => u.Email == email);
        }

        public string GenerateJwtToken(User user)
        {
            try
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("technicianId", user.TechnicianId?.ToString() ?? "")
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"] ?? "development_super_secret_key_32_chars_long!"));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"] ?? "TechnoFixAPI",
                    audience: _configuration["Jwt:Audience"] ?? "TechnoFixClient",
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(24),
                    signingCredentials: creds);

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                _logger.LogInformation("Сгенерирован JWT токен для пользователя: {User}", user.Username);

                return tokenString;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при генерации JWT токена");
                // Возвращаем простой токен в случае ошибки
                return $"jwt_token_{user.Id}_{Guid.NewGuid()}";
            }
        }

        private UserDTO MapToUserDTO(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                TechnicianId = user.TechnicianId
            };
        }
    }
}