namespace MindfulTime.Notification.Domain.Interfaces;

public interface ISendMessageEmail
{
    public Task<bool> SendMessage(SendModel user);
}
