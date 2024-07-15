using Microsoft.AspNetCore.Mvc;
using MindfulTime.Weather.Interfaces;
using Newtonsoft.Json;
using OpenClasses.Weather;


namespace MindfulTime.Weather.Controllers
{
    [ApiController]
    [Route("api/[action]")]
    public class WeatherController(ILogger<WeatherController> logger, IMainHttpService httpService) : ControllerBase
    {
        private readonly IMainHttpService _httpService = httpService;
        private readonly ILogger<WeatherController> _logger = logger;

        [HttpPost]
        public async Task<IActionResult> GetWeather(WeatherDTO city)
        {
            try
            {
                if (!ModelState.IsValid) throw new BadHttpRequestException("Данные не валидны");

                var url = $"http://api.weatherapi.com/v1/current.json?key=76d3f5ff04d74f5fa20152125242005&q={city.City}&aqi=no";
                var data = await _httpService.CreateRequest(url);

                if (data.Contains("FALSE")) throw new BadHttpRequestException("Запрос некорректен");
                var result = JsonConvert.DeserializeObject<WeatherResult>(data);
                _logger.LogInformation("Данные по погоде были получены");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Ошибка в сервисе погоды.", ex);
            }

            return Ok();
        }
    }
}
