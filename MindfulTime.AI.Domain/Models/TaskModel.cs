using CsvHelper.Configuration.Attributes;

namespace MindfulTime.AI.Domain.Models;

public static class TaskModel
{
    public class TaskData
    {
        [LoadColumn(0)]
        [Name("temperature")]
        public float Temperature;

        [LoadColumn(1)]
        [Name("type")]
        public string WeatherType;

        [LoadColumn(2)]
        [Name("storepoint")]
        public float StorePoint;

        [LoadColumn(3)]
        [Name("recommendation")]
        public string Recommendation;
    }

    public class TaskRecommendation
    {
        [ColumnName("PredictedLabel")] public string PredictedRecommendation;
    }
}
