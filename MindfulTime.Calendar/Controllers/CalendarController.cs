
namespace MindfulTime.Calendar.Controllers;

[Route("api/[action]")]
[ApiController]
public class CalendarController(IUserTaskService userTask, ILogger<CalendarController> logger) : ControllerBase
{
    private readonly IUserTaskService _userTask = userTask;
    private readonly ILogger<CalendarController> _logger = logger;

    [HttpPost]
    public async Task<IActionResult> CreateTask(EventDTO eventDto)
    {
        try
        {
            if (!ModelState.IsValid) throw new BadHttpRequestException("Входные данные не валидны.");

            var user = await _userTask.CreateTask(eventDto);
            if (user.isError) throw new BadHttpRequestException(user.ErrorMessage);
            _logger.LogInformation("Задача создалась в календаре");

            return Ok(user.Data);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Произошла ошибка в сервисе календаря при создании задачи.", ex);
        }

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> GetTasks(NUserMT user)
    {
        try
        {
            if (!ModelState.IsValid) throw new BadHttpRequestException("Входные данные не валидны.");

            var tasks = await _userTask.ReadTasks(user);
            if (tasks.isError) throw new BadHttpRequestException(tasks.ErrorMessage);
            _logger.LogInformation("Были получены задачи из календаря");

            return Ok(tasks.Data);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Произошла ошибка в сервисе календаря при получении задачи.", ex);
        }

        return Ok();
    }


    [HttpPost]
    public async Task<IActionResult> DeleteTasks([FromBody]string EventId)
    {
        try
        {
            if (!ModelState.IsValid) throw new BadHttpRequestException("Входные данные не валидны.");

            var tasks = await _userTask.DeleteTask(EventId);
            if (tasks.isError) throw new BadHttpRequestException(tasks.ErrorMessage);
            _logger.LogInformation("Были удалены задачи из календаря");

            return Ok(tasks.Data);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Произошла ошибка в сервисе календаря при удалении задачи.", ex);
        }

        return Ok();
    }
}
