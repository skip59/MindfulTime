namespace MindfulTime.Auth.App.Interfaces
{
    public interface IJwtGenerator
    {
        string CreateToken(MindfulTime.Auth.Infrastructure.Entities.User user);
    }
}
