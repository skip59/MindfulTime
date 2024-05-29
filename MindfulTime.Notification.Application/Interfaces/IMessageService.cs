using MindfulTime.Notification.Application.TelegramBot.Models;

namespace MindfulTime.Notification.Application.Interfaces
{
    public interface IMessageService
    {
        public Task<bool> SendMessage(SendModel user);
    }
}
