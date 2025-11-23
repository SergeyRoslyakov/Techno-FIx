using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Techno_Fix.Services;
using Techno_FIx.Models.DTOs;
using Techno_FIx.Services;

namespace Techno_Fix.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        // РЕГИСТРАЦИЯ ТОЛЬКО ДЛЯ АДМИНОВ
        [HttpPost("register")]
        [Authorize(Roles = "Admin")] // ТОЛЬКО АДМИН МОЖЕТ РЕГИСТРИРОВАТЬ
        public async Task<ActionResult> Register(RegisterRequestDTO registerDto)
        {
            try
            {
                _logger.LogInformation("Запрос на регистрацию пользователя: {Email} от администратора: {AdminName}",
                    registerDto.Email, User.Identity?.Name);

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Невалидные данные при регистрации");
                    return BadRequest(ModelState);
                }

                var result = await _authService.RegisterAsync(registerDto);

                _logger.LogInformation("Пользователь успешно зарегистрирован администратором: {Email}", registerDto.Email);

                return Ok(new
                {
                    success = true,
                    message = "Пользователь успешно зарегистрирован",
                    token = result.Token,
                    expires = result.Expires.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    user = new
                    {
                        id = result.User.Id,
                        username = result.User.Username,
                        email = result.User.Email,
                        role = result.User.Role,
                        technicianId = result.User.TechnicianId
                    }
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Ошибка аргумента при регистрации: {Message}", ex.Message);
                return BadRequest(new { success = false, error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Критическая ошибка при регистрации пользователя");
                return StatusCode(500, new { success = false, error = "Внутренняя ошибка сервера" });
            }
        }

        // ВХОД ОСТАЕТСЯ ДОСТУПЕН ДЛЯ ВСЕХ
        [HttpPost("login")]
        [AllowAnonymous] // Вход доступен всем
        public async Task<ActionResult> Login(LoginRequestDTO loginDto)
        {
            try
            {
                _logger.LogInformation("Запрос на вход пользователя: {Email}", loginDto.Email);

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Невалидные данные при входе");
                    return BadRequest(ModelState);
                }

                var result = await _authService.LoginAsync(loginDto);

                _logger.LogInformation("Успешный вход пользователя: {Email}", loginDto.Email);

                return Ok(new
                {
                    success = true,
                    message = "Успешный вход в систему",
                    token = result.Token,
                    expires = result.Expires.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    user = new
                    {
                        id = result.User.Id,
                        username = result.User.Username,
                        email = result.User.Email,
                        role = result.User.Role,
                        technicianId = result.User.TechnicianId
                    }
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Неавторизованный доступ при входе: {Email} - {Message}", loginDto.Email, ex.Message);
                return Unauthorized(new { success = false, error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Критическая ошибка при входе пользователя");
                return StatusCode(500, new { success = false, error = "Внутренняя ошибка сервера" });
            }
        }

        // ПУБЛИЧНЫЙ МЕТОД ДЛЯ РЕГИСТРАЦИИ ТОЛЬКО ПОЛЬЗОВАТЕЛЕЙ (User)
        [HttpPost("register-user")]
        [AllowAnonymous] // Доступно без авторизации
        public async Task<ActionResult> RegisterUser(RegisterUserDTO registerDto)
        {
            try
            {
                _logger.LogInformation("Публичная регистрация пользователя: {Email}", registerDto.Email);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Фиксируем роль "User" - нельзя зарегистрировать админа или техника
                var internalDto = new RegisterRequestDTO
                {
                    Username = registerDto.Username,
                    Email = registerDto.Email,
                    Password = registerDto.Password,
                    Role = "User" // ТОЛЬКО РОЛЬ USER
                };

                var result = await _authService.RegisterAsync(internalDto);

                _logger.LogInformation("Пользователь успешно зарегистрирован через публичный метод: {Email}", registerDto.Email);

                return Ok(new
                {
                    success = true,
                    message = "Пользователь успешно зарегистрирован",
                    user = new
                    {
                        id = result.User.Id,
                        username = result.User.Username,
                        email = result.User.Email,
                        role = result.User.Role
                    }
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при публичной регистрации");
                return StatusCode(500, new { success = false, error = "Внутренняя ошибка сервера" });
            }
        }
    }

    // DTO для публичной регистрации (только User)
    public class RegisterUserDTO
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}