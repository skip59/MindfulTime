using MindfulTime.Calendar.Domain.Consumers;

namespace MindfulTime.Calendar.Domain.Extensions;

public static class MassTransitExtension
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
