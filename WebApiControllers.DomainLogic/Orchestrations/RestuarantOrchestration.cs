using Microsoft.Extensions.Logging;
using WebApiControllers.Infrastructure.Repos;
using WebApiControllers.Shared.Models;

namespace WebApiControllers.DomainLogic.Orchestrations;

public class RestuarantOrchestration(ILogger<RestuarantOrchestration> log, IRestuarantRepo repo) : IRestuarantOrchestration
{
    private readonly ILogger<RestuarantOrchestration> logger = log;
    private readonly IRestuarantRepo restuarantRepo = repo;

    /// <summary>
    /// Retrieves all Restuarant from the database
    /// </summary>
    /// <returns>List of Restuarant objects</returns>
    public async Task<List<Restuarant>> GetAllRestuarants()
    {
        logger.LogInformation("Initiating get all restuarants");
        List<Restuarant>? restuarants = await restuarantRepo.GetAllRestuarants();

        if (restuarants is null || restuarants.Count == 0)
        {
            return [];
        }

        return restuarants;
    }

    /// <summary>
    /// Retrieves all Restuarant from the database matching search criteria
    /// </summary>
    /// <param name="name">Search Parameter on the Restuarant Name</param>
    /// <param name="cuisine">Search Parameter on the Restuarant CuisineType</param>
    /// <returns>List of Restuarant objects</returns>
    public async Task<List<Restuarant>> FindRestuarants(string name, string cuisine)
    {
        logger.LogInformation("Initiating find restuarants");
        List<Restuarant> restuarants = await restuarantRepo.FindRestuarants(name, cuisine);

        if (restuarants is null || restuarants.Count == 0)
        {
            return [];
        }

        return restuarants;
    }

    /// <summary>
    /// Retrieves a Restuarant from the database by id
    /// </summary>
    /// <param name="id">Unique Identifier for a restuarant</param>
    /// <returns>Restuarant</returns>
    public async Task<Restuarant> GetRestuarant(string id)
    {
        logger.LogInformation("Initiating get restuarant by id");
        Restuarant restuarant = await restuarantRepo.GetRestuarant(id);

        if (restuarant is null)
        {
            return new Restuarant();
        }

        return restuarant;
    }

    /// <summary>
    /// Inserts a new Restuarant record
    /// </summary>
    /// <param name="restuarant">Restuarant object to insert</param>
    /// <returns>Success status of the insert operation</returns>
    public async Task<bool> InsertRestuarant(Restuarant restuarant)
    {
        logger.LogInformation("Adding new restuarant");
        Restuarant newRestuarant = await restuarantRepo.InsertRestuarant(restuarant);

        logger.LogInformation("Checking insert operation result");
        return newRestuarant is not null && !string.IsNullOrWhiteSpace(newRestuarant.Id);
    }

    /// <summary>
    /// Inserts a new Restuarant record
    /// </summary>
    /// <param name="restuarants">Restuarant array with many items to insert</param>
    /// <returns>Success status of the insert operation</returns>
    public async Task<bool> InsertRestuarants(Restuarant[] restuarants)
    {
        logger.LogInformation("Adding new restuarant");
        Restuarant[] newRestuarants = await restuarantRepo.InsertRestuarants(restuarants);

        logger.LogInformation("Checking insert operation result");
        return newRestuarants is not null
            && newRestuarants.Length > 0
            && newRestuarants.All(r => !string.IsNullOrWhiteSpace(r.Id));
    }

    /// <summary>
    /// Updates a Restuarant record
    /// </summary>
    /// <param name="restuarant">Restuarant object to update</param>
    /// <returns>Success status of the update operation</returns>
    public async Task<bool> UpdateRestuarant(Restuarant restuarant)
    {
        logger.LogInformation("Updating restuarant");
        MongoUpdateResult result = await restuarantRepo.UpdateRestuarant(restuarant);

        logger.LogInformation("Checking update operation result");
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }
}
