using static MindfulTime.AI.Domain.Models.TaskModel;

namespace MindfulTime.AI.Domain.Services;

public class RecomendationService : IRecomendationService
{
    private readonly MLContext _mlContext;
    private readonly ITransformer _model;
    public RecomendationService()
    {
        _mlContext = new MLContext();
        _model = _mlContext.Model.Load(Path.Combine(Directory.GetCurrentDirectory(), "task_recomendation_model.zip"), out _);
    }

    public string GetRecommendation(float temperature, string weatherType, float storePoint)
    {
        var predictionEngine = _mlContext.Model.CreatePredictionEngine<TaskData, TaskRecommendation>(_model);
        var input = new TaskData
        {
            Temperature = temperature,
            WeatherType = weatherType,
            StorePoint = storePoint,
        };
        var result = predictionEngine.Predict(input);
        return result.PredictedRecommendation;
    }
}
