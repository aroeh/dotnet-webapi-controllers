using Asp.Versioning;
using Demo.Restuarants.API.Constants;
using Demo.Restuarants.API.Extensions;
using Demo.Restuarants.API.Models;
using Demo.Restuarants.Core.Interfaces;
using Demo.Restuarants.Shared.Models;
using DnsClient.Internal;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Restuarants.API.Controllers;

[ApiVersion(ApiVersions.Latest)]
[Route("api/restuarants/{restuarantId}/business-hours")]
public class BusinessHoursController
(
    ILogger<BusinessHoursController> logger,
    IBusinessHoursOrchestration orchestration
) : ApiControllerBase<BusinessHoursController>(logger)
{
    private readonly ILogger<BusinessHoursController> _logger = logger;
    private readonly IBusinessHoursOrchestration _orchestration = orchestration;

    /// <summary>
    /// List business hours for a restuarant
    /// </summary>
    /// <param name="restuarantId">Unique Identifier for a restuarant</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Collection of business hours for the restuarant matching <paramref name="restuarantId"/></returns>
    [HttpGet(Name = RouteConstants.ListBusinessHours)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> ListBusinessHoursAsync([FromRoute] string restuarantId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("List restuarant business hours request received");
        List<RestuarantBusinessHourBO> businessHours = await _orchestration.ListBusinessHoursAsync(restuarantId, cancellationToken);

        return TypedResults.Ok(businessHours);
    }

    /// <summary>
    /// Get a Restuarant business hour record using the provided id
    /// </summary>
    /// <param name="restuarantId">Unique Identifier for a restuarant</param>
    /// <param name="businessHourId">Unique Identifier for a restuarant business hour</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Business hour for the restuarant matching <paramref name="restuarantId"/> and <paramref name="businessHourId"/> if not <see langword="null"/></returns>
    [HttpGet("{businessHourId}", Name = RouteConstants.GetBusinessHour)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetBusinessHourAsync([FromRoute] string restuarantId, [FromRoute] string businessHourId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Get restuarant request received");
        RestuarantBusinessHourBO? businessHour = await _orchestration.GetBusinessHourAsync(restuarantId, businessHourId, cancellationToken);

        if (businessHour is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(businessHour);
    }

    /// <summary>
    /// Creates a new business hour entry for a restuarant
    /// </summary>
    /// <param name="restuarantId">Id of the Restuarant to add the business hour record to.</param>
    /// <param name="request">Restuarant business hour properties and data</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Restuarant object updated with the new id</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> AddBusinessHourAsync([FromRoute] string restuarantId, [FromBody] CreateBusinessHourRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Add restuarant business hour request received");
        RestuarantBusinessHourBO businessHour = await _orchestration.AddBusinessHourAsync(restuarantId, request.ToCreateBusinessHourRequestBO(), cancellationToken);

        return TypedResults.CreatedAtRoute(businessHour, RouteConstants.GetBusinessHour, new { restuarantId, businessHourId = businessHour.Id });
    }

    /// <summary>
    /// Add many new business hour entries to a restuarant
    /// </summary>
    /// <param name="restuarantId">Id of the Restuarant to add the business hour record to.</param>
    /// <param name="requests">Collection of restuarant business hour requests</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Collection of business hours added to the restuarant</returns>
    [HttpPost("bulk")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> AddManyBusinessHourAsync([FromRoute] string restuarantId, [FromBody] CreateBusinessHourRequest[] requests, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Add restuarant business hour request received");
        CreateBusinessHourRequestBO[] requestBOs = [.. requests.Select(_ => _.ToCreateBusinessHourRequestBO())];
        List<RestuarantBusinessHourBO> businessHours = await _orchestration.AddManyBusinessHoursAsync(restuarantId, requestBOs, cancellationToken);

        return TypedResults.CreatedAtRoute(businessHours, RouteConstants.ListBusinessHours, new { restuarantId });
    }

    /// <summary>
    /// Updates a business hour entry for a restuarant
    /// </summary>
    /// <param name="restuarantId">Id of the Restuarant to add the business hour record to.</param>
    /// <param name="businessHourId">Unique Identifier for a restuarant business hour</param>
    /// <param name="request">Restuarant business hour properties and data to update</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Results of the update transaction</returns>
    [HttpPatch("{businessHourId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> UpdateBusinessHourAsync([FromRoute] string restuarantId, [FromRoute] string businessHourId, [FromBody] UpdateBusinessHourRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Update restuarant business hour request received");
        TransactionResult results = await _orchestration.UpdateBusinessHourAsync(restuarantId, businessHourId, request.ToUpdateBusinessHourRequestBO(), cancellationToken);

        return TypedResults.Ok(results);
    }

    /// <summary>
    /// Removes a business hour entry from a restuarant
    /// </summary>
    /// <param name="restuarantId">Id of the Restuarant to add the business hour record to.</param>
    /// <param name="businessHourId">Unique Identifier for a restuarant business hour</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Results of the update transaction</returns>
    [HttpDelete("{businessHourId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IResult> RemoveBusinessHourAsync([FromRoute] string restuarantId, [FromRoute] string businessHourId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Remove restuarant business hour request received");
        TransactionResult results = await _orchestration.RemoveBusinessHourAsync(restuarantId, businessHourId, cancellationToken);

        return TypedResults.Ok(results);
    }
}
