
namespace MindfulTime.AI.Controllers
{
    [Route("api/[action]")]
    [ApiController]
    public class RecomendationController() : ControllerBase
    {
        //private readonly IRecomendationService _recomendationService = recomendationService;

        //[HttpPost]
        //public async Task<IActionResult> GetRecomendation(UserEventMT model)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        var result = await Task.Run(() => _recomendationService.GetRecommendation(model.Temperature, model.WeatherType, model.StorePoint));
        //        return Ok(result);
        //    }
        //    return BadRequest(ModelState);
        //}
    }
}
