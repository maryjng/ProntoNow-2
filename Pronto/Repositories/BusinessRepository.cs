using Pronto.Models;
using Pronto.Data;
using Dapper;
using Pronto.Repositories.Interfaces;
using Pronto.DTOs;
using System;
using System.Threading.Tasks;
using System.Net;

namespace Pronto.Repositories
{
    public class BusinessRepository : IBusinessRepository
    {
        private readonly DatabaseHelper _databaseHelper;

        public BusinessRepository(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public async Task<ApiResponse<Business>> GetBusinessByIdAsync(int businessId)
        {
            using var connection = _databaseHelper.CreateConnection();
            var sql = "SELECT BusinessId, Name, Address, Email, PhoneNumber FROM business WHERE BusinessId=@businessId";

            var business = await connection.QuerySingleOrDefaultAsync<Business>(sql, new { BusinessId = businessId });

            if (business == null) 
            {
                return new ApiResponse<Business>
                {
                    Success = false,
                    ErrorMessage = "Business not found.",
                    StatusCode = 404
                };
            }

            return new ApiResponse<Business>
            {
                Success = true,
                Data = business,
                StatusCode = 200
            };
        }

        public async Task<ApiResponse<Business>> CreateBusinessAsync(Business business)
        {
            using var connection = _databaseHelper.CreateConnection();
            var sql = @"INSERT INTO business (Name, Address, Email, PhoneNumber)
                    VALUES (@Name, @Address, @Email, @PhoneNumber);
                    SELECT LAST_INSERT_ID();";

            var businessId = await connection.ExecuteScalarAsync<int>(sql, business);

            if (businessId == 0)
            {
                return new ApiResponse<Business>
                {
                    Success = false,
                    ErrorMessage = "Error creating business.",
                    StatusCode = 500
                };
            }

            business.BusinessId = businessId;

            return new ApiResponse<Business>
            { 
                Success = true, 
                Data = business, 
                StatusCode = 201 
            };
        }

        public async Task<ApiResponse<Business>> UpdateBusinessAsync(int businessId, BusinessUpdateDTO businessUpdateDTO)
        {
            using var connection = _databaseHelper.CreateConnection();
            var businessExists = await connection.QuerySingleOrDefaultAsync<Business>
                ("SELECT * FROM business WHERE BusinessId=@BusinessId", new { BusinessId = businessId });

            if (businessExists == null)
            {
                return new ApiResponse<Business>
                {
                    Success = false,
                    ErrorMessage = "Business not found.",
                    StatusCode = 400
                };
            }

            businessExists.Name = businessUpdateDTO.Name ?? businessExists.Name;
            businessExists.Address = businessUpdateDTO.Address ?? businessExists.Address;
            businessExists.Email = businessUpdateDTO.Email ?? businessExists.Email;
            businessExists.PhoneNumber = businessUpdateDTO.PhoneNumber ?? businessExists.PhoneNumber;

            var sql = @"UPDATE business SET Name = @Name, Address = @Address, Email = @Email, PhoneNumber = @PhoneNumber WHERE BusinessId = @BusinessId";

            var rowsAffected = await connection.ExecuteAsync(sql, businessExists);

            if (rowsAffected == 0)
            {
                return new ApiResponse<Business>
                {
                    Success = false,
                    ErrorMessage = "No changes were made. The data was already up-to-date.",
                    StatusCode = 200
                };
            }

            return new ApiResponse<Business>
            {
                Success = true,
                Data = businessExists,
                StatusCode = 200
            };
        }
    }
}