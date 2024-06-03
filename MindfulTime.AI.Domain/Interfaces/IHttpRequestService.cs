namespace MindfulTime.AI.Domain.Interfaces;

public interface IHttpRequestService
{
    public Task<string> HttpRequest(string URL, StringContent content);
}
