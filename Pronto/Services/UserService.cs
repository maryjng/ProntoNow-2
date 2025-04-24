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
    }
}
