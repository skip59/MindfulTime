using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MindfulTime.Notification
{
    public class NotificationServiceHealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            return HealthCheckResult.Healthy("Notification is OK");
        }
    }
}
