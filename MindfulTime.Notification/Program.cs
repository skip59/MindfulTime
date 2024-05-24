using MassTransit;
using MindfulTime.Notification.Domain.DI;
using System.Reflection;

namespace MindfulTime.Notification
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string connection = builder.Configuration.GetConnectionString("UserDatabase");
            // Add services to the container.
            builder.Services.InitDbContext(connection);
            builder.Services.AddMassTransit(o =>
            {
                o.AddConsumers(Assembly.GetEntryAssembly());
                o.UsingRabbitMq((context, cfg) => cfg.ConfigureEndpoints(context));
            });
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
