using MassTransit;
using MindfulTime.Calendar.Domain.DI;
using MindfulTime.Calendar.Application.Interfaces;
using MindfulTime.Calendar.Application.Services;
using System.Reflection;

namespace MindfulTime.Calendar
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string connection = builder.Configuration.GetConnectionString("UserDatabase");
            builder.Services.InitDbContext(connection);
            builder.Services.AddMassTransit(o =>
            {
                o.AddConsumers(Assembly.GetEntryAssembly());
                o.UsingRabbitMq((context, cfg) => cfg.ConfigureEndpoints(context));
            });
            // Add services to the container.
            builder.Services.AddTransient<IUserTaskService, UserTaskService>();
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
