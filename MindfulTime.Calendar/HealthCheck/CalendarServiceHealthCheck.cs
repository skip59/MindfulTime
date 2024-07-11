using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MindfulTime.Calendar
{
    public class CalendarServiceHealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            return HealthCheckResult.Healthy("Calendar is OK");
        }
    }
}
