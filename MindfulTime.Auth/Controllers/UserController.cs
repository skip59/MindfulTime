using Microsoft.AspNetCore.Authorization;
using MindfulTime.Auth.App.Controllers.User.Login;
using MindfulTime.Auth.App.Controllers.User.Registration;

namespace MindfulTime.Auth.App.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        [HttpPost("LoginAsync")]
        public async Task<ActionResult<MindfulTime.Auth.Infrastructure.Entities.User>> LoginAsync(LoginQuery query)
        {
            return await Mediator.Send(query);
        }
        [HttpPost("RegistrationAsync")]
        public async Task<ActionResult<MindfulTime.Auth.Infrastructure.Entities.User>> RegistrationAsync(RegistrationCommand command)
        {
            return await Mediator.Send(command);
        }
        [HttpPost("UserList")]
        public async Task<ActionResult<List<MindfulTime.Auth.Infrastructure.Entities.User>>> UserList(UserListQuery query)
        {
            return await Mediator.Send(query);
        }
    }
}
