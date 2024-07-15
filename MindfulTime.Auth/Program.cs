using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MindfulTime.Auth.App.Controllers.User.Login;
using MindfulTime.Auth.Infrastructure.Entities;
using MindfulTime.Auth.Infrastructure.Repository;
using System.Text;
using MediatR;
using MindfulTime.Auth.Infrastructure.Security;

namespace MindfulTime.Auth;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        string connection = builder.Configuration.GetConnectionString("UserDatabase");
        builder.Services.AppAuthContext(connection);
        //builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(LoginHandler).Assembly));
        builder.Services.AddMediatR(typeof(LoginHandler).Assembly);
        builder.Services.AddControllers();

        var builderIde = builder.Services.AddIdentityCore<User>();
        var identityBuilder = new IdentityBuilder(builderIde.UserType, builderIde.Services);

        identityBuilder.AddEntityFrameworkStores<ApplicationDbContext>();
        identityBuilder.AddSignInManager<SignInManager<User>>();
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


        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddIdentity<User, Role>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();
        builder.Services.AddSwaggerGen();
       
        builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();
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
