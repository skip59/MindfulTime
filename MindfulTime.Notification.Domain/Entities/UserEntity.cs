namespace MindfulTime.Notification.Infrastructure.Entities;

public class UserEntity
{
    [Key]
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public string TelegramId { get; set; }
    public bool IsSendMessage { get; set; }
}
