using System.ComponentModel.DataAnnotations;

namespace OpenClasses.Auth
{
    public class UserDto
    {
        public Guid Id { get; set; }
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string TelegramId { get; set; }
        public bool IsSendMessage { get; set; }
    }
}
