using System.ComponentModel.DataAnnotations;

namespace OpenClasses
{
    public class UserDto
    {
        public Guid Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
    }
}
