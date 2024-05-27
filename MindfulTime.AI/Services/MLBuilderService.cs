using Microsoft.ML;
using Microsoft.ML.Data;
using System.Diagnostics;
using static MindfulTime.AI.Models.TaskModel;

namespace MindfulTime.AI.Services
{
    public class MLBuilderService
    {
        private static readonly string ModelPath = Path.Combine(Environment.CurrentDirectory, "task_recomendation_model.zip");
        private static readonly MLContext mlModelContext = new();

        public static bool CreateNewMLModel()
        {
            try
            {
                var syntheticData = GenerateData();

                var dataView = mlModelContext.Data.LoadFromEnumerable(syntheticData);

                var pipeLine = mlModelContext.Transforms.Conversion.MapValueToKey("Label", nameof(TaskData.Recommendation))
                                                                   .Append(mlModelContext.Transforms.Categorical.OneHotEncoding("WeatherTypeEncoded", nameof(TaskData.WeatherType)))
                                                                   .Append(mlModelContext.Transforms.Concatenate("Features", nameof(TaskData.Temperature), nameof(TaskData.StorePoint)))
                                                                   .Append(mlModelContext.Transforms.NormalizeMinMax("Features"));

                var trainer = mlModelContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features").Append(mlModelContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
                var trainingPipeline = pipeLine.Append(trainer);
                Console.WriteLine("Начато обучение модели синтетическими данными...");
                Stopwatch sw = Stopwatch.StartNew();
                var model = trainingPipeline.Fit(dataView);
                sw.Stop();
                Console.WriteLine($"Модель создана и обучена. Затрачено времени: {TimeSpan.FromMilliseconds(sw.ElapsedMilliseconds).TotalSeconds}");
                var predictions = model.Transform(dataView);
                var metrics = mlModelContext.MulticlassClassification.Evaluate(predictions, "Label", "Score");
                SaveMetrics(metrics);
                mlModelContext.Model.Save(model, dataView.Schema, ModelPath);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static void SaveMetrics(MulticlassClassificationMetrics metrics)
        {
            using var writer = new StreamWriter("metrics.txt", false);
            writer.WriteLine($"LogLoss: {metrics.LogLoss}");
            writer.WriteLine($"PerClassLogLoss: {string.Join(", ", metrics.PerClassLogLoss)}");
            writer.WriteLine($"MacroAccuracy: {metrics.MacroAccuracy}");
            writer.WriteLine($"MicroAccuracy: {metrics.MicroAccuracy}");
            writer.WriteLine($"TopKAccuracy: {metrics.TopKAccuracy}");
            Console.WriteLine($"LogLoss: {metrics.LogLoss}");
            Console.WriteLine($"PerClassLogLoss: {string.Join(", ", metrics.PerClassLogLoss)}");
            Console.WriteLine($"MacroAccuracy: {metrics.MacroAccuracy}");
            Console.WriteLine($"MicroAccuracy: {metrics.MicroAccuracy}");
            Console.WriteLine($"TopKAccuracy: {metrics.TopKAccuracy}");
        }
        private static List<TaskData> GenerateData()
        {
            return
            [
                new TaskData { Temperature = 22f, WeatherType = "Sunny", StorePoint = 70f, Recommendation = "Продолжайте в том же духе" },
                new TaskData { Temperature = 18f, WeatherType = "Rainy", StorePoint = 30f, Recommendation = "Попробуйте разбить задачи на более мелкие части" },
                new TaskData { Temperature = 15f, WeatherType = "Cloudy", StorePoint = 50f, Recommendation = "Уделите время планированию задач" },
                new TaskData { Temperature = 25f, WeatherType = "Sunny", StorePoint = 85f, Recommendation = "Отдохните немного, чтобы не перегружаться" },
                new TaskData { Temperature = 10f, WeatherType = "Snowy", StorePoint = 40f, Recommendation = "Работайте в комфортном темпе" },
                new TaskData { Temperature = 28f, WeatherType = "Sunny", StorePoint = 90f, Recommendation = "Используйте техники тайм-менеджмента" },
                new TaskData { Temperature = 20f, WeatherType = "Rainy", StorePoint = 60f, Recommendation = "Сделайте перерыв и займитесь чем-то расслабляющим" },
                new TaskData { Temperature = 30f, WeatherType = "Sunny", StorePoint = 95f, Recommendation = "Попробуйте работать на свежем воздухе" },
                new TaskData { Temperature = 12f, WeatherType = "Cloudy", StorePoint = 35f, Recommendation = "Обратитесь за помощью, если чувствуете усталость" },
                new TaskData { Temperature = 22f, WeatherType = "Rainy", StorePoint = 55f, Recommendation = "Разбейте задачи на подзадачи и выполняйте поэтапно" },
                new TaskData { Temperature = 18f, WeatherType = "Sunny", StorePoint = 75f, Recommendation = "Занимайтесь важными задачами в первую очередь" },
                new TaskData { Temperature = 15f, WeatherType = "Cloudy", StorePoint = 65f, Recommendation = "Работайте в удобное время суток" },
                new TaskData { Temperature = 25f, WeatherType = "Sunny", StorePoint = 80f, Recommendation = "Используйте техники медитации для расслабления" },
                new TaskData { Temperature = 10f, WeatherType = "Snowy", StorePoint = 45f, Recommendation = "Создайте уютную рабочую обстановку" },
                new TaskData { Temperature = 28f, WeatherType = "Sunny", StorePoint = 88f, Recommendation = "Делайте частые, но короткие перерывы" },
                new TaskData { Temperature = 20f, WeatherType = "Rainy", StorePoint = 60f, Recommendation = "Слушайте музыку для повышения продуктивности" },
                new TaskData { Temperature = 30f, WeatherType = "Sunny", StorePoint = 100f, Recommendation = "Завершайте задачи последовательно, не перескакивая" },
                new TaskData { Temperature = 12f, WeatherType = "Cloudy", StorePoint = 50f, Recommendation = "Используйте приложения для управления временем" },
                new TaskData { Temperature = 22f, WeatherType = "Rainy", StorePoint = 55f, Recommendation = "Обратитесь к коллегам за поддержкой и советом" },
                new TaskData { Temperature = 18f, WeatherType = "Sunny", StorePoint = 70f, Recommendation = "Следите за своим самочувствием и уровнем энергии" }
            ];
        }
    }
}
