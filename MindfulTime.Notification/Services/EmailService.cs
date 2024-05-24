using MindfulTime.Notification.Domain.Repository.Entities;
using MindfulTime.Notification.Interfaces;

namespace MindfulTime.Notification.Services
{
    public class EmailService : IMessageService
    {
        public Task<bool> SendMessage(User user)
        {
            throw new NotImplementedException();
        }
    }
}
