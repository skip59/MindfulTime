using MindfulTime.Notification.Domain.Repository.Entities;
using MindfulTime.Notification.Application.Interfaces;

namespace MindfulTime.Notification.Services
{
    public class TelegramService : IMessageService
    {
        public Task<bool> SendMessage(User user)
        {
            throw new NotImplementedException();
        }
    }
}
