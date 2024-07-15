using MediatR;

namespace MindfulTime.Auth.App.Controllers.User.Registration
{
    public class RegistrationCommand : IRequest<MindfulTime.Auth.Infrastructure.Entities.User>
    {
        public string DisplayName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
