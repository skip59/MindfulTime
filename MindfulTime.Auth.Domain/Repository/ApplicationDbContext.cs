using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MindfulTime.Auth.Infrastructure.Repository
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            //InitBaseRecords();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public void InitBaseRecords()
        {

            if (!Users.Any(x => x.Name == "Admin"))
            {
                var user = new User
                {
                    Id = Guid.Parse("242d4794-a0c5-4b23-af6b-dff338ca9b80"),
                    Email = "admin@gmail.ru",
                    Name = "Admin",
                    PasswordHash = CryptoService.HashPassword("Admin"),
                    Role = "Admin"
                };
                Users.Add(user);
                SaveChanges();
            }

        }
    }
}