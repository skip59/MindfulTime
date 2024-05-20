using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MindfulTime.Calendar.Domain.Repository;
using MindfulTime.Calendar.Domain.Repository.Entities;
using MindfulTime.Calendar.Domain.Repository.Interfaces;
using MindfulTime.Calendar.Domain.Repository.Services;

namespace MindfulTime.Calendar.Domain.DI
{
    public static class DependencyInjection
    {
        public static void InitDbContext(this IServiceCollection service, string Connection = null!)
        {
            if (string.IsNullOrEmpty(Connection)) return;
            service.AddDbContext<ApplicationDbContext>(o => o.UseNpgsql(Connection));
            service.AddTransient<UserCalendarRepositoryService>();
            service.AddTransient<IBaseRepository<UserTask>, TaskRepositoryService>();
        }
    }
}
