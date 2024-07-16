﻿using Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MindfulTime.Auth.Infrastructure.Repository;
using NuGet.Protocol;
using System.Net;

namespace MindfulTime.Auth.App.Controllers.User.Registration
{
    public class RegistrationHandler : IRequestHandler<RegistrationCommand, MindfulTime.Auth.Infrastructure.Entities.User>
    {
        private readonly UserManager<MindfulTime.Auth.Infrastructure.Entities.User> _userManager;
        private readonly RoleManager<MindfulTime.Auth.Infrastructure.Entities.Role> _roleManager;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly ApplicationDbContext _context;

        public RegistrationHandler(ApplicationDbContext context, UserManager<MindfulTime.Auth.Infrastructure.Entities.User> userManager, IJwtGenerator jwtGenerator, RoleManager<MindfulTime.Auth.Infrastructure.Entities.Role> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<MindfulTime.Auth.Infrastructure.Entities.User> Handle(RegistrationCommand request, CancellationToken cancellationToken)
        {
            request.DisplayName = request.Email;
            request.UserName = request.Email;
            if (await _context.Users.Where(x => x.Email == request.Email).AnyAsync())
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Email = "Email already exist" });
            }

            if (await _context.Users.Where(x => x.UserName == request.UserName).AnyAsync())
            {
                throw new RestException(HttpStatusCode.BadRequest, new { UserName = "UserName already exist" });
            }

            var user = new MindfulTime.Auth.Infrastructure.Entities.User
            {
                DisplayName = request.DisplayName,
                Email = request.Email,
                NormalizedEmail = request.Email,
                UserName = request.UserName,
                Name = request.UserName
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                var role = await _userManager.AddToRoleAsync(user, "User");
                return new MindfulTime.Auth.Infrastructure.Entities.User
                {
                    DisplayName = user.DisplayName,
                    Token = _jwtGenerator.CreateToken(user, "User"),
                    UserName = user.UserName,
                    Role = "User"
                };
            }

            throw new Exception("Client creation failed");
        }
    }
}