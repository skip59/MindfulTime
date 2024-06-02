namespace MindfulTime.Notification.Domain.Repository.Services;

public class NotificationRepositoryService : IBaseRepository<Message>
{
    public Task<BaseResponse<Message>> CreateAsync(Message entity)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse<Message>> DeleteAsync(Message entity)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Message> ReadAsync()
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse<Message>> UpdateAsync(Message entity)
    {
        throw new NotImplementedException();
    }
}
