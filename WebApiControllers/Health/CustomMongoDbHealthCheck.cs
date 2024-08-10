using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;
using WebApiControllers.DataAccess;

namespace WebApiControllers.Health
{
    public class CustomMongoDbHealthCheck(IMongoService mongo) : IHealthCheck
    {
        private readonly IMongoService mongoService = mongo;

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken token = new())
        {
            try
            {
                var dbNames = await mongoService.Client.ListDatabaseNamesAsync(token);
                
                // it is not recommended to display database names
                // This is just showing how we can pass data to the response context and have it written to the response
                Dictionary<string, object> data = new()
                {
                    { "DB_Names", dbNames.ToList() }
                };
                return HealthCheckResult.Healthy("Database Connection is Healthy", data);
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Unable to connect to the database", ex);
            }
        }
    }
}
