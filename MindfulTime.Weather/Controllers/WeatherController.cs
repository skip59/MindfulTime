using Microsoft.AspNetCore.Mvc;
using MindfulTime.Weather.Interfaces;
using MindfulTime.Weather.Models;
using Newtonsoft.Json;
using OpenClasses;


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
            if(!ModelState.IsValid) return BadRequest(ModelState);
            var url = $"http://api.weatherapi.com/v1/current.json?key=76d3f5ff04d74f5fa20152125242005&q={city.City}&aqi=no";
            var data = await _httpService.CreateRequest(url);
            if (data.Contains("FALSE")) return BadRequest();
            var result = JsonConvert.DeserializeObject<WeatherResponse>(data);
            return Ok(result);
        }
    }
}
