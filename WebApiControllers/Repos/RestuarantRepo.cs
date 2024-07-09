using MongoDB.Driver;
using WebApiControllers.DataAccess;
using WebApiControllers.Models;

namespace WebApiControllers.Repos
{
    // class setup using a primary constructor
    public class RestuarantRepo(ILogger<RestuarantRepo> log, IRestuarantData data) : IRestuarantRepo
    {
        private readonly ILogger<RestuarantRepo> logger = log;
        private readonly IRestuarantData restuarantData = data;

        /// <summary>
        /// Retrieves all Restuarant from the database
        /// </summary>
        /// <returns>List<Restuarant></returns>
        public async Task<List<Restuarant>> GetAllRestuarants()
        {
            logger.LogInformation("Initiating get all restuarants");
            List<Restuarant> restuarants = await restuarantData.GetAllRestuarants();

            if (restuarants == null || restuarants.Count == 0)
            {
                return [];
            }

            return restuarants;
        }

        /// <summary>
        /// Retrieves all Restuarant from the database matching search criteria
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cuisine"></param>
        /// <returns>List<Restuarant></returns>
        public async Task<List<Restuarant>> FindRestuarants(string name, string cuisine)
        {
            logger.LogInformation("Initiating find restuarants");
            List<Restuarant> restuarants = await restuarantData.FindRestuarants(name, cuisine);

            if (restuarants == null || restuarants.Count == 0)
            {
                return [];
            }

            return restuarants;
        }

        /// <summary>
        /// Retrieves a Restuarant from the database by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Restuarant</returns>
        public async Task<Restuarant> GetRestuarant(string id)
        {
            logger.LogInformation("Initiating get restuarant by id");
            Restuarant restuarant = await restuarantData.GetRestuarant(id);

            if (restuarant == null)
            {
                return new Restuarant();
            }

            return restuarant;
        }

        /// <summary>
        /// Inserts a new Restuarant record
        /// </summary>
        /// <param name="rest"></param>
        /// <returns>bool</returns>
        public async Task<bool> InsertRestuarant(Restuarant rest)
        {
            logger.LogInformation("Adding new restuarant");
            Restuarant newRestuarant = await restuarantData.InsertRestuarant(rest);

            logger.LogInformation("Checking insert operation result");
            return newRestuarant != null && !string.IsNullOrWhiteSpace(newRestuarant.Id);
        }

        /// <summary>
        /// Updates a Restuarant record
        /// </summary>
        /// <param name="rest"></param>
        /// <returns>bool</returns>
        public async Task<bool> UpdateRestuarant(Restuarant rest)
        {
            logger.LogInformation("Updating restuarant");
            ReplaceOneResult result = await restuarantData.UpdateRestuarant(rest);

            logger.LogInformation("Checking update operation result");
            return (result.IsAcknowledged && result.ModifiedCount > 0);
        }
    }
}
