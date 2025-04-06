using Pronto.Models;
using Pronto.DTOs;
using System.Threading.Tasks;

namespace Pronto.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<ApiResponse<User>> CreateUserAsync(User user);
        Task<ApiResponse<User>> GetUserByIdAsync(int userId);
        Task<ApiResponse<User>> UpdateUserAsync(int userId, UserUpdateDTO updatedUserDTO);
    }
}
