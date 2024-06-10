namespace MindfulTime.Notification.Domain.Interfaces;

public interface IMessageService<T> where T : class
{
    public Task<bool> SendMessage(T user);
}
