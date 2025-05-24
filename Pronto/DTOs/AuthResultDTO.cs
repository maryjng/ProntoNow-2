using Pronto.Models;

namespace Pronto.DTOs
{
    public class AuthResultDTO
    {
        public User User { get; set; }
        public string Token { get; set; }
    }
}
