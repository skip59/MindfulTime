namespace MindfulTime.Auth.Domain.Extensions;

public static class RabbitExtension
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
