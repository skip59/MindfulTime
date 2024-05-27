using Microsoft.AspNetCore.Mvc;
using MindfulTime.AI.Interfaces;
using MindfulTime.AI.Models;

namespace MindfulTime.AI.Controllers
{
    [Route("api/[action]")]
    [ApiController]
    public class RecomendationController(IRecomendationService recomendationService) : ControllerBase
    {
        private readonly IRecomendationService _recomendationService = recomendationService;

        [HttpPost]
        public async Task<IActionResult> GetRecomendation(InputModelDTO model)
        {
            if (ModelState.IsValid)
            {
                var result = await Task.Run(() => _recomendationService.GetRecommendation(model.Temperature, model.WeatherType, model.StorePoint));
                return Ok(result);
            }
            return BadRequest(ModelState);
        }
    }
}
