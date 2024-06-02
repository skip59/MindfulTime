namespace MindfulTime.AI.Domain.Models;

public static class TaskModel
{
    public class TaskData
    {
        [LoadColumn(0)] public float Temperature;
        [LoadColumn(1)] public string WeatherType;
        [LoadColumn(2)] public float StorePoint;
        [LoadColumn(3)] public string Recommendation;
    }

    public class TaskRecommendation
    {
        [ColumnName("PredictedLabel")] public string PredictedRecommendation;
    }
}
