using Asp.Versioning;
using Demo.Restuarants.API.Extensions;
using Demo.Restuarants.API.Models;
using Demo.Restuarants.Core.Interfaces;
using Demo.Restuarants.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Restuarants.API.Controllers;

[ApiVersion(ApiVersions.Latest)]
[Route("api/[controller]")]
public class RestuarantController
(
    ILogger<RestuarantController> log,
    IRestuarantOrchestration orchestration
) : ApiControllerBase<RestuarantController>(log)
{
    private readonly ILogger<RestuarantController> _logger = log;
    private readonly IRestuarantOrchestration _orchestration = orchestration;

    /// <summary>
    /// List restuarants
    /// </summary>
    /// <param name="queryParameters">Optional - Query parameters to filter restuarants</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Paginated list of restuarants matching <paramref name="queryParameters"/></returns>
    [HttpGet]
    public async Task<IResult> ListRestuarants([FromQuery] FilterQueryParameters queryParameters, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Find restuarants request received");
        PaginationResponse<RestuarantBO> restuarants = await _orchestration.ListRestuarants(queryParameters.ToFilterQueryParametersBO(), cancellationToken);

        return TypedResults.Ok(restuarants);
    }

    /// <summary>
    /// Get a Restuarant using the provided id
    /// </summary>
    /// <param name="id">Unique Identifier for a restuarant</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Restuarant if not <see langword="null"/></returns>
    [HttpGet("{id}")]
    public async Task<IResult> GetRestuarant([FromRoute] string id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Get restuarant request received");
        RestuarantBO? restuarant = await _orchestration.GetRestuarant(id, cancellationToken);

        if (restuarant is null || string.IsNullOrWhiteSpace(restuarant.Id))
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(restuarant);
    }

    /// <summary>
    /// Creates a new restuarant
    /// </summary>
    /// <param name="request">Restuarant object to insert</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Restuarant object updated with the new id</returns>
    [HttpPost]
    public async Task<IResult> CreateRestuarant([FromBody] CreateRestuarantRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Add restuarant request received");
        RestuarantBO restuarant = await _orchestration.CreateRestuarant(request.ToCreateRestuarantRequestBO(),cancellationToken);

        return TypedResults.Created(HttpContext.Request.Path.Value, restuarant);
    }

    /// <summary>
    /// Creates many new restuarants
    /// </summary>
    /// <param name="requests">Collection of create restuarant requests</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Create results for the transaction</returns>
    [HttpPost("bulk")]
    public async Task<IResult> CreateManyRestuarants([FromBody] CreateRestuarantRequest[] requests, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Add restuarant request received");
        CreateRestuarantRequestBO[] requestBOs = [.. requests.Select(_ => _.ToCreateRestuarantRequestBO())];
        var results = await _orchestration.CreateManyRestuarants(requestBOs, cancellationToken);

        return TypedResults.Ok(results);
    }

    /// <summary>
    /// Update an existing restuarant
    /// </summary>
    /// <param name="id">Id of the Restuarant to update</param>
    /// <param name="request">Restuarant object to update</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Success result</returns>
    [HttpPatch("{id}")]
    public async Task<IResult> UpdateRestuarant([FromRoute] string id, [FromBody] UpdateRestuarantRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Update restuarant request received");
        bool success = await _orchestration.UpdateRestuarant(id, request.ToUpdateRestuarantRequestBO(), cancellationToken);

        return TypedResults.Ok(success);
    }

    /// <summary>
    /// Remove a restuarant
    /// </summary>
    /// <param name="id">Id of the Restuarant to update</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Success result</returns>
    [HttpDelete("{id}")]
    public async Task<IResult> RemoveRestuarant([FromRoute] string id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Remove restuarant request received");
        bool success = await _orchestration.RemoveRestuarant(id, cancellationToken);

        return TypedResults.Ok(success);
    }
}
