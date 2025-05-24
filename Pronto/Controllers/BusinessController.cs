using Microsoft.AspNetCore.Mvc;
using Dapper;
using MySql.Data.MySqlClient;
using System.Data;
using System.Threading.Tasks;
using Pronto.Models;
using Pronto.Repositories;
using Pronto.DTOs;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace Pronto.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/businesses")]
    public class BusinessController : ControllerBase
    {
        private readonly BusinessRepository _businessRepository;

        public BusinessController(BusinessRepository businessRepository)
        {
            _businessRepository = businessRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBusiness(int id)
        {
            var resp = await _businessRepository.GetBusinessByIdAsync(id);

            return StatusCode(resp.StatusCode, resp);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBusiness([FromBody] Business business)
        {
            var resp = await _businessRepository.CreateBusinessAsync(business);

            return StatusCode(resp.StatusCode, resp);
        }

        [HttpPatch("{businessId}")]
        public async Task<IActionResult> UpdateBusiness(int businessId, BusinessUpdateDTO businessUpdateDTO)
        {
            var resp = await _businessRepository.UpdateBusinessAsync(businessId, businessUpdateDTO);

            return StatusCode(resp.StatusCode, resp);
        }
    }
}