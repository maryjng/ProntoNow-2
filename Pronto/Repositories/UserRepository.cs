using Pronto.Models;
using Pronto.Data;
using Dapper;
using Pronto.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace Pronto.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseHelper _databaseHelper;

        public UserRepository(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public async Task<ApiResponse<User>> GetUserByIdAsync(int userId)
        {
            using var connection = _databaseHelper.CreateConnection();
            var sql = "SELECT UserId, BusinessId, Email, PasswordHash, CreatedAt FROM user WHERE UserId = @UserId";

            var user = await connection.QuerySingleOrDefaultAsync<User>(sql, new { UserId = userId });

            if (user == null)
            {
                return new ApiResponse<User>
                {
                    Success = false,
                    ErrorMessage = "User not found.",
                    StatusCode = 404
                };
            }

            return new ApiResponse<User>
            {
                Success = true,
                Data = user,
                StatusCode = 200
            };
        }

        public async Task<ApiResponse<User>> CreateUserAsync(User user)
        {
            using var connection = _databaseHelper.CreateConnection();
            var sql = @"INSERT INTO user (BusinessId, Email, PasswordHash) 
                        VALUES (@BusinessId, @Email, @PasswordHash); 
                        SELECT LAST_INSERT_ID();";

            var userId = await connection.ExecuteScalarAsync<int>(sql, user);

            if (userId == 0)
            {
                return new ApiResponse<User>
                {
                    Success = false,
                    ErrorMessage = "Error creating user.",
                    StatusCode = 500
                };
            }

            user.UserId = userId;
            return new ApiResponse<User>
            {
                Success = true,
                Data = user,
                StatusCode = 201
            };
        }

        public async Task<ApiResponse<User>> UpdateUserAsync(int userId, User updatedUser)
        {
            using var connection = _databaseHelper.CreateConnection();
            var userExists = await connection.QuerySingleOrDefaultAsync<User>("SELECT * FROM user WHERE UserId=@UserId", new { UserId = userId });

            if (userExists == null)
            {
                return new ApiResponse<User>
                {
                    Success = false,
                    ErrorMessage = "User not found.",
                    StatusCode = 404
                };
            }

            var sql = @"UPDATE user SET BusinessId = @BusinessId, Email = @Email, PasswordHash = @PasswordHash WHERE UserId = @UserId";

            var rowsAffected = await connection.ExecuteAsync(sql, updatedUser);

            if (rowsAffected == 0)
            {
                return new ApiResponse<User>
                {
                    Success = false,
                    ErrorMessage = "No changes were made. The data was already up-to-date.",
                    StatusCode = 200
                };
            }

            var updatedUserWithoutPassword = new User
            {
                UserId = updatedUser.UserId,
                BusinessId = updatedUser.BusinessId,
                Email = updatedUser.Email,
                CreatedAt = updatedUser.CreatedAt,
            }; // do not include PasswordHash field in return user

            return new ApiResponse<User>
            {
                Success = true,
                Data = updatedUserWithoutPassword,
                StatusCode = 200
            };
        }
    }
}
