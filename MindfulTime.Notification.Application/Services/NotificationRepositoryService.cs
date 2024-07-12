namespace MindfulTime.Notification.Domain.Repository.Services;

public class NotificationRepositoryService : IBaseRepository<MessageResultEntity>
{
    public Task<BaseResponse<MessageResultEntity>> CreateAsync(MessageResultEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse<MessageResultEntity>> DeleteAsync(MessageResultEntity entity)
    {
        throw new NotImplementedException();
    }

    public IQueryable<MessageResultEntity> ReadAsync()
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse<MessageResultEntity>> UpdateAsync(MessageResultEntity entity)
    {
        throw new NotImplementedException();
    }
}
