using Microsoft.AspNetCore.Authorization;
using MindfulTime.Auth.App.Controllers.User.Login;
using MindfulTime.Auth.App.Controllers.User.Registration;

namespace MindfulTime.Auth.App.Controllers
{
    [AllowAnonymous]
    [Route("api/[action]")]
    [ApiController]
    public class UserController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult<MindfulTime.Auth.Infrastructure.Entities.User>> LoginAsync(LoginQuery query)
        {
            return await Mediator.Send(query);
        }
        [HttpPost]
        public async Task<ActionResult<MindfulTime.Auth.Infrastructure.Entities.User>> RegistrationAsync(RegistrationCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
