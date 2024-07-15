using FluentValidation;

namespace MindfulTime.Auth.App.Controllers.User.Login
{
    public class UserListQueryValidation : AbstractValidator<UserListQuery>
    {
        public UserListQueryValidation()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
