using MindfulTime.Notification.Domain.Interfaces;
using MindfulTime.Notification.Domain.Services;

namespace MindfulTime.Notification.Domain.Extensions;

public static class DependencyInjection
{
    public static void InitDbContext(this IServiceCollection service, string Connection = null!)
    {
        if (string.IsNullOrEmpty(Connection)) return;
        service.AddDbContext<ApplicationDbContext>(o => o.UseNpgsql(Connection));
        service.AddTransient<UserNotificationRepositoryService>();
        service.AddTransient<IBaseRepository<Message>, NotificationRepositoryService>();
    }
}
