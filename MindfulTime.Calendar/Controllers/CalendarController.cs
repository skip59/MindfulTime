using Microsoft.AspNetCore.Mvc;
using MindfulTime.Calendar.Interfaces;
using OpenClasses;

namespace MindfulTime.Calendar.Controllers
{
    [Route("api/[action]")]
    [ApiController]
    public class CalendarController(IUserTaskService userTask) : ControllerBase
    {
        private readonly IUserTaskService _userTask = userTask;

        [HttpPost]
        public async Task<IActionResult> CreateTask(EventDTO eventDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userTask.CreateTask(eventDto);
                if (user.isError) return BadRequest(user.ErrorMessage);
                return Ok(user.Data);
            }
            return BadRequest("Входные данные не валидны.");
        }
    }
}
