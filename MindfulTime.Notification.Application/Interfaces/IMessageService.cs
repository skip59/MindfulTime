namespace MindfulTime.Notification.Domain.Interfaces;

public interface IMessageService
{
    public Task<bool> SendMessage(SendModel user);
}
