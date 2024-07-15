using Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MindfulTime.Auth.App.Controllers.User.Login;
using System.Net;

namespace MindfulTime.Auth.App.Controllers.User.Users
{
    public class UsersListHandler : IRequestHandler<UserListQuery, List<MindfulTime.Auth.Infrastructure.Entities.User>>
    {
        private readonly UserManager<MindfulTime.Auth.Infrastructure.Entities.User> _userManager;

        private readonly SignInManager<MindfulTime.Auth.Infrastructure.Entities.User> _signInManager;

        private readonly IJwtGenerator _jwtGenerator;

        public UsersListHandler(UserManager<MindfulTime.Auth.Infrastructure.Entities.User> userManager, SignInManager<Infrastructure.Entities.User> signInManager, IJwtGenerator jwtGenerator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<List<MindfulTime.Auth.Infrastructure.Entities.User>> Handle(UserListQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new RestException(HttpStatusCode.Unauthorized);
            }
            var users = _userManager.Users.ToList();
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (result.Succeeded)
            {
                return users;
            }
            throw new RestException(HttpStatusCode.Unauthorized);
        }
    }
}
