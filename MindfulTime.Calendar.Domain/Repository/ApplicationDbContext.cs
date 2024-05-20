using Microsoft.EntityFrameworkCore;
using MindfulTime.Calendar.Domain.Repository.Entities;

namespace MindfulTime.Calendar.Domain.Repository
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserTask> Tasks { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
            InitBaseRecords();
        }
        public void InitBaseRecords()
        {

            if (!Users.Any(x => x.Name == "Admin"))
            {
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = "admin@gmail.ru",
                    Name = "Admin",
                    Password = "Admin",
                    Role = "Admin"
                };
                Users.Add(user);
                SaveChanges();
            }

        }


    }
}
