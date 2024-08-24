using MongoDB.Driver;
using WebApiControllers.Models;

namespace WebApiControllers.DataAccess
{
    public class DatabaseWrapper(ILoggerFactory logFactory, IConfiguration config) : IDatabaseWrapper
    {
        private readonly MongoService mongoService = new(logFactory, config);


        /// <summary>
        /// Checks the connection to the database and returns basic data parameters
        /// </summary>
        /// <returns>Dictionary<string, object></returns>
        public Task<Dictionary<string, object>> ConnectionEstablished() => mongoService.ConnectionEstablished();

        /// <summary>
        /// Finds items in the specified collection using a filter definition
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName">MongoDb Collection Name</param>
        /// <param name="filter">Expression Filter to match documents</param>
        /// <returns>List T</returns>
        public Task<List<T>> FindMany<T>(string collectionName, FilterDefinition<T> filter) => mongoService.FindMany<T>(collectionName, filter);

        /// <summary>
        /// Finds one item in the specified collection using a filter definition
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName">MongoDb Collection Name</param>
        /// <param name="filter">Expression Filter to match documents</param>
        /// <returns>T</returns>
        public Task<T> FindOne<T>(string collectionName, FilterDefinition<T> filter) => mongoService.FindOne<T>(collectionName, filter);

        /// <summary>
        /// Insert a new document into the specified collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="document"></param>
        /// <returns>T</returns>
        public Task<T> InsertOne<T>(string collectionName, T document) => mongoService.InsertOne<T>(collectionName, document);

        /// <summary>
        /// Replaces a document with a later version using a filter to match the record
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName">MongoDb Collection Name</param>
        /// <param name="filter">Expression Filter to match documents</param>
        /// <param name="document">Latest version of the document</param>
        /// <returns>MongoUpdateResult</returns>
        public Task<MongoUpdateResult> ReplaceOne<T>(string collectionName, FilterDefinition<T> filter, T document) => mongoService.ReplaceOne<T>(collectionName, filter, document);
    }
}
