using Microsoft.AspNetCore.Mvc;
using Techno_FIx.Models.DTOs;
using Techno_FIx.Services;

namespace Techno_FIx.Controllers
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

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDTO>> Register(RegisterRequestDTO registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _authService.RegisterAsync(registerDto);

                _logger.LogInformation("Новый пользователь зарегистрирован: {Email}", registerDto.Email);

                return CreatedAtAction(nameof(Register), new { id = result.User.Id }, new
                {
                    userId = result.User.Id,
                    message = "Пользователь успешно зарегистрирован",
                    token = result.Token,
                    user = result.User
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при регистрации пользователя");
                return StatusCode(500, new { error = "Внутренняя ошибка сервера" });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Login(LoginRequestDTO loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _authService.LoginAsync(loginDto);

                _logger.LogInformation("Пользователь вошел в систему: {Email}", loginDto.Email);

                return Ok(new
                {
                    token = result.Token,
                    expires = result.Expires,
                    user = result.User
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при входе пользователя");
                return StatusCode(500, new { error = "Внутренняя ошибка сервера" });
            }
        }
    }
}
