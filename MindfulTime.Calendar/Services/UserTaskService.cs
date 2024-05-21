using MindfulTime.Calendar.Domain.Repository.Entities;
using MindfulTime.Calendar.Domain.Repository.Interfaces;
using MindfulTime.Calendar.DTO;
using MindfulTime.Calendar.Interfaces;
using OpenClasses;

namespace MindfulTime.Calendar.Services
{
    public class UserTaskService(IBaseRepository<UserTask> repository) : IUserTaskService
    {
        private readonly IBaseRepository<UserTask> _repository = repository;
        public async Task<BaseResponse<EventDTO>> CreateTask(EventDTO _event)
        {
            var modelTask = Mapper(_event);
            var result = await _repository.CreateAsync(modelTask);
            if(result.IsError) return new BaseResponse<EventDTO>() { ErrorMessage = result.ErrorMessage};
            return new BaseResponse<EventDTO>() { Data = _event };
        }

        public Task<BaseResponse<EventDTO>> DeleteTask(EventDTO _event)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<EventDTO>> ReadTask(EventDTO _event)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<EventDTO>> UpdateTask(EventDTO _event)
        {
            throw new NotImplementedException();
        }

        private static UserTask Mapper(EventDTO eventDTO)
        {
            return new UserTask()
            {
                AllDay = eventDTO.AllDay,
                Description = eventDTO.Description,
                End = eventDTO.End,
                Start = eventDTO.Start,
                EventId = eventDTO.EventId,
                Title = eventDTO.Title,
                UserId = eventDTO.UserId,
                StorePoint = eventDTO.StorePoint,
            };
        }
    }
}
