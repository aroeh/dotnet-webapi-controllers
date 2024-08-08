using MongoDB.Driver;
using WebApiControllers.Constants;
using WebApiControllers.Models;

namespace WebApiControllers.DataAccess
{
    // class setup using a primary constructor
    public class RestuarantData(ILogger<RestuarantData> log, IMongoService mongo) : IRestuarantData
    {
        private readonly ILogger<RestuarantData> logger = log;


        /// <summary>
        /// Returns a list of all restuarants in the database
        /// </summary>
        /// <returns>Collection of available restuarant records.  Returns empty list if there are no records</returns>
        public async Task<List<Restuarant>> GetAllRestuarants()
        {
            FilterDefinitionBuilder<Restuarant> builder = Builders<Restuarant>.Filter;
            var filter = builder.Where(d => true);

            logger.LogInformation("Finding all restuarants");
            return await mongo.FindMany<Restuarant>(DataAccessConstants.MongoCollection, filter);
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
            FilterDefinitionBuilder<Restuarant> builder = Builders<Restuarant>.Filter;
            var filter = builder.Where(d => d.Name.Contains(name) && d.CuisineType == cuisine);

            logger.LogInformation("Finding restuarants by name and cuisine type");
            return await mongo.FindMany<Restuarant>(DataAccessConstants.MongoCollection, filter);
        }

        /// <summary>
        /// Retrieves a restuarant record based on the matching id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Restuarant record if found.  Returns new Restuarant if not found</returns>
        public async Task<Restuarant> GetRestuarant(string id)
        {
            FilterDefinitionBuilder<Restuarant> builder = Builders<Restuarant>.Filter;
            var filter = builder.Eq(d => d.Id, id);

            logger.LogInformation("Finding restuarant by id");
            return await mongo.FindOne<Restuarant>(DataAccessConstants.MongoCollection, filter);
        }

        /// <summary>
        /// Inserts a new Restuarant Record
        /// </summary>
        /// <param name="rest"></param>
        /// <returns>Restuarant object updated with the new id</returns>
        public async Task<Restuarant> InsertRestuarant(Restuarant rest)
        {
            logger.LogInformation("Adding new restuarant");
            Restuarant newRestuarant = await mongo.InsertOne<Restuarant>(DataAccessConstants.MongoCollection, rest);

            return newRestuarant;
        }

        /// <summary>
        /// Updates and existing restuarant record
        /// </summary>
        /// <param name="rest"></param>
        /// <returns>MongoDb replace results for the update operation</returns>
        public async Task<MongoUpdateResult> UpdateRestuarant(Restuarant rest)
        {
            FilterDefinitionBuilder<Restuarant> builder = Builders<Restuarant>.Filter;
            var filter = builder.Eq(d => d.Id, rest.Id);

            logger.LogInformation("replacing restuarant document");
            return await mongo.ReplaceOne<Restuarant>(DataAccessConstants.MongoCollection, filter, rest);
        }
    }
}
