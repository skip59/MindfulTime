namespace MindfulTime.AI.Consumers;

public class UserEventQueue(IRecomendationService recomendationService, IHttpRequestService httpRequest, IPublishEndpoint publish, ILogger<UserEventQueue> logger) : IConsumer<UserEventMT>
{
    private readonly IRecomendationService recomendationService = recomendationService;
    private readonly IHttpRequestService httpRequestService = httpRequest;
    private readonly IPublishEndpoint publishEndpoint = publish;
    private readonly ILogger<UserEventQueue> _logger;
    public async Task Consume(ConsumeContext<UserEventMT> context)
    {
        try
        {
            
            var model = new UserEventMT()
            {
                StorePoint = context.Message.StorePoint,
            };
            var city = new WeatherDTO { City = "Tyumen" };
            var HttpResponseWeather = await httpRequestService.HttpRequest(URL.GET_WEATHER, new StringContent(JsonConvert.SerializeObject(city), encoding: System.Text.Encoding.UTF8, "application/json"));
            if (HttpResponseWeather.Contains("FALSE")) throw new Exception(HttpResponseWeather);

            var weatherModel = JsonConvert.DeserializeObject<WeatherResult>(HttpResponseWeather);
            model.Temperature = weatherModel.current.temp_c;
            model.WeatherType = weatherModel.current.condition.text;

            var result = await Task.Run(() => recomendationService.GetRecommendation(model.Temperature, model.WeatherType, model.StorePoint));

            var resultModel = new UserEvent_out_MT()
            {
                UserId = context.Message.UserId,
                StorePoint = model.StorePoint,
                Recomendation = result,
                Temperature = model.Temperature,
                WeatherType = model.WeatherType
            };
           await publishEndpoint.Publish(resultModel);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Не удалось получить рекомендацию.");
        }
        
    }
}
