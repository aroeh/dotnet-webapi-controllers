using WebApiControllers.Models;

namespace WebApiControllers.Repos
{
    public interface IRestuarantRepo
    {
        /// <summary>
        /// Retrieves all Restuarant from the database
        /// </summary>
        /// <returns>List<Restuarant></returns>
        Task<List<Restuarant>> GetAllRestuarants();

        /// <summary>
        /// Retrieves all Restuarant from the database matching search criteria
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cuisine"></param>
        /// <returns>List<Restuarant></returns>
        Task<List<Restuarant>> FindRestuarants(string name, string cuisine);

        /// <summary>
        /// Retrieves a Restuarant from the database by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Restuarant</returns>
        Task<Restuarant> GetRestuarant(string id);

        /// <summary>
        /// Inserts a new Restuarant record
        /// </summary>
        /// <param name="rest"></param>
        /// <returns>bool</returns>
        Task<bool> InsertRestuarant(Restuarant rest);

        /// <summary>
        /// Updates a Restuarant record
        /// </summary>
        /// <param name="rest"></param>
        /// <returns>bool</returns>
        Task<bool> UpdateRestuarant(Restuarant rest);
    }
}
