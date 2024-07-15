using Microsoft.AspNetCore.Identity;

namespace MindfulTime.Auth.Infrastructure.Entities;

public class User : IdentityUser<Guid>
{
    public string DisplayName { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public string TelegramId { get; set; }
    public bool IsSendMessage { get; set; }
    public string Token { get; set; }
}
