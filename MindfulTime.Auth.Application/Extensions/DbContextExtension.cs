namespace MindfulTime.Auth.Domain.Extensions;

public static class DbContextExtension
{
    public static void InitDbContext(this IServiceCollection service, string Connection = null!)
    {
        if (string.IsNullOrEmpty(Connection)) return;
        service.AddDbContext<ApplicationDbContext>(o => o.UseNpgsql(Connection));
        service.AddScoped<IBaseRepository<User>, UsersRepositoryService>();
    }

}
