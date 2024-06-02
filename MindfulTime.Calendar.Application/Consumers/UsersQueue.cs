namespace MindfulTime.Calendar.Domain.Consumers;

public class UsersQueue(UserCalendarRepositoryService repository) : IConsumer<UserMT>, IConsumer<User_del_MT>, IConsumer<User_upd_MT>
{
    private readonly UserCalendarRepositoryService _repository = repository;

    public async Task Consume(ConsumeContext<UserMT> context)
    {
        await _repository.CreateAsync(context.Message);
    }

    public async Task Consume(ConsumeContext<User_del_MT> context)
    {
        await _repository.DeleteAsync(context.Message);
    }

    public async Task Consume(ConsumeContext<User_upd_MT> context)
    {
        await _repository.UpdateAsync(context.Message);
    }
}
