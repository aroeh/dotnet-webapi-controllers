using Asp.Versioning;
using Demo.Restuarants.API.Middleware;
using Demo.Restuarants.API.Models.V2;
using Demo.Restuarants.Core.Interfaces;
using Demo.Restuarants.Shared.Models;
using Demo.Restuarants.Shared.Models.V2;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Demo.Restuarants.API.Controllers.V2;

/// <summary>
/// This API has been expanded greatly over the V1 version.
/// This is just an example of how certain endpoints may be added or altered safely
/// </summary>
/// <remarks>
/// <para>
/// This version demonstrates some simple validation, using IResult and Typed Results.
/// Versioning assumes that Core and orchestrations will always operate on the latest logic.
/// Models are mapped into the newest and latest versions to be passed to the core layer.
/// </para>
/// <para>Even this controller is still not demonstrating some ideal practices and has been deprecated.</para>
/// <para>When this version is no longer needed, then delete it and the V2 models</para>
/// </remarks>
[ApiController]
[ApiVersion("2.0", Deprecated = true)]
[Produces("application/json")]
[Route("[controller]")]
public class RestuarantController : ControllerBase
{
    private readonly ILogger<RestuarantController> logger;
    private readonly IRestuarantOrchestration restuarantOrchestration;

    public RestuarantController(ILoggerFactory logFactory, IRestuarantOrchestration orchestration)
    {
        logFactory = LoggerFactory.Create(builder =>
        {
            builder.AddJsonConsole(options => 
                options.JsonWriterOptions = new JsonWriterOptions()
                {
                    Indented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });
        });
        logger = logFactory.CreateLogger<RestuarantController>();
        restuarantOrchestration = orchestration;
    }

    /// <summary>
    /// Get All Restuarants
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IResult> Get()
    {
        logger.GetAllRestuarants();
        FilterQueryParametersBO queryParameters = new(null, null);
        var results = await restuarantOrchestration.ListRestuarants(queryParameters);

        if (results == null || !results.Data.Any())
        {
            return TypedResults.NotFound();
        }

        logger.GetAllRestuarantsComplete();
        return TypedResults.Ok(results.Data);
    }

    /// <summary>
    /// Find Restuarants using matching criteria from query strings
    /// </summary>
    /// <returns></returns>
    [HttpPost("find")]
    public async Task<IResult> Find([FromBody] SearchCriteria search)
    {
        logger.FindRestuarants(JsonSerializer.Serialize(search));
        FilterQueryParametersBO queryParameters = new([search.Name], search.Cuisine);
        var results = await restuarantOrchestration.ListRestuarants(queryParameters);

        if (results == null || !results.Data.Any())
        {
            return TypedResults.NotFound();
        }

        logger.FindRestuarantsComplete();
        return TypedResults.Ok(results.Data);
    }

    /// <summary>
    /// Find Restuarants using matching criteria from query strings
    /// Demonstrates the same retrieval, but using HTTP GET and passing a complex object as the 
    /// input from the query
    /// 
    /// ex: GET {{controllerAddress}}/getfind?name=Test&cuisine=Test
    /// array values can also be passed by adding a parameter and values like types=type1&types=type2
    /// .Net will parse the query string parameters into the object properties
    /// </summary>
    /// <returns></returns>
    [HttpGet("getfind")]
    public async Task<IResult> GetFind([FromQuery] SearchCriteria search)
    {
        logger.FindRestuarants(JsonSerializer.Serialize(search));
        FilterQueryParametersBO queryParameters = new([search.Name], search.Cuisine);
        var results = await restuarantOrchestration.ListRestuarants(queryParameters);

        if (results == null || !results.Data.Any())
        {
            return TypedResults.NotFound();
        }

        logger.FindRestuarantsComplete();
        return TypedResults.Ok(results.Data);
    }

    /// <summary>
    /// Get a Restuarant using the provided id
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IResult> Restuarant(string id)
    {
        logger.RestuarantById(id);
        RestuarantBO? restuarant = await restuarantOrchestration.GetRestuarant(id);

        if(restuarant == null || string.IsNullOrWhiteSpace(restuarant.Id))
        {
            return TypedResults.NotFound();
        }

        logger.RestuarantByIdComplete();
        return TypedResults.Ok(restuarant);
    }

    /// <summary>
    /// Inserts a new restuarant
    /// </summary>
    /// <param name="restuarant"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IResult> Post([FromBody] Restuarant restuarant)
    {
        logger.AddRestuarant(JsonSerializer.Serialize(restuarant));
        CreateRestuarantRequestBO requestBO = new(
            restuarant.Name,
            restuarant.CuisineType,
            restuarant.Website,
            restuarant.Phone,
            new CreateLocationRequestBO(
                restuarant.Address.Street,
                restuarant.Address.City,
                restuarant.Address.State,
                restuarant.Address.Country,
                restuarant.Address.ZipCode
            )
        );
        await restuarantOrchestration.CreateRestuarant(requestBO);

        logger.AddRestuarantComplete();
        return TypedResults.Ok(true);
    }

    /// <summary>
    /// Inserts many new restuarants
    /// </summary>
    /// <param name="restuarants">Restuarant array with many items to insert</param>
    /// <returns>Task of Typed Results via IResult</returns>
    [HttpPost("bulk")]
    public async Task<IResult> PostMany([FromBody] Restuarant[] restuarants)
    {
        logger.LogInformation("Add restuarant request received");

        CreateRestuarantRequestBO[] requestBOs = [.. restuarants.Select(_ => 
        new CreateRestuarantRequestBO(
            _.Name,
            _.CuisineType,
            _.Website,
            _.Phone,
            new CreateLocationRequestBO(
                _.Address.Street,
                _.Address.City,
                _.Address.State,
                _.Address.Country,
                _.Address.ZipCode
            )
        ))];
        var results = await restuarantOrchestration.CreateManyRestuarants(requestBOs);

        logger.LogInformation("Add restuarant request complete...returning results");
        return TypedResults.Ok(results.Success);
    }

    /// <summary>
    /// Updates an existing restuarant
    /// </summary>
    /// <param name="restuarant"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<IResult> Put([FromBody] Restuarant restuarant)
    {
        logger.UpdateRestuarant(JsonSerializer.Serialize(restuarant));
        UpdateRestuarantRequestBO requestBO = new(
            restuarant.Name,
            restuarant.CuisineType,
            restuarant.Website,
            restuarant.Phone,
            new UpdateLocationRequestBO(
                restuarant.Address.Street,
                restuarant.Address.City,
                restuarant.Address.State,
                restuarant.Address.Country,
                restuarant.Address.ZipCode
            )
        );
        bool success = await restuarantOrchestration.UpdateRestuarant(restuarant.Id, requestBO);

        logger.UpdateRestuarantComplete();
        return TypedResults.Ok(success);
    }
}
