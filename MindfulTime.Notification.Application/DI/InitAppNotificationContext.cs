using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using MindfulTime.Calendar.Application.Consumers;
using MindfulTime.Notification.Application.Interfaces;
using MindfulTime.Notification.Domain.DI;
using MindfulTime.Notification.Services;

namespace MindfulTime.Notification.Application.DI
{
    public static class InitAppNotificationContext
    {
        public static void AppNotificationContext(this IServiceCollection service, string Connection = null!)
        {
            service.InitDbContext(Connection ?? null);
            service.AddMassTransit(o =>
            {
                o.AddConsumer<UsersNotifyQueue>();
                o.UsingRabbitMq((context, cfg) => cfg.ConfigureEndpoints(context));
            });
            service.AddSingleton<IMessageService, TelegramService>();
        }
    }
}
