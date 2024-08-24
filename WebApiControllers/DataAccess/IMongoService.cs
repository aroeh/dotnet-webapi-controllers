using MongoDB.Driver;
using WebApiControllers.Models;

namespace WebApiControllers.DataAccess
{
    public interface IMongoService
    {
        /// <summary>
        /// Checks the connection to the database and returns basic data parameters
        /// </summary>
        /// <returns>Dictionary<string, object></returns>
        Task<Dictionary<string, object>> ConnectionEstablished();

        /// <summary>
        /// Finds items in the specified collection using a filter definition
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName">MongoDb Collection Name</param>
        /// <param name="filter">Expression Filter to match documents</param>
        /// <returns>List T</returns>
        Task<List<T>> FindMany<T>(string collectionName, FilterDefinition<T> filter);

        /// <summary>
        /// Finds one item in the specified collection using a filter definition
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName">MongoDb Collection Name</param>
        /// <param name="filter">Expression Filter to match documents</param>
        /// <returns>T</returns>
        Task<T> FindOne<T>(string collectionName, FilterDefinition<T> filter);

        /// <summary>
        /// Insert a new document into the specified collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName">MongoDb Collection Name</param>
        /// <param name="document">New Document to be inserted</param>
        /// <returns>T</returns>
        Task<T> InsertOne<T>(string collectionName, T document);

        /// <summary>
        /// Replaces a document with a later version using a filter to match the record
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName">MongoDb Collection Name</param>
        /// <param name="filter">Expression Filter to match documents</param>
        /// <param name="document">Latest version of the document</param>
        /// <returns>MongoUpdateResult</returns>
        Task<MongoUpdateResult> ReplaceOne<T>(string collectionName, FilterDefinition<T> filter, T document);
    }
}
