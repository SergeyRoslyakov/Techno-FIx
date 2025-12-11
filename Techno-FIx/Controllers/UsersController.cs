using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Techno_FIx.Models;
using Techno_FIx.Models.DTOs;
using Techno_FIx.Repositories;
using Techno_FIx.Services;

namespace Techno_FIx.Controllers
{
    public class UsersController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public UsersController(
            IAuthService authService,
            IRepository<User> userRepository,
            IMapper mapper)
        {
            _authService = authService;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Получить список всех пользователей (только для администраторов)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            var users = await _userRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<UserDTO>>(users));
        }

        /// <summary>
        /// Получить профиль текущего пользователя
        /// </summary>
        [HttpGet("profile")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Technician},{UserRoles.User}")]
        public async Task<ActionResult<UserDTO>> GetMyProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                return NotFound(new { message = "Пользователь не найден" });

            return Ok(_mapper.Map<UserDTO>(user));
        }

        /// <summary>
        /// Получить пользователя по ID (только для администраторов)
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult<UserDTO>> GetUserById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound(new { message = $"Пользователь с ID {id} не найден" });

            return Ok(_mapper.Map<UserDTO>(user));
        }

        /// <summary>
        /// Обновить роль пользователя (только для администраторов)
        /// </summary>
        [HttpPut("{id}/role")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult> UpdateUserRole(int id, [FromBody] string newRole)
        {
            var validRoles = new[] { UserRoles.Admin, UserRoles.Technician, UserRoles.User };
            if (!validRoles.Contains(newRole))
            {
                return BadRequest(new { message = "Недопустимая роль" });
            }

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound(new { message = $"Пользователь с ID {id} не найден" });

            user.Role = newRole;

            await _userRepository.UpdateAsync(user);
            return Ok(new { message = "Роль пользователя обновлена" });
        }
    }
}
