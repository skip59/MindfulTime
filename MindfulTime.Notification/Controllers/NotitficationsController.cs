
using MassTransit;
using OpenClasses.Calendar;
using OpenClasses.Machine;

namespace MindfulTime.Notification.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotitficationsController : ControllerBase
{
    private readonly IPublishEndpoint _publishEndpoint;

    public NotitficationsController(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    [HttpPost("SendMessage")]
    public async Task<IActionResult> SendMessage(EventDTO eventDto)
    {
        var publishUserTask = new UserEventMT
        {
            StorePoint = float.TryParse(eventDto.StorePoint.ToString(), out var storePoint) ? storePoint : 0,
            Temperature = 0,
            UserId = eventDto.UserId,
            WeatherType = string.Empty,
        };
        await _publishEndpoint.Publish(publishUserTask);

        return Ok();
    }
}
