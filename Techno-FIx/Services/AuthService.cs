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
        private readonly IMapper _mapper;

        public AuthService(
            IRepository<User> userRepository,
            IConfiguration configuration,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<AuthResponseDTO> RegisterAsync(RegisterRequestDTO registerDto)
        {
            if (await UserExistsAsync(registerDto.Email))
            {
                throw new ArgumentException("Пользователь с таким email уже существует");
            }

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                Password = registerDto.Password, // Пароль сохраняется как есть
                Role = registerDto.Role ?? "User",
                TechnicianId = registerDto.TechnicianId
            };

            var createdUser = await _userRepository.CreateAsync(user);
            var token = GenerateJwtToken(createdUser);

            return new AuthResponseDTO
            {
                Token = token,
                Expires = DateTime.UtcNow.AddMinutes(
                    _configuration.GetValue<int>("Jwt:ExpiresInMinutes")),
                User = _mapper.Map<UserDTO>(createdUser)
            };
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginRequestDTO loginDto)
        {
            var users = await _userRepository.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Email == loginDto.Email);

            // Простая проверка пароля без хеширования
            if (user == null || user.Password != loginDto.Password)
            {
                throw new UnauthorizedAccessException("Неверный email или пароль");
            }

            var token = GenerateJwtToken(user);

            return new AuthResponseDTO
            {
                Token = token,
                Expires = DateTime.UtcNow.AddMinutes(
                    _configuration.GetValue<int>("Jwt:ExpiresInMinutes")),
                User = _mapper.Map<UserDTO>(user)
            };
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            var users = await _userRepository.GetAllAsync();
            return users.Any(u => u.Email == email);
        }

        public string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("technicianId", user.TechnicianId?.ToString() ?? "")
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:ExpiresInMinutes")),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}