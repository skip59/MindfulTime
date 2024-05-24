using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MindfulTime.Notification.Domain.Repository;
using MindfulTime.Notification.Domain.Repository.Entities;
using MindfulTime.Notification.Domain.Repository.Interfaces;
using MindfulTime.Notification.Domain.Repository.Services;

namespace MindfulTime.Notification.Domain.DI
{
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
}
