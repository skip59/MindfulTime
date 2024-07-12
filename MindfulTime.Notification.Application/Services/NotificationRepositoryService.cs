namespace MindfulTime.Notification.Domain.Repository.Services;

public class NotificationRepositoryService(ApplicationDbContext dbContext) : IBaseRepository<MessageResultEntity>
{
    private readonly ApplicationDbContext dbContext = dbContext;

    public async Task<BaseResponse<MessageResultEntity>> CreateAsync(MessageResultEntity entity)
    {
        try
        {
            await dbContext.Messages.AddAsync(entity);
            await dbContext.SaveChangesAsync();
            return new BaseResponse<MessageResultEntity>() { Data=entity };
        }
        catch (Exception ex)
        {
            return new BaseResponse<MessageResultEntity>() { ErrorMessage = ex.Message };
        }
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
