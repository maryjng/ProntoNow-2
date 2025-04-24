using Microsoft.AspNetCore.Mvc;
using Pronto.Repositories.Interfaces;
using Pronto.DTOs;
using Pronto.Services;

namespace Pronto.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;

        public UserController(IUserService userService, IUserRepository userRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var resp = await _userRepository.GetUserByIdAsync(id);

            return StatusCode(resp.StatusCode, resp);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDTO dto)
        {
            var resp = await _userService.RegisterUserAsync(dto);

            return StatusCode(resp.StatusCode, resp);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO dto)
        {
            var resp = await _userService.LoginAsync(dto); 

            return StatusCode(resp.StatusCode, resp);
        }

        [HttpPatch("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, UserUpdateDTO userUpdateDTO)
        {
            var resp = await _userRepository.UpdateUserAsync(userId, userUpdateDTO);

            return StatusCode(resp.StatusCode, resp);
        }

    }
}
