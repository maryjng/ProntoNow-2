using Microsoft.AspNetCore.Mvc;
using Dapper;
using MySql.Data.MySqlClient;
using System.Data;
using System.Threading.Tasks;
using Pronto.Models;
using Pronto.Repositories;

namespace Pronto.Controllers
{
    [ApiController]
    [Route("api/devices")]
    public class DeviceController : ControllerBase
    {
        private readonly DeviceRepository _deviceRepository;

        public DeviceController(DeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDevice(int id)
        {
            var device = await _deviceRepository.GetDeviceByIdAsync(id);
            if (device == null)
            {
                return NotFound();
             }

            return Ok(device);
        }


        public async Task<IActionResult> CreateDevice([FromBody] Device device)
        {
            if (device == null)
            {
                return BadRequest();
            }

            var deviceId = await _deviceRepository.CreateDeviceAsync(device);

            return CreatedAtAction(nameof(GetDevice), new { id = deviceId }, device);
        }

        // UPDATE device
    }
}