using Microsoft.Extensions.Diagnostics.HealthChecks;
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
                return HealthCheckResult.Healthy();
            }
            catch (Exception ex)
            {

                return HealthCheckResult.Unhealthy("Unable to connect to the database", ex);
            }
        }
    }
}
