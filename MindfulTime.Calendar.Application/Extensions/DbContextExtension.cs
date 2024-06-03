namespace MindfulTime.Calendar.Domain.Extensions;

public static class DbContextExtension
{
    public static void InitDbContext(this IServiceCollection service, string Connection = null!)
    {
        if (string.IsNullOrEmpty(Connection)) return;
        service.AddDbContext<ApplicationDbContext>(o => o.UseNpgsql(Connection));
        service.AddTransient<UserCalendarRepositoryService>();
        service.AddTransient<IBaseRepository<UserTask>, TaskRepositoryService>();
    }
}
