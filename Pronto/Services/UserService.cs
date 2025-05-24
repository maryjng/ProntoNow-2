using Org.BouncyCastle.Crypto.Generators;
using Pronto.Models;
using Pronto.Repositories.Interfaces;
using Pronto.DTOs;

namespace Pronto.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IJwtTokenService _jwtTokenService;

        public UserService(IUserRepository userRepository, IPasswordHasherService passwordHasherService, IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _passwordHasherService = passwordHasherService;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<ApiResponse<User>> RegisterUserAsync(UserRegistrationDTO userDto)
        {
            var hashedPassword = _passwordHasherService.HashPassword(userDto.Password);

            var user = new User
            {
                BusinessId = userDto.BusinessId,
                PasswordHash = hashedPassword,
                Email = userDto.Email,
            };

            return await _userRepository.CreateUserAsync(user);
        }
        public async Task<ApiResponse<AuthResultDTO>> LoginAsync(UserLoginDTO dto)
        {
            var user = await _userRepository.GetUserByEmailAsync(dto.Email);

            if (user == null)
            {
                return new ApiResponse<AuthResultDTO>
                {
                    Success = false,
                    ErrorMessage = "User does not exist.",
                    StatusCode = 404
                };
            }

            bool isPasswordCorrect = _passwordHasherService.VerifyPassword(dto.Password, user.PasswordHash);

            if (!isPasswordCorrect)
            {
                return new ApiResponse<AuthResultDTO>
                {
                    Success = false,
                    ErrorMessage = "Invalid credentials.",
                    StatusCode = 400
                };
            }

            var token = _jwtTokenService.GenerateToken(user);

            return new ApiResponse<AuthResultDTO>
            {
                Success = true,
                Data = new AuthResultDTO
                {
                    User = user,
                    Token = token
                },
                StatusCode = 200
            };
        }

        public async Task<ApiResponse<User>> UpdateUserAsync(int userId, UserUpdateDTO dto)
        {
            var resp = await _userRepository.GetUserByIdAsync(userId);
            if (!resp.Success || resp.Data == null)
            {
                return new ApiResponse<User>
                {
                    Success = false,
                    ErrorMessage = "User not found.",
                    StatusCode = 404
                };
            }

            var user = resp.Data;


            if ((dto.Email != null || dto.Password != null) && string.IsNullOrEmpty(dto.CurrentPassword))
            {
                return new ApiResponse<User>
                {
                    Success = false,
                    ErrorMessage = "Current password required to change email or password.",
                    StatusCode = 400
                };
            }

            if (!string.IsNullOrEmpty(dto.CurrentPassword))
            {
                var validPassword = _passwordHasherService.VerifyPassword(dto.CurrentPassword, user.PasswordHash);
                if (!validPassword)
                {
                    return new ApiResponse<User>
                    {
                        Success = false,
                        ErrorMessage = "Incorrect current password.",
                        StatusCode = 401
                    };
                }
            }

            if (!string.IsNullOrEmpty(dto.Email)) user.Email = dto.Email;
            if (!string.IsNullOrEmpty(dto.Password)) user.PasswordHash = _passwordHasherService.HashPassword(dto.Password);
            if (dto.BusinessId.HasValue) user.BusinessId = dto.BusinessId;

            await _userRepository.UpdateUserAsync(user);

            return new ApiResponse<User>
            {
                Success = true,
                Data = user,
                StatusCode = 200
            };
        }

    }
}
