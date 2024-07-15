using Microsoft.AspNetCore.Hosting;
using Microsoft.OpenApi.Models;
using MindfulTime.Auth.App;
using System.Text.RegularExpressions;
using Serilog;

namespace MindfulTime.Auth;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services));

        string connection = builder.Configuration.GetConnectionString("UserDatabase");
        builder.Services.AppAuthContext(connection);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(options =>
        {
            options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            options.CustomOperationIds(e => $"{Regex.Replace(e.RelativePath, "{|}", "")}{e.HttpMethod}");
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "MindfulTime.Auth",
                Version = "v1",
                Description = $"Service Authorization"
            });
            var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly).ToList();
            xmlFiles.ForEach(xmlFile => options.IncludeXmlComments(xmlFile));

            options.SchemaFilter<ExampleSchemaFilter>();

        });
        builder.Services.AddSwaggerGenNewtonsoftSupport();

        var app = builder.Build();

        if (!app.Environment.IsProduction())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger(o => o.SerializeAsV2 = true);
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"MindfulTime.Auth v1");
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
