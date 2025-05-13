using Microsoft.AspNetCore.Mvc;
using Dapper;
using Pronto.Models;
using Pronto.Repositories;
using Pronto.DTOs;
using Pronto.Repositories.Interfaces;

namespace Pronto.Controllers
{
    [ApiController]
    [Route("api/devices")]
    public class DeviceController : ControllerBase
    {
        private readonly DeviceRepository _deviceRepository;
        private readonly ReportRepository _reportRepository;

        public DeviceController(DeviceRepository deviceRepository, ReportRepository reportRepository)
        {
            _deviceRepository = deviceRepository;
            _reportRepository = reportRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDevice(int id)
        {
            var resp = await _deviceRepository.GetDeviceByIdAsync(id);

            return StatusCode(resp.StatusCode, resp);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDevice([FromBody] Device device)
        {
            var resp = await _deviceRepository.CreateDeviceAsync(device);

            return StatusCode(resp.StatusCode, resp);
        }

        [HttpPatch("{deviceId}")]
        public async Task<IActionResult> UpdateDevice(int deviceId, DeviceUpdateDTO deviceUpdateDTO)
        {
            var resp = await _deviceRepository.UpdateDeviceAsync(deviceId, deviceUpdateDTO);

            return StatusCode(resp.StatusCode, resp);
        }

        [HttpGet]
        // GET all devices for a User
        public async Task<IActionResult> GetDevicesByUserId([FromQuery] int userId)
        {
            var resp = await _deviceRepository.GetDevicesByUserIdAsync(userId);
            return StatusCode(resp.StatusCode, resp);
        }

    }
}