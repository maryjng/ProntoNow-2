using Pronto.Models;

namespace Pronto.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}
