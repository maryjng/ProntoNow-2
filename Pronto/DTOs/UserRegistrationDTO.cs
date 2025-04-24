using System.ComponentModel.DataAnnotations;

namespace Pronto.DTOs
{
    public class UserRegistrationDTO
    {
        [Required]
        public int BusinessId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
