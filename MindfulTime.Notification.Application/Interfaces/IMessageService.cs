using MindfulTime.Notification.Domain.Repository.Entities;

namespace MindfulTime.Notification.Application.Interfaces
{
    public interface IMessageService
    {
        public Task<bool> SendMessage(User user);
    }
}
