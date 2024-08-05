using MongoDB.Driver;
using WebApiControllers.Constants;

namespace WebApiControllers.DataAccess
{
    public class MongoService : IMongoService
    {
        /// <summary>
        /// Instance of the MongoClient object
        /// </summary>
        public MongoClient Client { get; private set; }

        /// <summary>
        /// Database instance from the client connection
        /// </summary>
        public IMongoDatabase Database { get; private set; }

        public MongoService(IConfiguration config)
        {
            // provide the key value to use to lookup the connection string from secrets

            // for this project depending on your resources available - you will need to setup the connection string
            
            // Local secrets - uncomment this line if using local secrets config to store the connection string
            Client = new(config.GetValue<string>(DataAccessConstants.MongoConn));

            // Environment Variable - uncomment this line if passing in the connection string via an env variable
            // probably most commonly used with local containers for simplicity.  Ideally, this will pulled from secrets
            // Client = new(Environment.GetEnvironmentVariable(DataAccessConstants.MongoConn));

            // configure the client and set a database name ideally from a constants file
            Database = Client.GetDatabase(DataAccessConstants.MongoDatabase);
        }
    }
}
