namespace MindfulTime.Calendar.Domain.Services;

public class TaskRepositoryService(ApplicationDbContext context) : IBaseRepository<UserTask>
{
    private readonly ApplicationDbContext _context = context;
    public async Task<BaseResponse<UserTask>> CreateAsync(UserTask entity)
    {
        try
        {
            await _context.Tasks.AddAsync(entity);
            await _context.SaveChangesAsync();

            return new BaseResponse<UserTask> { Data = entity };
        }
        catch (Exception ex)
        {
            return new BaseResponse<UserTask>() { ErrorMessage = ex.Message };
        }
    }

    public async Task<BaseResponse<UserTask>> DeleteAsync(UserTask entity)
    {
        try
        {
            _context.Tasks.Remove(entity);
            await _context.SaveChangesAsync();

            return new BaseResponse<UserTask> { Data = entity };
        }
        catch (Exception ex)
        {
            return new BaseResponse<UserTask>() { ErrorMessage = ex.Message };
        }
    }

    public IQueryable<UserTask> ReadAsync()
    {
        return _context.Tasks;
    }

    public async Task<BaseResponse<UserTask>> UpdateAsync(UserTask entity)
    {
        try
        {
            _context.Tasks.Update(entity);
            await _context.SaveChangesAsync();

            return new BaseResponse<UserTask> { Data = entity };
        }
        catch (Exception ex)
        {
            return new BaseResponse<UserTask>() { ErrorMessage = ex.Message };
        }
    }
}
