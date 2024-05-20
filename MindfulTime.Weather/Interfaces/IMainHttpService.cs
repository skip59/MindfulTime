namespace MindfulTime.Weather.Interfaces
{
    public interface IMainHttpService
    {
        public Task<string> CreateRequest(string URL);
    }
}
