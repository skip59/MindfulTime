using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MindfulTime.Weather
{
    public class WeatherServiceHealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            return HealthCheckResult.Healthy("Weather is OK");
        }
    }
}
