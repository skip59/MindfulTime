namespace MindfulTime.Notification.Domain.Models;

public class SendModel
{
    public Guid UserId { get; set; }
    public string Message { get; set; }
    public int TelegramId { get; set; }
}
