using MongoDB.Driver;
using WebApiControllers.Constants;
using WebApiControllers.Models;

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

        private readonly ILogger<MongoService> logger;

        /// <summary>
        /// For this project depending on your resources available - you will need to setup the connection string
        /// </summary>
        /// <param name="log"></param>
        /// <param name="config"></param>
        public MongoService(ILogger<MongoService> log, IConfiguration config)
        {
            logger = log;

            // provide the key value to use to lookup the connection string from secrets

            // Local secrets - uncomment this line if using local secrets config to store the connection string
            // Client = new(config.GetValue<string>(DataAccessConstants.MongoConn));

            // Environment Variable - uncomment this line if passing in the connection string via an env variable
            // probably most commonly used with local containers for simplicity.  Ideally, this will pulled from secrets
            logger.LogInformation("Retrieving MongoDB Connection string from ENV Variables");
            logger.LogInformation("Configuring MongoDB Client");
            Client = new(Environment.GetEnvironmentVariable(DataAccessConstants.MongoConn));

            // configure the client and set a database name ideally from a constants file
            logger.LogInformation("Configuring MongoDB Database");
            Database = Client.GetDatabase(DataAccessConstants.MongoDatabase);

            logger.LogInformation("MongoDB Connection established and service ready");
        }

        /// <summary>
        /// Finds items in the specified collection using a filter definition
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName">MongoDb Collection Name</param>
        /// <param name="filter">Expression Filter to match documents</param>
        /// <returns>List T</returns>
        public async Task<List<T>> FindMany<T>(string collectionName, FilterDefinition<T> filter)
        {
            var collection = Database.GetCollection<T>(collectionName);

            logger.LogInformation("Finding items by Filter");
            return await collection.Find(filter).ToListAsync();
        }

        /// <summary>
        /// Finds one item in the specified collection using a filter definition
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName">MongoDb Collection Name</param>
        /// <param name="filter">Expression Filter to match documents</param>
        /// <returns>T</returns>
        public async Task<T> FindOne<T>(string collectionName, FilterDefinition<T> filter)
        {
            var collection = Database.GetCollection<T>(collectionName);

            logger.LogInformation("Finding items by Filter");
            return await collection.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Insert a new document into the specified collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="document"></param>
        /// <returns>T</returns>
        public async Task<T> InsertOne<T>(string collectionName, T document)
        {
            var collection = Database.GetCollection<T>(collectionName);

            logger.LogInformation("inserting new document");
            await collection.InsertOneAsync(document);

            return document;
        }

        /// <summary>
        /// Replaces a document with a later version using a filter to match the record
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName">MongoDb Collection Name</param>
        /// <param name="filter">Expression Filter to match documents</param>
        /// <param name="document">Latest version of the document</param>
        /// <returns>MongoUpdateResult</returns>
        public async Task<MongoUpdateResult> ReplaceOne<T>(string collectionName, FilterDefinition<T> filter, T document)
        {
            var collection = Database.GetCollection<T>(collectionName);

            logger.LogInformation("starting replace operation");
            ReplaceOneResult result = await collection.ReplaceOneAsync(filter, document);

            logger.LogInformation("operation completed...returning result");
            return new MongoUpdateResult
            {
                IsAcknowledged = result.IsAcknowledged,
                ModifiedCount = result.ModifiedCount
            };
        }
    }
}
