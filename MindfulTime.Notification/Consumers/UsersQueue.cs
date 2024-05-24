using MassTransit;
using MindfulTime.Notification.Domain.Repository.Services;
using OpenClasses.Calendar;
using OpenClasses.Notification;

namespace MindfulTime.Calendar.Consumers
{
    public class UsersQueue(UserNotificationRepositoryService repository) : IConsumer<NUserMT>, IConsumer<NUser_del_MT>, IConsumer<NUser_upd_MT>
    {
        private readonly UserNotificationRepositoryService _repository = repository;
        public async Task Consume(ConsumeContext<NUserMT> context)
        {
            await _repository.CreateAsync(context.Message);
        }

        public async Task Consume(ConsumeContext<NUser_del_MT> context)
        {
            await _repository.DeleteAsync(context.Message);
        }

        public async Task Consume(ConsumeContext<NUser_upd_MT> context)
        {
            await _repository.UpdateAsync(context.Message);
        }
    }

}
