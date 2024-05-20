using MassTransit;
using MindfulTime.Calendar.Domain.Repository.Services;
using OpenClasses;

namespace MindfulTime.Calendar.Consumers
{
    public class UsersQueue(UserCalendarRepositoryService repository) : IConsumer<UserMT>
    {
        private readonly UserCalendarRepositoryService _repository = repository;
        public async Task Consume(ConsumeContext<UserMT> context)
        {
            await _repository.CreateAsync(context.Message);
        }
    }
}
