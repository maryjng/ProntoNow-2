
namespace Pronto.DTOs
{ 
    public class UserUpdateDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int? BusinessId { get; set; }
        public string CurrentPassword { get; set; }
    }
}