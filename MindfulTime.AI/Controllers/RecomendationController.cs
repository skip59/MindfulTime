
namespace MindfulTime.AI.Controllers
{
    [Route("api/[action]")]
    [ApiController]
    public class RecomendationController(ILogger<RecomendationController> logger) : ControllerBase
    {
        private readonly ILogger<RecomendationController> _logger = logger;

        [HttpPost]
        public async Task<IActionResult> TrainML([FromBody]string fileName)
        {
            _logger.LogInformation("Старт обучения модели");

            try
            {
                MLBuilderService.CreateNewMLModel(fileName);
                _logger.LogInformation("Конец обучения модели");
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Ошибка обучения модели. ", ex);
                return Ok(false);

            }

            return Ok(true);
        }
    }
}
