using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using WebApiControllers.Shared.Models;
using WebApiControllers.DomainLogic.Orchestrations;

namespace WebApiControllers.Controllers.V1;

/// <summary>
/// This demonstrates an example of a version 1 controller
/// that will be alter and expanded in future versions.
/// Version 1 is listed as deprecated.
/// </summary>
/// <param name="log"></param>
/// <param name="repo"></param>
[ApiController]
[ApiVersion("1.0", Deprecated = true)]
[Produces("application/json")]
[Route("[controller]/v{version:apiVersion}")]
public class RestuarantController(ILogger<RestuarantController> log, IRestuarantOrchestration orchestration) : ControllerBase
{
    private readonly ILogger<RestuarantController> logger = log;
    private readonly IRestuarantOrchestration restuarantOrchestration = orchestration;

    /// <summary>
    /// Get All Restuarants
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<Restuarant>> Get()
    {
        logger.LogInformation("Get all restuarants request received");
        List<Restuarant> restuarants = await restuarantOrchestration.GetAllRestuarants();

        logger.LogInformation("Get all restuarants request complete...returning results");
        return restuarants;
    }

    /// <summary>
    /// Find Restuarants using matching criteria from query strings
    /// </summary>
    /// <returns></returns>
    [HttpGet("[action]")]
    public async Task<List<Restuarant>> Find([FromQuery] string name, [FromQuery] string cuisine)
    {
        logger.LogInformation("Find restuarants request received");
        List<Restuarant> restuarants = await restuarantOrchestration.FindRestuarants(name, cuisine);

        logger.LogInformation("Find restuarants request complete...returning results");
        return restuarants;
    }

    /// <summary>
    /// Get a Restuarant using the provided id
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<Restuarant> Restuarant(string id)
    {
        logger.LogInformation("Get restuarant request received");
        Restuarant restuarant = await restuarantOrchestration.GetRestuarant(id);

        logger.LogInformation("Get restuarant request complete...returning results");
        return restuarant;
    }
}
