using Demo.Restuarants.Infrastructure.MongoDb.Options;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Diagnostics;

namespace Demo.Restuarants.Infrastructure.MongoDb.Health;

public class CustomMongoDbHealthCheck(ILogger logger, IOptions<MongoDbOptions> options) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken token = new())
    {
        try
        {
            // the connection check returns a Dictionary of data that can be passed into the health check result and displayed in the response
            Dictionary<string, object> connectionCheckResults = await CheckConnection(token);

            // check for a duration of time on the connection - is past a certain point, consider the service degraded
            if (connectionCheckResults.TryGetValue("TestDuration", out object? value))
            {
                TimeSpan duration = (TimeSpan)value;
                if (duration.Seconds > 4)
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

    private async Task<Dictionary<string, object>> CheckConnection(CancellationToken token)
    {
        MongoClient client = new(options.Value.ConnectionString);

        try
        {
            long connectionTestStart = Stopwatch.GetTimestamp();
            var dbNames = await client.ListDatabaseNamesAsync(token);
            TimeSpan connectionTestDuration = Stopwatch.GetElapsedTime(connectionTestStart);

            Dictionary<string, object> data = [];

            if (dbNames is not null)
            {
                data.Add("Connected", true);
                data.Add("TestDuration", connectionTestDuration);
            }

            return data;
        }
        catch (Exception ex)
        {
            logger.LogError(99, ex, "Unable to establish database connection");
            Dictionary<string, object> errorData = new()
            {
                { "ErrorCode", 99 },
                { "Connected", false }
            };
            return errorData;
        }
    }
}
