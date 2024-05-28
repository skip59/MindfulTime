﻿using Microsoft.EntityFrameworkCore;
using MindfulTime.Auth.Domain.Repository.Entities;

namespace MindfulTime.Auth.Domain.Repository
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

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
                    Id = Guid.Parse("242d4794-a0c5-4b23-af6b-dff338ca9b80"),
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
