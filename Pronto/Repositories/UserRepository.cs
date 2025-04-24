using Pronto.Models;
using Pronto.Data;
using Dapper;
using Pronto.Repositories.Interfaces;
using Pronto.DTOs;
using System.Data;

namespace Pronto.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDatabaseHelper _databaseHelper;

        public UserRepository(IDatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        /// <summary>
        /// This wraps the extension method so that we can moq the db doodickle.  This will need to be done for any, and all, extension method
        /// calls to the database, where we need to mock their response.
        /// </summary>
        public virtual async Task<T?> QuerySingleOrDefaultAsync<T>(IDbConnection connection, string sql, object? param = null) =>
            await connection.QuerySingleOrDefaultAsync<T>(sql, param);

        public async Task<ApiResponse<User>> GetUserByIdAsync(int userId)
        {
            using var connection = _databaseHelper.CreateConnection();
            var sql = "SELECT UserId, BusinessId, Email, PasswordHash, CreatedAt FROM user WHERE UserId = @UserId";

            //var user = await connection.QuerySingleOrDefaultAsync<User>(sql, new { UserId = userId });
            var user = await QuerySingleOrDefaultAsync<User>(connection, sql, new { UserId = userId });

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

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            using var connection = _databaseHelper.CreateConnection();
            var sql = "SELECT UserId, BusinessId, Email, PasswordHash, CreatedAt FROM user WHERE Email = @Email";

            var user = await QuerySingleOrDefaultAsync<User>(connection, sql, new { Email = email });

            return user;
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

        public async Task<ApiResponse<User>> UpdateUserAsync(int userId, UserUpdateDTO updatedUserDTO)
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

            userExists.Email = updatedUserDTO.Email ?? userExists.Email;
            userExists.PasswordHash = updatedUserDTO.PasswordHash ?? userExists.PasswordHash;
            userExists.BusinessId = updatedUserDTO.BusinessId ?? userExists.BusinessId;

            var sql = @"UPDATE user SET BusinessId = @BusinessId, Email = @Email, PasswordHash = @PasswordHash WHERE UserId = @UserId AND (BusinessId != @BusinessId OR Email != @Email OR PasswordHash != @PasswordHash)";

            var rowsAffected = await connection.ExecuteAsync(sql, userExists);

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
                UserId = userExists.UserId,
                BusinessId = userExists.BusinessId,
                Email = userExists.Email,
                CreatedAt = userExists.CreatedAt,
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

//public class CacheConsumer
//{
//    public void DoStuff()
//    {
//        var cache = new Cache<KeyName, MyCoolValueThinger>();
//        var key = new KeyName("doot");
//        var bloop = cache.Get(key);

//        var secondKey = new KeyNameDos("doot");
//        var bloopTheSecond = cache.Get(secondKey);

//        var notValidKey = new SomethingCompletelyDifferent("rawr");
//        cache.Get(notValidKey);
//    }
//}

//    public class KeyName(string key);
//    public class KeyNameDos(string key) : KeyName(key);
//    public readonly record struct SomethingCompletelyDifferent(string key);
//    public class MyCoolValueThinger;
//    public interface ICache<TKey, TValue>
//    {
//        TValue Get(TKey key);
//        void Put(TKey key, TValue value);
//    }
//    public class Cache<TKey, TValue> : ICache<TKey, TValue>
//    {
//        public TValue Get(TKey key) => throw new NotImplementedException();
//        public void Put(TKey key, TValue value) => throw new NotImplementedException();
//    };
