using Asp.Versioning;
using Demo.Restuarants.Core.Interfaces;
using Demo.Restuarants.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Restuarants.API.Controllers.V1;

/// <summary>
/// This demonstrates an example of a version 1 controller
/// that will be alter and expanded in future versions.
/// Version 1 is listed as deprecated.
/// </summary>
/// <remarks>
/// This also demonstrates additional aspects of versioning, like mapping to models
/// to support breaking changes.
/// Once done with this version, it should be deleted and not impact the rest of the solution.
/// </remarks>
[ApiController]
[ApiVersion("1.0", Deprecated = true)]
[Produces("application/json")]
[Route("[controller]")]
public class RestuarantController(ILogger<RestuarantController> log, IRestuarantOrchestration orchestration) : ControllerBase
{
    private readonly ILogger<RestuarantController> logger = log;
    private readonly IRestuarantOrchestration restuarantOrchestration = orchestration;

    /// <summary>
    /// Get All Restuarants
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<RestuarantBO>> Get()
    {
        logger.LogInformation("Get all restuarants request received");
        FilterQueryParametersBO queryParameters = new(null, null);
        var results = await restuarantOrchestration.ListRestuarants(queryParameters);

        logger.LogInformation("Get all restuarants request complete...returning results");
        return [.. results.Data];
    }

    /// <summary>
    /// Find Restuarants using matching criteria from query strings
    /// </summary>
    /// <returns></returns>
    [HttpGet("[action]")]
    public async Task<List<RestuarantBO>> Find([FromQuery] string name, [FromQuery] string cuisine)
    {
        logger.LogInformation("Find restuarants request received");
        FilterQueryParametersBO queryParameters = new([name], cuisine);
        var results = await restuarantOrchestration.ListRestuarants(queryParameters);

        logger.LogInformation("Find restuarants request complete...returning results");
        return [.. results.Data];
    }

    /// <summary>
    /// Get a Restuarant using the provided id
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<RestuarantBO?> Restuarant(string id)
    {
        logger.LogInformation("Get restuarant request received");
        RestuarantBO? restuarant = await restuarantOrchestration.GetRestuarant(id);

        logger.LogInformation("Get restuarant request complete...returning results");
        return restuarant;
    }
}
