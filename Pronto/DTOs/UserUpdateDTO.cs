
namespace Pronto.DTOs
{ 
    public class UserUpdateDTO
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int? BusinessId { get; set; }
    }
}