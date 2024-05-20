using MindfulTime.Weather.Interfaces;

namespace MindfulTime.Weather.Services
{
    public class MainHttpService : IMainHttpService
    {
        public async Task<string> CreateRequest(string URL)
        {
            try
            {
                using HttpClient httpClient = new();
                var response = await httpClient.GetAsync($"{URL}");
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                httpClient.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                return $"FALSE: {ex.Message}";
            }
        }
    }
}
