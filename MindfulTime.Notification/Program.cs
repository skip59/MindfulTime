namespace MindfulTime.Notification;


public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        string connection = builder.Configuration.GetConnectionString("NotitficationServiceDatabase");
        // Add services to the container.
        builder.Services.AppNotificationContext(connection);

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddTransientBotHandlers();
       
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

        #region tg_bot
        var telegram = new PRBot(option =>
        {
            option.Token = "7198784583:AAExyHT2C7BLjTWKmvWcXxlp3WOMB5UO3l0";
            option.ClearUpdatesOnStart = true;
            option.WhiteListUsers = new List<long>();
            option.Admins = new List<long>() { };
            option.BotId = 0;
        },
        app.Services.GetService<IServiceProvider>());
        telegram.OnLogCommon += Telegram_OnLogCommon;
        telegram.OnLogError += Telegram_OnLogError;
        await telegram.Start();

        CancellationTokenSource cancellationToken = new CancellationTokenSource();

        var task = Task.Run(() => TelegramCommands.AlwaisTask(telegram.botClient, new TimeSpan(0, 0, seconds: 10), cancellationToken.Token));

        var msg = PRTelegramBot.Helpers.Message.Send(telegram.botClient, 294490085, "Бот начал работать");



        void Telegram_OnLogError(Exception ex, long? id)
        {
            Console.WriteLine(ex.Message, id);
            //PRTelegramBot.Helpers.Message.Send(telegram.botClient, 294490085, $"{DateTime.Now}:{ex}");
        }

        void Telegram_OnLogCommon(string msg, Enum typeEvent, ConsoleColor color)
        {
            Console.WriteLine(msg, typeEvent);
            //PRTelegramBot.Helpers.Message.Send(telegram.botClient, 294490085, $"{DateTime.Now}:{msg}");
        }
        #endregion

        app.Run();
    }
}
