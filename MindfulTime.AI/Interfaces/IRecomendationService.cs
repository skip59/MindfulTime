namespace MindfulTime.AI.Interfaces
{
    public interface IRecomendationService
    {
        public string GetRecommendation(float temperature, string weatherType, float storePoint);
    }
}
