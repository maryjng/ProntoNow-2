using Microsoft.AspNetCore.Mvc;
using Dapper;
using MySql.Data.MySqlClient;
using System.Data;
using System.Threading.Tasks;
using Pronto.Models;
using Pronto.Repositories;
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
            if (!resp.Success)
            {
                return StatusCode(resp.StatusCode, resp);
            }

            return Ok(resp);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            var resp = await _userRepository.CreateUserAsync(user);

            if (!resp.Success)
            {
                return StatusCode(resp.StatusCode, resp);
            }

            return StatusCode(resp.StatusCode, resp);
        }

        [HttpPatch("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] User updatedUser)
        {
            if (updatedUser == null)
            {
                return BadRequest("Invalid user data.");
            }

            var resp = await _userRepository.UpdateUserAsync(userId, updatedUser);

            if (!resp.Success)
            {
                return StatusCode(resp.StatusCode, resp);
            }

            return StatusCode(resp.StatusCode, resp);
        }

    }
}
