using MindfulTime.Notification.Application.Interfaces;
using MindfulTime.Notification.Application.TelegramBot.Models;

namespace MindfulTime.Notification.Services
{
    public class EmailService : IMessageService
    {
        public Task<bool> SendMessage(SendModel user)
        {
            throw new NotImplementedException();
        }
    }
}
