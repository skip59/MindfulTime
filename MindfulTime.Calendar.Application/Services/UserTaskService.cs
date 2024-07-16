using System.Security.Cryptography.X509Certificates;

namespace MindfulTime.Calendar.Domain.Services;

public class UserTaskService(IBaseRepository<UserTask> repository, IPublishEndpoint publish) : IUserTaskService
{
    private readonly IBaseRepository<UserTask> _repository = repository;
    private readonly IPublishEndpoint _publishEndpoint = publish;
    public async Task<BaseResponse<EventDTO>> CreateTask(EventDTO _event)
    {
        var modelTask = Mapper(_event);
        var result = await _repository.CreateAsync(modelTask);
        if (result.isError) return new BaseResponse<EventDTO>() { ErrorMessage = result.ErrorMessage };
        var publishUserTask = new UserEventMT
        {
            StorePoint = float.TryParse(modelTask.StorePoint.ToString(), out var storePoint) ? storePoint : 0,
            Temperature = 0,
            UserId = modelTask.UserId,
            WeatherType = string.Empty,
        };
        await _publishEndpoint.Publish(publishUserTask);
        return new BaseResponse<EventDTO>() { Data = _event };
    }

    public async Task<BaseResponse<EventDTO>> DeleteTask(string eventId)
    {
        var test = _repository.ReadAsync().ToList();
        var task = test.Where(x => x.EventId.Equals(Guid.Parse(eventId))).FirstOrDefault();
        if (task == null) return new BaseResponse<EventDTO>() { ErrorMessage = "Нет данных" };

        await _repository.DeleteAsync(task);

        return new BaseResponse<EventDTO>()
        {
            Data = new EventDTO() { EventId = Guid.Parse(eventId)},
        };
    }

    public async Task<BaseResponse<List<EventDTO>>> ReadTasks(NUserMT user)
    {
        var tasks = await _repository.ReadAsync().Where(x => x.UserId == user.Id).ToListAsync();
        if (tasks.Count == 0) return new BaseResponse<List<EventDTO>>() { ErrorMessage = "Нет данных" };
        return new BaseResponse<List<EventDTO>>()
        {
            Data = ReverseMapper(tasks)
        };
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

    private static List<EventDTO> ReverseMapper(List<UserTask> userTasks)
    {
        var result = new List<EventDTO>();
        foreach (var userTask in userTasks)
        {
            var eventTask = new EventDTO
            {
                AllDay = userTask.AllDay,
                Description = userTask.Description,
                End = userTask.End,
                Start = userTask.Start,
                EventId = userTask.EventId,
                Title = userTask.Title,
                UserId = userTask.UserId,
            };
            result.Add(eventTask);
        }
        return result;
    }
}
