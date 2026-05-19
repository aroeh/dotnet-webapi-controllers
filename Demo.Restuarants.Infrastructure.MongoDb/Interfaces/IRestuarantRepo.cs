using Demo.Restuarants.Shared.Models;

namespace Demo.Restuarants.Infrastructure.MongoDb.Interfaces;

public interface IRestuarantRepo
{
    /// <summary>
    /// Query restuarants
    /// </summary>
    /// <param name="queryParameters">Optional - Query parameters to filter restuarants</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Paginated collection of restuarant records matching <paramref name="queryParameters"/></returns>
    Task<PaginationResponse<RestuarantBO>> QueryRestuarantsAsync(FilterQueryParametersBO queryParameters, CancellationToken cancellationToken);

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
    /// <param name="restuarant">Restuarant properties and data</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Restuarant object updated with the new id</returns>
    Task<RestuarantBO> CreateRestuarantAsync(RestuarantBO restuarant, CancellationToken cancellationToken);

    /// <summary>
    /// Create many new Restuarants
    /// </summary>
    /// <param name="restuarants">Collection of new restuarants</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Results for the transaction</returns>
    Task<TransactionResult> CreateManyRestuarantsAsync(RestuarantBO[] restuarants, CancellationToken cancellationToken);

    /// <summary>
    /// Update an existing restuarant
    /// </summary>
    /// <param name="id">Id of the restuarant</param>
    /// <param name="request">Restuarant properties to update</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Results for the transaction</returns>
    Task<TransactionResult> UpdateRestuarantAsync(string id, UpdateRestuarantRequestBO request, CancellationToken cancellationToken);

    /// <summary>
    /// Update the location an existing restuarant
    /// </summary>
    /// <param name="id">Id of the restuarant</param>
    /// <param name="request">Restuarant location properties to update</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Results for the transaction</returns>
    Task<TransactionResult> UpdateRestuarantLocationAsync(string id, UpdateLocationRequestBO request, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a restuarant from the database
    /// </summary>
    /// <param name="id">Id of the restuarant</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Results for the transaction</returns>
    Task<TransactionResult> RemoveRestuarantAsync(string id, CancellationToken cancellationToken);

    /// <summary>
    /// List business hours for a restuarant
    /// </summary>
    /// <param name="restuarantId">Unique Identifier for a restuarant</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Collection of business hours for the restuarant matching <paramref name="restuarantId"/></returns>
    Task<List<RestuarantBusinessHourBO>> ListBusinessHoursAsync(string restuarantId, CancellationToken cancellationToken);

    /// <summary>
    /// Get business hour for a restuarant by id
    /// </summary>
    /// <param name="restuarantId">Unique Identifier for a restuarant</param>
    /// <param name="businessHourId">Unique Identifier for a restuarant business hour record</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Business hour for the restuarant matching <paramref name="restuarantId"/> and <paramref name="businessHourId"/> if not <see langword="null"/></returns>
    Task<RestuarantBusinessHourBO?> GetBusinessHourAsync(string restuarantId, string businessHourId, CancellationToken cancellationToken);

    /// <summary>
    /// Add a new business hour entry to a restuarant
    /// </summary>
    /// <param name="restuarantId">Id of the Restuarant to add the business hour record to.</param>
    /// <param name="businessHour">Restuarant business hour properties and data</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Business hour object updated with the new id</returns>
    Task<RestuarantBusinessHourBO> AddBusinessHourAsync(string restuarantId, RestuarantBusinessHourBO businessHour, CancellationToken cancellationToken);

    /// <summary>
    /// Add many new business hour entries to a restuarant
    /// </summary>
    /// <param name="restuarantId">Id of the Restuarant to add the business hour record to.</param>
    /// <param name="businessHours">Collection of restuarant business hour requests</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Collection of business hours added to the restuarant</returns>
    Task<List<RestuarantBusinessHourBO>> AddManyBusinessHoursAsync(string restuarantId, RestuarantBusinessHourBO[] businessHours, CancellationToken cancellationToken);

    /// <summary>
    /// Updates a business hour entry for a restuarant
    /// </summary>
    /// <param name="restuarantId">Id of the Restuarant to add the business hour record to.</param>
    /// <param name="businessHourId">Unique Identifier for a restuarant business hour</param>
    /// <param name="request">Restuarant business hour properties and data to update</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Results of the update transaction</returns>
    Task<TransactionResult> UpdateBusinessHourAsync(string restuarantId, string businessHourId, UpdateBusinessHourRequestBO request, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a business hour entry from a restuarant
    /// </summary>
    /// <param name="restuarantId">Id of the Restuarant to add the business hour record to.</param>
    /// <param name="businessHourId">Unique Identifier for a restuarant business hour</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Results of the update transaction</returns>
    Task<TransactionResult> RemoveBusinessHourAsync(string restuarantId, string businessHourId, CancellationToken cancellationToken);
}
