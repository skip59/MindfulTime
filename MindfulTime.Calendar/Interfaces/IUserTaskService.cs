using MindfulTime.Calendar.DTO;
using OpenClasses;

namespace MindfulTime.Calendar.Interfaces
{
    public interface IUserTaskService
    {
        public Task<BaseResponse<EventDTO>> CreateTask(EventDTO _event);
        public Task<BaseResponse<EventDTO>> DeleteTask(EventDTO _event);
        public Task<BaseResponse<EventDTO>> UpdateTask(EventDTO _event);
        public Task<BaseResponse<EventDTO>> ReadTask(EventDTO _event);
    }
}
