﻿using MediatR;

namespace MindfulTime.Auth.App.Controllers.User.Login
{
    public class LoginQuery : IRequest<MindfulTime.Auth.Infrastructure.Entities.User>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
