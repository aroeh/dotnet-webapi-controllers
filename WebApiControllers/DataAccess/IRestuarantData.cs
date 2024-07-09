using MongoDB.Driver;
using WebApiControllers.Models;

namespace WebApiControllers.DataAccess
{
    public interface IRestuarantData
    {
        /// <summary>
        /// Returns a list of all restuarants in the database
        /// </summary>
        /// <returns>Collection of available restuarant records.  Returns empty list if there are no records</returns>
        Task<List<Restuarant>> GetAllRestuarants();

        /// <summary>
        /// Simple method for finding restuarants by name and type of cuisine.
        /// This could be enhanced to include more criteria like location
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cuisine"></param>
        /// <returns>Collection of available restuarant records.  Returns empty list if there are no records found matching criteria</returns>
        Task<List<Restuarant>> FindRestuarants(string name, string cuisine);

        /// <summary>
        /// Retrieves a restuarant record based on the matching id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Restuarant record if found.  Returns new Restuarant if not found</returns>
        Task<Restuarant> GetRestuarant(string id);

        /// <summary>
        /// Inserts a new Restuarant Record
        /// </summary>
        /// <param name="rest"></param>
        /// <returns>Restuarant object updated with the new id</returns>
        Task<Restuarant> InsertRestuarant(Restuarant rest);

        /// <summary>
        /// Updates and existing restuarant record
        /// </summary>
        /// <param name="rest"></param>
        /// <returns>MongoDb replace results for the update operation</returns>
        Task<ReplaceOneResult> UpdateRestuarant(Restuarant rest);
    }
}
