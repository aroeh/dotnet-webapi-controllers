using Demo.Restuarants.Core.Extensions;
using Demo.Restuarants.Core.Interfaces;
using Demo.Restuarants.Infrastructure.MongoDb.Interfaces;
using Demo.Restuarants.Shared.Exceptions;
using Demo.Restuarants.Shared.Models;
using Demo.Restuarants.Shared.Utils;
using Microsoft.Extensions.Logging;

namespace Demo.Restuarants.Core.Orchestrations;

public class BusinessHoursOrchestration(ILogger<BusinessHoursOrchestration> logger, IRestuarantRepo repo) : IBusinessHoursOrchestration
{
    private readonly ILogger<BusinessHoursOrchestration> _logger = logger;
    private readonly IRestuarantRepo _repo = repo;

    /// <summary>
    /// List business hours for a restuarant
    /// </summary>
    /// <param name="restuarantId">Unique Identifier for a restuarant</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Collection of business hours for the restuarant matching <paramref name="restuarantId"/></returns>
    public async Task<List<RestuarantBusinessHourBO>> ListBusinessHoursAsync(string restuarantId, CancellationToken cancellationToken)
    {
        return await _repo.ListBusinessHoursAsync(restuarantId, cancellationToken);
    }

    /// <summary>
    /// Get a Restuarant business hour record using the provided id
    /// </summary>
    /// <param name="restuarantId">Unique Identifier for a restuarant</param>
    /// <param name="businessHourId">Unique Identifier for a restuarant business hour</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Business hour for the restuarant matching <paramref name="restuarantId"/> and <paramref name="businessHourId"/> if not <see langword="null"/></returns>
    public async Task<RestuarantBusinessHourBO?> GetBusinessHourAsync(string restuarantId, string businessHourId, CancellationToken cancellationToken)
    {
        return await _repo.GetBusinessHourAsync(restuarantId, businessHourId, cancellationToken);
    }

    /// <summary>
    /// Add a new business hour entry to a restuarant
    /// </summary>
    /// <param name="restuarantId">Id of the Restuarant to add the business hour record to.</param>
    /// <param name="request">Restuarant business hour properties and data</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Business hour object updated with the new id</returns>
    public async Task<RestuarantBusinessHourBO> AddBusinessHourAsync(string restuarantId, CreateBusinessHourRequestBO request, CancellationToken cancellationToken)
    {
        _ = await _repo.GetRestuarantAsync(restuarantId, cancellationToken) ?? throw new NotFoundException("Restuarant does not exist.  Unable to add business hour.", restuarantId);

        _logger.LogInformation("Adding new restuarant business hour");
        string newId = IdGenerator.GenerateId();
        RestuarantBusinessHourBO businessHour = request.MapToBusinessHours(newId);

        return await _repo.AddBusinessHourAsync(restuarantId, businessHour, cancellationToken);
    }

    /// <summary>
    /// Add many new business hour entries to a restuarant
    /// </summary>
    /// <param name="restuarantId">Id of the Restuarant to add the business hour record to.</param>
    /// <param name="requests">Collection of restuarant business hour requests</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Collection of business hours added to the restuarant</returns>
    public async Task<List<RestuarantBusinessHourBO>> AddManyBusinessHoursAsync(string restuarantId, CreateBusinessHourRequestBO[] requests, CancellationToken cancellationToken)
    {
        _ = await _repo.GetRestuarantAsync(restuarantId, cancellationToken) ?? throw new NotFoundException("Restuarant does not exist.  Unable to add business hour.", restuarantId);

        _logger.LogInformation("Adding new restuarant business hours");
        RestuarantBusinessHourBO[] requestCollection = new RestuarantBusinessHourBO[requests.Length];
        for (int i = 0; i < requests.Length; i++)
        {
            string newId = IdGenerator.GenerateId();
            RestuarantBusinessHourBO newItem = requests[i].MapToBusinessHours(newId);
            requestCollection[i] = newItem;
        }

        return await _repo.AddManyBusinessHoursAsync(restuarantId, requestCollection, cancellationToken);
    }

    /// <summary>
    /// Updates a business hour entry for a restuarant
    /// </summary>
    /// <param name="restuarantId">Id of the Restuarant to add the business hour record to.</param>
    /// <param name="businessHourId">Unique Identifier for a restuarant business hour</param>
    /// <param name="request">Restuarant business hour properties and data to update</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Results of the update transaction</returns>
    public async Task<TransactionResult> UpdateBusinessHourAsync(string restuarantId, string businessHourId, UpdateBusinessHourRequestBO request, CancellationToken cancellationToken)
    {
        _ = await _repo.GetRestuarantAsync(restuarantId, cancellationToken) ?? throw new NotFoundException("Restuarant does not exist.  Unable to add business hour.", restuarantId);

        _logger.LogInformation("Updating restuarant business hour");
        return await _repo.UpdateBusinessHourAsync(restuarantId, businessHourId, request, cancellationToken);
    }

    /// <summary>
    /// Removes a business hour entry from a restuarant
    /// </summary>
    /// <param name="restuarantId">Id of the Restuarant to add the business hour record to.</param>
    /// <param name="businessHourId">Unique Identifier for a restuarant business hour</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Results of the update transaction</returns>
    public async Task<TransactionResult> RemoveBusinessHourAsync(string restuarantId, string businessHourId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Removing restuarant business hour");
        return await _repo.RemoveBusinessHourAsync(restuarantId, businessHourId, cancellationToken);
    }
}
