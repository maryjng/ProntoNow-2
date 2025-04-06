using Pronto.Models;
using Pronto.Data;
using Dapper;
using Pronto.Repositories.Interfaces;


namespace Pronto.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly DatabaseHelper _databaseHelper;

        public UserRepository(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public async Task<int> CreateUserAsync(User user)
        {
            using var connection = _databaseHelper.CreateConnection();
            var sql = @"INSERT INTO user (BusinessId, Email, PasswordHash) 
                    VALUES (@BusinessId, @Email, @PasswordHash); 
                    SELECT LAST_INSERT_ID();";
            return await connection.ExecuteScalarAsync<int>(sql, user);
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            using var connection = _databaseHelper.CreateConnection();
            var sql = "SELECT UserId, BusinessId, Email, PasswordHash, CreatedAt FROM user WHERE UserId = @UserId";

            //var sql = "SELECT * FROM user WHERE user_id = @UserId";
            var user = await connection.QuerySingleOrDefaultAsync<User>(sql, new { UserId = userId });
            return user;
        }
    }
}