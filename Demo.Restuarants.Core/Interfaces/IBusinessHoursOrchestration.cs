using Demo.Restuarants.Shared.Models;

namespace Demo.Restuarants.Core.Interfaces;

public interface IBusinessHoursOrchestration
{
    /// <summary>
    /// List business hours for a restuarant
    /// </summary>
    /// <param name="restuarantId">Unique Identifier for a restuarant</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Collection of business hours for the restuarant matching <paramref name="restuarantId"/></returns>
    Task<List<RestuarantBusinessHourBO>> ListBusinessHoursAsync(string restuarantId, CancellationToken cancellationToken);

    /// <summary>
    /// Get a Restuarant business hour record using the provided id
    /// </summary>
    /// <param name="restuarantId">Unique Identifier for a restuarant</param>
    /// <param name="businessHourId">Unique Identifier for a restuarant business hour</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Business hour for the restuarant matching <paramref name="restuarantId"/> and <paramref name="businessHourId"/> if not <see langword="null"/></returns>
    Task<RestuarantBusinessHourBO?> GetBusinessHourAsync(string restuarantId, string businessHourId, CancellationToken cancellationToken);

    /// <summary>
    /// Add a new business hour entry to a restuarant
    /// </summary>
    /// <param name="restuarantId">Id of the Restuarant to add the business hour record to.</param>
    /// <param name="request">Restuarant business hour properties and data</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Business hour object updated with the new id</returns>
    Task<RestuarantBusinessHourBO> AddBusinessHourAsync(string restuarantId, CreateBusinessHourRequestBO request, CancellationToken cancellationToken);

    /// <summary>
    /// Add many new business hour entries to a restuarant
    /// </summary>
    /// <param name="restuarantId">Id of the Restuarant to add the business hour record to.</param>
    /// <param name="requests">Collection of restuarant business hour requests</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Collection of business hours added to the restuarant</returns>
    Task<List<RestuarantBusinessHourBO>> AddManyBusinessHoursAsync(string restuarantId, CreateBusinessHourRequestBO[] requests, CancellationToken cancellationToken);

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
