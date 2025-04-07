using Microsoft.AspNetCore.Mvc;
using Dapper;
using MySql.Data.MySqlClient;
using System.Data;
using System.Threading.Tasks;
using Pronto.Models;
using Pronto.Repositories;
using Pronto.DTOs;
using System.Net;

namespace Pronto.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var resp = await _userRepository.GetUserByIdAsync(id);

            return StatusCode(resp.StatusCode, resp);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            var resp = await _userRepository.CreateUserAsync(user);

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
