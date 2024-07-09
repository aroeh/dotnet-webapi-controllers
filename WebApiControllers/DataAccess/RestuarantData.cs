using MongoDB.Driver;
using WebApiControllers.Constants;
using WebApiControllers.Models;

namespace WebApiControllers.DataAccess
{
    // class setup using a primary constructor
    public class RestuarantData(ILogger<RestuarantData> log, IMongoService mongo) : IRestuarantData
    {
        private readonly IMongoCollection<Restuarant> collection = mongo.Database.GetCollection<Restuarant>(DataAccessConstants.MongoCollection);

        private readonly ILogger<RestuarantData> logger = log;


        /// <summary>
        /// Returns a list of all restuarants in the database
        /// </summary>
        /// <returns>Collection of available restuarant records.  Returns empty list if there are no records</returns>
        public async Task<List<Restuarant>> GetAllRestuarants()
        {
            logger.LogInformation("Finding all restuarants");
            return await collection.Find(d => true).ToListAsync();
        }

        /// <summary>
        /// Simple method for finding restuarants by name and type of cuisine.
        /// This could be enhanced to include more criteria like location
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cuisine"></param>
        /// <returns>Collection of available restuarant records.  Returns empty list if there are no records found matching criteria</returns>
        public async Task<List<Restuarant>> FindRestuarants(string name, string cuisine)
        {
            logger.LogInformation("Finding restuarants by name and cuisine type");
            return await collection.Find(d => d.Name.Contains(name) && d.CuisineType == cuisine).ToListAsync();
        }

        /// <summary>
        /// Retrieves a restuarant record based on the matching id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Restuarant record if found.  Returns new Restuarant if not found</returns>
        public async Task<Restuarant> GetRestuarant(string id)
        {
            logger.LogInformation("Finding restuarant by id");
            return await collection.Find(d => d.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Inserts a new Restuarant Record
        /// </summary>
        /// <param name="rest"></param>
        /// <returns>Restuarant object updated with the new id</returns>
        public async Task<Restuarant> InsertRestuarant(Restuarant rest)
        {
            logger.LogInformation("inserting data");
            await collection.InsertOneAsync(rest);

            return rest;
        }

        /// <summary>
        /// Updates and existing restuarant record
        /// </summary>
        /// <param name="rest"></param>
        /// <returns>MongoDb replace results for the update operation</returns>
        public async Task<ReplaceOneResult> UpdateRestuarant(Restuarant rest)
        {
            logger.LogInformation("starting replace operation");
            ReplaceOneResult result = await collection.ReplaceOneAsync(d => d.Id == rest.Id, rest);

            logger.LogInformation("operation completed...returning result");
            return result;
        }
    }
}
