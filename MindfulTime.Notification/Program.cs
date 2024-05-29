using MindfulTime.Notification.Application.DI;
using PRTelegramBot.Configs;
using PRTelegramBot.Core;
using PRTelegramBot.Extensions;


namespace MindfulTime.Notification
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string connection = builder.Configuration.GetConnectionString("UserDatabase");
            // Add services to the container.
            builder.Services.AppNotificationContext(connection);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddTransientBotHandlers();
            void PrBotInstance_OnLogError(Exception ex, long? id)
            {
                Console.WriteLine(ex.Message, id);
            }

            void PrBotInstance_OnLogCommon(string msg, Enum typeEvent, ConsoleColor color)
            {
                Console.WriteLine(msg, typeEvent);
            }
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

            var prBotInstance = new PRBot(new TelegramConfig
            {
                Token = "6836849143:AAHFUyavwOBbnlfcHDOGe5xDbRyISE_K7bo",
                ClearUpdatesOnStart = true,
                BotId = 0,
            },
            app.Services.GetService<IServiceProvider>());
            prBotInstance.OnLogCommon += PrBotInstance_OnLogCommon;
            prBotInstance.OnLogError += PrBotInstance_OnLogError;
            await prBotInstance.Start();

            app.Run();
        }
    }
}
