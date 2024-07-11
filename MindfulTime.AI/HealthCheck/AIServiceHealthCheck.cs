using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MindfulTime.AI
{
    public class AIServiceHealthCheck :IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            return HealthCheckResult.Healthy("AI is OK");
        }
    }
}
