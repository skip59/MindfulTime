using MindfulTime.Calendar.Domain.Repository.Entities;
using MindfulTime.Calendar.Domain.Repository.Interfaces;

namespace MindfulTime.Calendar.Domain.Repository.Services
{
    public class TaskRepositoryService : IBaseRepository<UserTask>
    {
        public Task<BaseResponse<UserTask>> CreateAsync(UserTask entity)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<UserTask>> DeleteAsync(UserTask entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<UserTask> ReadAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<UserTask>> UpdateAsync(UserTask entity)
        {
            throw new NotImplementedException();
        }
    }
}
