namespace MindfulTime.Notification.Domain.Models;

public class SendModel
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public string Message { get; set; }
    public long TelegramId { get; set; }
    public string Email { get; set; }
}
