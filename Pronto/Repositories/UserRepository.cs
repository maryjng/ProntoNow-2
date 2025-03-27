using Pronto.Models;
using Pronto.Data;
using Dapper;

namespace Pronto.Repositories
{
    public class UserRepository
    {
        private readonly DatabaseHelper _databaseHelper;

        public UserRepository(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public async Task<int> CreateUserAsync(User user)
        {
            using var connection = _databaseHelper.CreateConnection();
            var sql = @"INSERT INTO user (business_id, email, password_hash) 
                    VALUES (@BusinessId, @Email, @PasswordHash); 
                    SELECT LAST_INSERT_ID();";
            return await connection.ExecuteScalarAsync<int>(sql, user);
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            using var connection = _databaseHelper.CreateConnection();
            var sql = "SELECT user_id AS UserId, business_id AS BusinessId, email AS Email, password_hash AS PasswordHash, created_at AS CreatedAt FROM user WHERE user_id = @UserId";

            //var sql = "SELECT * FROM user WHERE user_id = @UserId";
            var user = await connection.QuerySingleOrDefaultAsync<User>(sql, new { UserId = userId });
            return user;
        }
    }
}