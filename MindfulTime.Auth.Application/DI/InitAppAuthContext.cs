using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using MindfulTime.Auth.Application.Interfaces;
using MindfulTime.Auth.Application.Services;
using MindfulTime.Auth.Domain.DI;

namespace MindfulTime.Auth.Application.DI
{
    public static class InitAppAuthContext
    {
        public static void AppAuthContext(this IServiceCollection service, string Connection = null!)
        {
            service.InitDbContext(Connection ?? null);
            service.AddMassTransit(o =>
            {
                o.UsingRabbitMq();
            });
            service.AddTransient<IUserService, UserService>();
        }
    }
}
