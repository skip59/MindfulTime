using CsvHelper;
using System.Globalization;
using static MindfulTime.AI.Domain.Models.TaskModel;

namespace MindfulTime.AI.Domain.Services;

public class MLBuilderService
{
    private static readonly string ModelPath = Path.Combine(Environment.CurrentDirectory, "task_recomendation_model.zip");
    private static readonly MLContext mlModelContext = new();

    public static bool CreateNewMLModel(string filePath = null)
    {
        try
        {
            var syntheticData = new List<TaskData>();

            if (filePath == null || !File.Exists(filePath))
            {
                syntheticData = GenerateData();
            }
            else
            {
                syntheticData = GenerateDataFromFile(filePath);
            }


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
    new TaskData { Temperature = 18f, WeatherType = "Sunny", StorePoint = 70f, Recommendation = "Следите за своим самочувствием и уровнем энергии" },
    new TaskData { Temperature = 25f, WeatherType = "Sunny", StorePoint = 80f, Recommendation = "Планируйте свой день заранее" },
    new TaskData { Temperature = 14f, WeatherType = "Cloudy", StorePoint = 45f, Recommendation = "Пейте достаточно воды" },
    new TaskData { Temperature = 27f, WeatherType = "Sunny", StorePoint = 92f, Recommendation = "Не забывайте о перерывах" },
    new TaskData { Temperature = 9f, WeatherType = "Snowy", StorePoint = 38f, Recommendation = "Старайтесь поддерживать рабочее пространство в порядке" },
    new TaskData { Temperature = 21f, WeatherType = "Rainy", StorePoint = 62f, Recommendation = "Работайте в удобном для вас темпе" },
    new TaskData { Temperature = 24f, WeatherType = "Sunny", StorePoint = 78f, Recommendation = "Используйте методы визуализации для планирования" },
    new TaskData { Temperature = 16f, WeatherType = "Cloudy", StorePoint = 53f, Recommendation = "Планируйте задачи с учетом приоритетов" },
    new TaskData { Temperature = 19f, WeatherType = "Rainy", StorePoint = 65f, Recommendation = "Оценивайте свои силы адекватно" },
    new TaskData { Temperature = 13f, WeatherType = "Cloudy", StorePoint = 48f, Recommendation = "Старайтесь избегать отвлекающих факторов" },
    new TaskData { Temperature = 26f, WeatherType = "Sunny", StorePoint = 85f, Recommendation = "Используйте техники продуктивности, такие как Pomodoro" },
    new TaskData { Temperature = 5f, WeatherType = "Freezing fog", StorePoint = 60f, Recommendation = "Одевайтесь теплее и избегайте долгих прогулок" },
    new TaskData { Temperature = -2f, WeatherType = "Blizzard", StorePoint = 75f, Recommendation = "Работайте дома и избегайте выхода на улицу без необходимости" },
    new TaskData { Temperature = 15f, WeatherType = "Patchy rain nearby", StorePoint = 55f, Recommendation = "Планируйте задачи с учетом возможных отвлечений" },
    new TaskData { Temperature = 3f, WeatherType = "Light snow", StorePoint = 35f, Recommendation = "Следите за погодой и избегайте переохлаждения" },
    new TaskData { Temperature = 32f, WeatherType = "Hot", StorePoint = 90f, Recommendation = "Пейте много воды и избегайте долгого пребывания на солнце" },
    new TaskData { Temperature = 25f, WeatherType = "Partly Cloudy", StorePoint = 65f, Recommendation = "Пользуйтесь солнцезащитным кремом и оставайтесь в тени" },
    new TaskData { Temperature = 18f, WeatherType = "Overcast", StorePoint = 50f, Recommendation = "Работайте в хорошо освещенном месте для поднятия настроения" },
    new TaskData { Temperature = 10f, WeatherType = "Mist", StorePoint = 45f, Recommendation = "Одевайтесь по погоде и следите за видимостью на дороге" },
    new TaskData { Temperature = 8f, WeatherType = "Fog", StorePoint = 40f, Recommendation = "Будьте внимательны на улице, особенно на дороге" },
    new TaskData { Temperature = 20f, WeatherType = "Moderate rain", StorePoint = 70f, Recommendation = "Используйте дождевик и зонт, чтобы не промокнуть" },
    new TaskData { Temperature = 5f, WeatherType = "Heavy snow", StorePoint = 60f, Recommendation = "Одевайтесь теплее и избегайте переохлаждения" },
    new TaskData { Temperature = 22f, WeatherType = "Light rain shower", StorePoint = 55f, Recommendation = "Планируйте задачи с учетом возможных коротких перерывов" },
    new TaskData { Temperature = 27f, WeatherType = "Torrential rain shower", StorePoint = 80f, Recommendation = "Работайте дома и избегайте выходов на улицу" },
    new TaskData { Temperature = 13f, WeatherType = "Light sleet", StorePoint = 40f, Recommendation = "Будьте осторожны на улице, чтобы не подскользнуться" },
    new TaskData { Temperature = 2f, WeatherType = "Heavy freezing drizzle", StorePoint = 45f, Recommendation = "Избегайте длительных прогулок на холоде и одевайтесь теплее" },
    new TaskData { Temperature = 0f, WeatherType = "Patchy freezing drizzle nearby", StorePoint = 50f, Recommendation = "Одевайтесь по погоде и следите за скользкими дорогами" },
    new TaskData { Temperature = 18f, WeatherType = "Patchy moderate snow", StorePoint = 55f, Recommendation = "Будьте осторожны на улице и избегайте скользких участков" },
    new TaskData { Temperature = 21f, WeatherType = "Moderate or heavy freezing rain", StorePoint = 70f, Recommendation = "Планируйте задачи так, чтобы минимизировать выходы на улицу" },
    new TaskData { Temperature = 29f, WeatherType = "Patchy light rain in area with thunder", StorePoint = 85f, Recommendation = "Избегайте пребывания на открытых пространствах во время грозы" },
    new TaskData { Temperature = 24f, WeatherType = "Moderate or heavy rain in area with thunder", StorePoint = 75f, Recommendation = "Работайте в помещении и будьте осторожны с электричеством" },
    new TaskData { Temperature = 6f, WeatherType = "Patchy light snow in area with thunder", StorePoint = 65f, Recommendation = "Следите за погодой и одевайтесь по погоде" },
    new TaskData { Temperature = 15f, WeatherType = "Moderate or heavy snow in area with thunder", StorePoint = 70f, Recommendation = "Избегайте длительных прогулок и одевайтесь теплее" }
        ];
    }

    private static List<TaskData> GenerateDataFromFile(string filePath)
    {
        using var sr = new StreamReader(filePath);
        using var csv = new CsvReader(sr, CultureInfo.InvariantCulture);

        var records = csv.GetRecords<TaskData>().ToList();

        return records;
    }

}
