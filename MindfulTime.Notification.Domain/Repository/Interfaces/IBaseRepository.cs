namespace MindfulTime.Notification.Domain.Repository.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<BaseResponse<T>> CreateAsync(T entity);
        IQueryable<T> ReadAsync();
        Task<BaseResponse<T>> DeleteAsync(T entity);
        Task<BaseResponse<T>> UpdateAsync(T entity);
    }
}
