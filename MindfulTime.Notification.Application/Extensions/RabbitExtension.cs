using MindfulTime.Notification.Domain.Interfaces;
using MindfulTime.Notification.Domain.Services;

namespace MindfulTime.Notification.Domain.Extensions;

public static class RabbitExtension
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
