using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MindfulTime.Auth.Infrastructure.Entities;
using MindfulTime.Auth.Infrastructure.Repository;
using System.Text;
using MediatR;
using MindfulTime.Auth.Infrastructure.Security;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using MindfulTime.Auth.App.Controllers.User.Login;
using MindfulTime.Auth.App.Controllers.User.Registration;

namespace MindfulTime.Auth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string connection = builder.Configuration.GetConnectionString("UserDatabase");
            builder.Services.AppAuthContext(connection);
            builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseNpgsql(connection));
            builder.Services.AddMediatR(typeof(Program));
            builder.Services.AddControllers();
            builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();
            builder.Services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddMediatR(typeof(LoginHandler).Assembly);
            builder.Services.AddMediatR(typeof(RegistrationHandler).Assembly);

            builder.Services.AddMvc(option =>
            {
                option.EnableEndpointRouting = false;
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                option.Filters.Add(new AuthorizeFilter(policy));
            }).SetCompatibilityVersion(CompatibilityVersion.Latest);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"]));
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                    };
                });

            builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();
            builder.Services.AddScoped<IUserStore<User>, UserStore<User, Role, ApplicationDbContext, Guid>>();
            builder.Services.AddScoped<UserManager<User>>();
            builder.Services.AddScoped<RoleManager<Role>>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}