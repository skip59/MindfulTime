namespace MindfulTime.Notification.Domain.Interfaces;

public interface ISendMessage
{
    public Task<bool> SendMessage(SendModel user);
    public Task<bool> SendMessageTg(SendModel user);
}
