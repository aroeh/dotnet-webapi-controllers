using MongoDB.Driver;

namespace WebApiControllers.DataAccess
{
    public interface IMongoService
    {
        /// <summary>
        /// Instance of the MongoClient object
        /// </summary>
        MongoClient Client { get; }

        /// <summary>
        /// Database instance from the client connection
        /// </summary>
        IMongoDatabase Database { get; }
    }
}
