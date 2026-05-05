using Asp.Versioning;
using Demo.Restuarants.API.Constants;
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IResult> ListRestuarantsAsync([FromQuery] FilterQueryParameters queryParameters, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Find restuarants request received");
        PaginationResponse<RestuarantBO> restuarants = await _orchestration.ListRestuarantsAsync(queryParameters.ToFilterQueryParametersBO(), cancellationToken);

        return TypedResults.Ok(restuarants);
    }

    /// <summary>
    /// Get a Restuarant using the provided id
    /// </summary>
    /// <param name="id">Unique Identifier for a restuarant</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Restuarant if not <see langword="null"/></returns>
    [HttpGet("{id}", Name = RouteConstants.GetRestuarant)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //[ActionName(nameof(GetRestuarantAsync))] // If using IActionResult, uncomment this to set the action name for the Create method
    public async Task<IResult> GetRestuarantAsync([FromRoute] string id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Get restuarant request received");
        RestuarantBO? restuarant = await _orchestration.GetRestuarantAsync(id, cancellationToken);

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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> CreateRestuarantAsync([FromBody] CreateRestuarantRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Add restuarant request received");
        RestuarantBO restuarant = await _orchestration.CreateRestuarantAsync(request.ToCreateRestuarantRequestBO(), cancellationToken);

        // Best practice to use the path and the id to return in the header and point to the resource
        return TypedResults.CreatedAtRoute(restuarant, RouteConstants.GetRestuarant, new { id = restuarant.Id });

        /*
         * TypedResults offers 2 different methods to use for returning created data:  Created and CreatedAtRoute
         * CreatedAtRoute can return the host name along with the route and parameters.  It uses a route name to generate the URI to return in the header.
         * It does require the Get method to have the Name attribute.  See GetRestuarantAsync method for the exact implementation.  The route name needs to be set [HttpGet("{id}", Name = "GetRestuarant")]
         * 
         * Created expects a URI for the route to point back to the created resource.  This can include the full host name or just the relative location.
         * ex: api/restuarant/<id> vs https://localhost/api/restuarant/<id>
         * 
         * implementation ex:
         * return TypedResults.Created($"{HttpContext.Request.Path.Value}/{restuarant.Id}", restuarant);
         */
    }

    /*
     * This method could be written using IActionResult and related method, in fact all methods in this controller could be written to use IAction Result.
     * The IResult matches the same return type as minimal apis and that's the only reason it was used here.  Both work great.
     * 
     * Here's what the create method would look like using IActionResult
     * Notice the CreatedAtAction - with methods using an Async Suffix you will need to set the ActionName attribute on the name of the method being referenced
     * see the GetRestuarantAsync for the implementation
        public async Task<IActionResult> CreateRestuarantAsync([FromBody] CreateRestuarantRequest request, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Add restuarant request received");
            RestuarantBO restuarant = await _orchestration.CreateRestuarantAsync(request.ToCreateRestuarantRequestBO(), cancellationToken);

            return CreatedAtAction(nameof(GetRestuarantAsync), new { id = restuarant.Id }, restuarant);
        }
     */

    /// <summary>
    /// Creates many new restuarants
    /// </summary>
    /// <param name="requests">Collection of create restuarant requests</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Create results for the transaction</returns>
    [HttpPost("bulk")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> CreateManyRestuarantsAsync([FromBody] CreateRestuarantRequest[] requests, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Add restuarant request received");
        CreateRestuarantRequestBO[] requestBOs = [.. requests.Select(_ => _.ToCreateRestuarantRequestBO())];
        var results = await _orchestration.CreateManyRestuarantsAsync(requestBOs, cancellationToken);

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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> UpdateRestuarantAsync([FromRoute] string id, [FromBody] UpdateRestuarantRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Update restuarant request received");
        TransactionResult results = await _orchestration.UpdateRestuarantAsync(id, request.ToUpdateRestuarantRequestBO(), cancellationToken);

        return TypedResults.Ok(results);
    }

    /// <summary>
    /// Remove a restuarant
    /// </summary>
    /// <param name="id">Id of the Restuarant to update</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Success result</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IResult> RemoveRestuarantAsync([FromRoute] string id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Remove restuarant request received");
        bool success = await _orchestration.RemoveRestuarantAsync(id, cancellationToken);

        return TypedResults.Ok(success);
    }
}
