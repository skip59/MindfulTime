namespace MindfulTime.AI.Interfaces
{
    public interface IHttpRequestService
    {
        public Task<string> HttpRequest(string URL, StringContent content);
    }
}
