using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MindfulTime.Auth
{
    public class AuthServiceHealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            return HealthCheckResult.Healthy("Auth is OK");
        }
    }
}
