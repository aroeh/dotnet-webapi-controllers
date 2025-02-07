using Microsoft.Extensions.Diagnostics.HealthChecks;
using WebApiControllers.Infrastructure.Extensions;


namespace WebApiControllers.Health;

internal class CustomMongoDbHealthCheck(IDatabaseWrapper mongo) : IHealthCheck
{
    private readonly IDatabaseWrapper mongoService = mongo;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken token = new())
    {
        try
        {
            // the mongoService returns a Dictionary of data that can be passed into the health check result and displayed in the response
            Dictionary<string, object> connectionCheckResults = await mongoService.ConnectionEstablished();

            // check for a duration of time on the connection - is past a certain point, consider the service degraded
            if(connectionCheckResults.TryGetValue("TestDuration", out object? value))
            {
                TimeSpan duration = (TimeSpan)value;
                if(duration.Seconds > 4)
                {
                    return HealthCheckResult.Degraded("Database Connection is Degraded - Response from the connection is Slow", null, connectionCheckResults);
                }
            }

            return HealthCheckResult.Healthy("Database Connection is Healthy", connectionCheckResults);
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Unable to connect to the database", ex);
        }
    }
}
