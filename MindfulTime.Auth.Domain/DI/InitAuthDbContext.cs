using Microsoft.Extensions.DependencyInjection;
using MindfulTime.Auth.Domain.Repository.Entities;
using MindfulTime.Auth.Domain.Repository.Interfaces;
using MindfulTime.Auth.Domain.Repository.Services;
using MindfulTime.Auth.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace MindfulTime.Auth.Domain.DI
{
    public static class InitAuthDbContext
    {
        public static void InitDbContext(this IServiceCollection service, string Connection = null!)
        {
            if (string.IsNullOrEmpty(Connection)) return;
            service.AddDbContext<ApplicationDbContext>(o => o.UseNpgsql(Connection));
            service.AddScoped<IBaseRepository<User>, UsersRepositoryService>();
        }

    }
}
