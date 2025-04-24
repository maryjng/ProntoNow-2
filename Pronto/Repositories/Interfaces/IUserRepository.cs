using Pronto.Models;
using Pronto.DTOs;

namespace Pronto.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<ApiResponse<User>> CreateUserAsync(User user);
        Task<User?> GetUserByEmailAsync(string email);
        Task<ApiResponse<User>> GetUserByIdAsync(int userId);
        Task<ApiResponse<User>> UpdateUserAsync(int userId, UserUpdateDTO updatedUserDTO);
    }
}
