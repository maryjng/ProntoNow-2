using Microsoft.AspNetCore.Mvc;
using Pronto.Repositories.Interfaces;
using Pronto.DTOs;
using Pronto.Services;
using Microsoft.AspNetCore.Authorization;
using Pronto.Repositories;

namespace Pronto.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IReportRepository _reportRepository;
        private readonly IDeviceRepository _deviceRepository;

        public UserController(IUserService userService, IUserRepository userRepository, IReportRepository reportRepository, IDeviceRepository deviceRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
            _reportRepository = reportRepository;
            _deviceRepository = deviceRepository;
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
        [Authorize]
        public async Task<IActionResult> UpdateUser(int userId, UserUpdateDTO userUpdateDTO)
        {
            var resp = await _userRepository.UpdateUserAsync(userId, userUpdateDTO);

            return StatusCode(resp.StatusCode, resp);
        }

        // [HttpGet("{userId}/reports")]
        // [Authorize]
        // public async Task<IActionResult> GetReportsByUserId(int userId)
        // {
        //     var resp = await _reportRepository.GetReportsByUserIdAsync(userId);
        //     return StatusCode(resp.StatusCode, resp);
        // }

        // [HttpGet("{userId}/devices")]
        // [Authorize]
        // public async Task<IActionResult> GetDevicesByUserId(int userId)
        // {
        //     var resp = await _deviceRepository.GetDevicesByUserIdAsync(userId);
        //     return StatusCode(resp.StatusCode, resp);
        // }

    }
}
