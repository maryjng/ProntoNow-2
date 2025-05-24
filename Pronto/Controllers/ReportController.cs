using Microsoft.AspNetCore.Mvc;
using Dapper;
using Pronto.Models;
using Pronto.Repositories;
using Pronto.DTOs;
using Pronto.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Pronto.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportController : ControllerBase
    {
        private readonly ReportRepository _reportRepository;

        public ReportController(ReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReport(int id)
        {
            var resp = await _reportRepository.GetReportByIdAsync(id);

            return StatusCode(resp.StatusCode, resp);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReport([FromBody] Report report)
        {
            var resp = await _reportRepository.CreateReportAsync(report);

            return StatusCode(resp.StatusCode, resp);
        }

        [HttpPatch("{reportId}")]
        public async Task<IActionResult> UpdateReport(int reportId, ReportUpdateDTO reportUpdateDTO)
        {
            var resp = await _reportRepository.UpdateReportAsync(reportId, reportUpdateDTO);

            return StatusCode(resp.StatusCode, resp);
        }

        [HttpGet]
        //GET all reports given a User OR Device
        public async Task<IActionResult> GetReports([FromQuery] int? userId,[FromQuery] int? deviceId)
        {
            if (userId.HasValue)
            {
                var resp = await _reportRepository.GetReportsByUserIdAsync(userId.Value);
                return StatusCode(resp.StatusCode, resp);
            }
            else if (deviceId.HasValue)
            {
                var resp = await _reportRepository.GetReportsByDeviceIdAsync(deviceId.Value);
                return StatusCode(resp.StatusCode, resp);
            }

            return BadRequest("You must provide either a userId or a deviceId.");
        }
        
    }
}
