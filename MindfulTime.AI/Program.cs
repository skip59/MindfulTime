using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;

namespace MindfulTime.AI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var rootFolder = Directory.GetCurrentDirectory();
            var modelName = "task_recomendation_model.zip";
            if(!File.Exists(Path.Combine(rootFolder, modelName)))
            {
                var isCreated = MLBuilderService.CreateNewMLModel();
                if (!isCreated) throw new Exception("Ошибка создания модели");
            }
            builder.Services.AddTransient<IHttpRequestService, HttpRequestService>();
            builder.Services.AddMassTransit(o =>
            {
                o.AddConsumers(Assembly.GetEntryAssembly());
                o.UsingRabbitMq((context, cfg) => cfg.ConfigureEndpoints(context));
            });

            builder.Services.AddSingleton<IRecomendationService, RecomendationService>();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddHealthChecks().AddCheck<AIServiceHealthCheck>(nameof(AIServiceHealthCheck));

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

            app.MapHealthChecks("/health", new HealthCheckOptions()
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.Run();
        }
    }
}
