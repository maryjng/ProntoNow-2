using Pronto.DTOs;
using Pronto.Models;

namespace Pronto.Services
{
    public interface IUserService
    {
        Task<ApiResponse<User>> RegisterUserAsync(UserRegistrationDTO userDto);
        Task<ApiResponse<AuthResultDTO>> LoginAsync(UserLoginDTO userLoginDto);
    }
}
