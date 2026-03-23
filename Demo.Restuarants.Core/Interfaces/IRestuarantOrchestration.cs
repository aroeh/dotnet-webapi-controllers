using Demo.Restuarants.Shared.Models;

namespace Demo.Restuarants.Core.Interfaces;

public interface IRestuarantOrchestration
{
    /// <summary>
    /// List restuarants
    /// </summary>
    /// <param name="queryParameters">Optional - Query parameters to filter restuarants</param>
    /// <returns>Paginated list of restuarants matching <paramref name="queryParameters"/></returns>
    Task<PaginationResponse<RestuarantBO>> ListRestuarants(FilterQueryParametersBO queryParameters);

    /// <summary>
    /// Get restuarant by id
    /// </summary>
    /// <param name="id">Id of the restuarant</param>
    /// <returns>Restuarant if not <see langword="null"/></returns>
    Task<RestuarantBO?> GetRestuarant(string id);

    /// <summary>
    /// Creates a new Restuarant
    /// </summary>
    /// <param name="request">Restuarant properties and data</param>
    /// <returns>Restuarant object updated with the new id</returns>
    Task<RestuarantBO> CreateRestuarant(CreateRestuarantRequestBO request);

    /// <summary>
    /// Create many new Restuarants
    /// </summary>
    /// <param name="requests">Collection of create restuarant requests</param>
    /// <returns>MongoDb results for the transaction</returns>
    Task<TransactionResult> CreateManyRestuarants(CreateRestuarantRequestBO[] requests);

    /// <summary>
    /// Updates a Restuarant record
    /// </summary>
    /// <param name="id">Id of the restuarant</param>
    /// <param name="request">Restuarant properties to update</param>
    /// <returns>Success result</returns>
    Task<bool> UpdateRestuarant(string id, UpdateRestuarantRequestBO request);

    /// <summary>
    /// Removes a Restuarant record
    /// </summary>
    /// <param name="id">Id of the restuarant</param>
    /// <returns>Success result</returns>
    Task<bool> RemoveRestuarant(string id);
}
