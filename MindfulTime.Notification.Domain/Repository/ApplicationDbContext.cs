namespace MindfulTime.Notification.Infrastructure.Repository;

public class ApplicationDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<MessageResultEntity> Messages { get; set; }

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
            var user = new UserEntity
            {
                Id = Guid.Parse("242d4794-a0c5-4b23-af6b-dff338ca9b80"),
                Email = "admin@gmail.ru",
                Name = "Admin",
                Password = CryptoService.HashPassword("Admin"),
                Role = "Admin"
            };
            Users.Add(user);
            SaveChanges();
        }

    }


}
