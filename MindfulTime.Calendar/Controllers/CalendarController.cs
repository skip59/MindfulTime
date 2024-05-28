using Microsoft.AspNetCore.Mvc;
using MindfulTime.Calendar.Application.Interfaces;
using OpenClasses.Calendar;
using OpenClasses.Notification;

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

        [HttpPost]
        public async Task<IActionResult> GetTasks(NUserMT user)
        {
            if (ModelState.IsValid)
            {
                var tasks = await _userTask.ReadTasks(user);
                if (tasks.isError) return BadRequest(tasks.ErrorMessage);
                return Ok(tasks.Data);
            }
            return BadRequest("Входные данные не валидны.");
        }
    }
}
