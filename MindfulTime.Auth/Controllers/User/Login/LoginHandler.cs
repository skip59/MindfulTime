using Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace MindfulTime.Auth.App.Controllers.User.Login
{
    public class LoginHandler : IRequestHandler<LoginQuery, MindfulTime.Auth.Infrastructure.Entities.User>
    {
        private readonly UserManager<MindfulTime.Auth.Infrastructure.Entities.User> _userManager;

        private readonly SignInManager<MindfulTime.Auth.Infrastructure.Entities.User> _signInManager;

        private readonly IJwtGenerator _jwtGenerator;

        public LoginHandler(UserManager<MindfulTime.Auth.Infrastructure.Entities.User> userManager, SignInManager<Infrastructure.Entities.User> signInManager, IJwtGenerator jwtGenerator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<MindfulTime.Auth.Infrastructure.Entities.User> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new RestException(HttpStatusCode.Unauthorized);
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            var roleresult = await _userManager.GetRolesAsync(user);
            if (result.Succeeded)
            {
                return new MindfulTime.Auth.Infrastructure.Entities.User
                {
                    DisplayName = user.DisplayName,
                    Token = _jwtGenerator.CreateToken(user, roleresult.First()),
                    UserName = user.UserName
                };
            }

            throw new RestException(HttpStatusCode.Unauthorized);
        }
    }
}
