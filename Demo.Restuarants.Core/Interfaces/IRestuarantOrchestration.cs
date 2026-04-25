using Demo.Restuarants.Shared.Models;

namespace Demo.Restuarants.Core.Interfaces;

public interface IRestuarantOrchestration
{
    /// <summary>
    /// List restuarants
    /// </summary>
    /// <param name="queryParameters">Optional - Query parameters to filter restuarants</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Paginated list of restuarants matching <paramref name="queryParameters"/></returns>
    Task<PaginationResponse<RestuarantBO>> ListRestuarantsAsync(FilterQueryParametersBO queryParameters, CancellationToken cancellationToken);

    /// <summary>
    /// Get restuarant by id
    /// </summary>
    /// <param name="id">Id of the restuarant</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Restuarant if not <see langword="null"/></returns>
    Task<RestuarantBO?> GetRestuarantAsync(string id, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new Restuarant
    /// </summary>
    /// <param name="request">Restuarant properties and data</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Restuarant object updated with the new id</returns>
    Task<RestuarantBO> CreateRestuarantAsync(CreateRestuarantRequestBO request, CancellationToken cancellationToken);

    /// <summary>
    /// Create many new Restuarants
    /// </summary>
    /// <param name="requests">Collection of create restuarant requests</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>MongoDb results for the transaction</returns>
    Task<TransactionResult> CreateManyRestuarantsAsync(CreateRestuarantRequestBO[] requests, CancellationToken cancellationToken);

    /// <summary>
    /// Updates a Restuarant record
    /// </summary>
    /// <param name="id">Id of the restuarant</param>
    /// <param name="request">Restuarant properties to update</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Success result</returns>
    Task<bool> UpdateRestuarantAsync(string id, UpdateRestuarantRequestBO request, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a Restuarant record
    /// </summary>
    /// <param name="id">Id of the restuarant</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Success result</returns>
    Task<bool> RemoveRestuarantAsync(string id, CancellationToken cancellationToken);
}
