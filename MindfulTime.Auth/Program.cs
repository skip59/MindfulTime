using MassTransit;
using MindfulTime.Auth.Domain.DI;
using MindfulTime.Auth.Interfaces;
using MindfulTime.Auth.Services;

namespace MindfulTime.Auth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string connection = builder.Configuration.GetConnectionString("UserDatabase");
            builder.Services.InitDbContext(connection ?? null);
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddMassTransit(o => 
            { 
                o.UsingRabbitMq(); 
            });
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
