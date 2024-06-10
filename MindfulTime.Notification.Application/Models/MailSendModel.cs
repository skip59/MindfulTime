namespace MindfulTime.Notification.Domain.Models;

public class MailSendModel
{
    public Guid UserId { get; set; }
    public string Message { get; set; }
    public string Mail { get; set; }
}
