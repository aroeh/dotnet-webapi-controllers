using Demo.Restuarants.Shared.Models;

namespace Demo.Restuarants.Infrastructure.MongoDb.Interfaces;

public interface IRestuarantRepo
{
    /// <summary>
    /// Query restuarants
    /// </summary>
    /// <param name="queryParameters">Optional - Query parameters to filter restuarants</param>
    /// <returns>Paginated collection of restuarant records matching <paramref name="queryParameters"/></returns>
    Task<PaginationResponse<RestuarantBO>> QueryRestuarants(FilterQueryParametersBO queryParameters);

    /// <summary>
    /// Get restuarant by id
    /// </summary>
    /// <param name="id">Id of the restuarant</param>
    /// <returns>Restuarant if not <see langword="null"/></returns>
    Task<RestuarantBO?> GetRestuarant(string id);

    /// <summary>
    /// Creates a new Restuarant
    /// </summary>
    /// <param name="restuarant">Restuarant properties and data</param>
    /// <returns>Restuarant object updated with the new id</returns>
    Task<RestuarantBO> CreateRestuarant(RestuarantBO restuarant);

    /// <summary>
    /// Create many new Restuarants
    /// </summary>
    /// <param name="restuarants">Collection of new restuarants</param>
    /// <returns>MongoDb results for the transaction</returns>
    Task<TransactionResult> CreateManyRestuarants(RestuarantBO[] restuarants);

    /// <summary>
    /// Update an existing restuarant
    /// </summary>
    /// <param name="id">Id of the restuarant</param>
    /// <param name="request">Restuarant properties to update</param>
    /// <returns>MongoDb results for the transaction</returns>
    Task<TransactionResult> UpdateRestuarant(string id, UpdateRestuarantRequestBO request);

    /// <summary>
    /// Removes a restuarant from the database
    /// </summary>
    /// <param name="id">Id of the restuarant</param>
    /// <returns>MongoDb results for the transaction</returns>
    Task<TransactionResult> RemoveRestuarant(string id);
}
