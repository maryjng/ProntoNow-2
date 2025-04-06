using Pronto.Models;
using System.Threading.Tasks;

namespace Pronto.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<ApiResponse<User>> CreateUserAsync(User user);
        Task<ApiResponse<User>> GetUserByIdAsync(int userId);
        Task<ApiResponse<User>> UpdateUserAsync(int userId, User updatedUser);
    }
}
