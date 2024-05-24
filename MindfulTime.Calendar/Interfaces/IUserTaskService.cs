using MindfulTime.Calendar.DTO;
using OpenClasses.Calendar;
using OpenClasses.Notification;

namespace MindfulTime.Calendar.Interfaces
{
    public interface IUserTaskService
    {
        public Task<BaseResponse<EventDTO>> CreateTask(EventDTO _event);
        public Task<BaseResponse<EventDTO>> DeleteTask(EventDTO _event);
        public Task<BaseResponse<EventDTO>> UpdateTask(EventDTO _event);
        public Task<BaseResponse<List<EventDTO>>> ReadTasks(NUserMT user);
    }
}
