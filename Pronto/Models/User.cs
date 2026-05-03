using System;
using System.Text.Json.Serialization;


namespace Pronto.Models
{
    public class User
    {
        public int UserId { get; set; }
        public int? BusinessId { get; set; }
        public string Email { get; set; }
        [JsonIgnore] public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
