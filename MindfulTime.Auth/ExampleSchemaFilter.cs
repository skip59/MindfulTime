using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MindfulTime.Auth.App;

internal class ExampleSchemaFilter : ISchemaFilter
{
    private readonly JsonSerializerSettings _settings = new()
    {
        Formatting = Formatting.Indented,
        NullValueHandling = NullValueHandling.Ignore,
        Converters = new List<JsonConverter> { new Newtonsoft.Json.Converters.StringEnumConverter() }
    };

    private static readonly ProblemDetails _problemDetailsObject = new()
    {
        Type = "Microsoft.AspNetCore.Http.BadHttpRequestException",
        Title = "One or more validation errors occurred",
        Status = 400
    };

    private static readonly UserDto _templateDtoObject = new()
    {
        Id = Guid.NewGuid(),
        Name = "Test",
        Email = "test@mail.com",
        Password = "Password",
        IsSendMessage = true,
        Role = "admin",
        TelegramId = "0"
    };

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        object example = context.Type.Name switch
        {
            nameof(ProblemDetails) => _problemDetailsObject,
            nameof(UserDto) => _templateDtoObject,
            _ => null
        };

        if (example is not null)
            schema.Example = new OpenApiString(JsonConvert.SerializeObject(example, _settings));
    }
}
