using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using MindfulTime.Calendar.Application.Consumers;
using MindfulTime.Calendar.Application.Interfaces;
using MindfulTime.Calendar.Application.Services;
using MindfulTime.Calendar.Domain.DI;

namespace MindfulTime.Calendar.Application.DI
{
    public static class InitAppCalendarContext
    {
        public static void AppCalendarContext(this IServiceCollection service, string Connection = null!)
        {
            service.InitDbContext(Connection ?? null);
            service.AddMassTransit(o =>
            {
                o.AddConsumer<UsersQueue>();
                o.UsingRabbitMq((context, cfg) => cfg.ConfigureEndpoints(context));
            });
            service.AddTransient<IUserTaskService, UserTaskService>();
        }
    }
}
