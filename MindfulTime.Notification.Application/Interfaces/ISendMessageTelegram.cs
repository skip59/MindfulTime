namespace MindfulTime.Notification.Domain.Interfaces
{
    public interface ISendMessageTelegram
    {
        public Task<bool> SendMessage(SendModel user);
    }
}
