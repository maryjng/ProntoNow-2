using Microsoft.AspNetCore.Mvc;
using Dapper;
using Pronto.Models;
using Pronto.Repositories;
using Pronto.DTOs;

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
            var resp = await _deviceRepository.GetDeviceByIdAsync(id);

            return StatusCode(resp.StatusCode, resp);
        }


        public async Task<IActionResult> CreateDevice([FromBody] Device device)
        {
            var resp = await _deviceRepository.CreateDeviceAsync(device);

            return StatusCode(resp.StatusCode, resp);
        }

        // UPDATE device
    }
}