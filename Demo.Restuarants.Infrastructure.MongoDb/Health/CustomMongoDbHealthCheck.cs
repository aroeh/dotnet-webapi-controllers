using Demo.Restuarants.Infrastructure.MongoDb.Options;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Diagnostics;

namespace Demo.Restuarants.Infrastructure.MongoDb.Health;

public class CustomMongoDbHealthCheck(IOptions<MongoDbOptions> options) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken token = new())
    {
        try
        {
            // the connection check returns a Dictionary of data that can be passed into the health check result and displayed in the response
            ConnectionResults connectionCheckResults = await CheckConnection(token);

            // check if a connection was made
            if (connectionCheckResults.ConnectionResult)
            {
                // check for a duration of time on the connection - is past a certain point, consider the service degraded
                if (connectionCheckResults.TestDuration.Seconds > 4)
                {
                    return HealthCheckResult.Degraded("Database Connection is Degraded - Response from the connection is Slow", null, connectionCheckResults.ToDictionary());
                }

                return HealthCheckResult.Healthy("Database Connection is Healthy", connectionCheckResults.ToDictionary());
            }

            return HealthCheckResult.Unhealthy(connectionCheckResults.Message, null, connectionCheckResults.ToDictionary());
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Unable to connect to the database", ex);
        }
    }

    private async Task<ConnectionResults> CheckConnection(CancellationToken token)
    {
        MongoClient client = new(options.Value.ConnectionString);
        long connectionTestStart = Stopwatch.GetTimestamp();
        TimeSpan connectionTestDuration;

        try
        {
            token.ThrowIfCancellationRequested();
            var dbNames = await client.ListDatabaseNamesAsync(token);
            connectionTestDuration = Stopwatch.GetElapsedTime(connectionTestStart);

            return new ConnectionResults()
            {
                ConnectionResult = dbNames is not null,
                TestDuration = connectionTestDuration
            };
        }
        catch (OperationCanceledException)
        {
            connectionTestDuration = Stopwatch.GetElapsedTime(connectionTestStart);
            return new ConnectionResults()
            {
                ConnectionResult = false,
                TestDuration = connectionTestDuration,
                Message = "Health check operation was cancelled."
            };
        }
        catch (TimeoutException)
        {
            connectionTestDuration = Stopwatch.GetElapsedTime(connectionTestStart);
            return new ConnectionResults()
            {
                ConnectionResult = false,
                TestDuration = connectionTestDuration,
                Message = "A timeout occurred while trying to connect to the database."
            };
        }
        catch (Exception)
        {
            connectionTestDuration = Stopwatch.GetElapsedTime(connectionTestStart);
            return new ConnectionResults()
            {
                ConnectionResult = false,
                TestDuration = connectionTestDuration,
                Message = "An unhandled error occurred while connecting to the database."
            };
        }
    }

    private record ConnectionResults
    {
        internal bool ConnectionResult { get; set; }
        internal TimeSpan TestDuration { get; set; }
        internal string Message { get; set; } = string.Empty;

        internal Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>
            {
                { "Connected", ConnectionResult },
                { "TestDuration", TestDuration },
                { "Message", Message }
            };
        }
    }
}
