using Demo.Restuarants.API.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using WebApiHttpClient.HttpClientHelpers;

namespace WebApiHttpClient.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TestController(ILogger<TestController> log, HttpFactoryHelper factory, HttpClientHelper client) : ControllerBase
{
    private readonly ILogger<TestController> logger = log;
    private readonly HttpFactoryHelper httpFactory = factory;
    private readonly HttpClientHelper clientHelper = client;

    /// <summary>
    /// Get All Restuarants
    /// </summary>
    /// <returns></returns>
    [HttpGet("factory")]
    public async Task<IResult> GetUsingFactory()
    {
        var restuarants = await httpFactory.GetAsync<PaginationResponse<RestuarantBO>>("api/restuarant/v3");

        return TypedResults.Ok(restuarants);
    }

    /// <summary>
    /// Get All Restuarants
    /// </summary>
    /// <returns></returns>
    [HttpGet("client")]
    public async Task<IResult> GetUsingClient()
    {
        clientHelper.SetHeader();
        var restuarants = await clientHelper.GetAsync<PaginationResponse<RestuarantBO>>("api/restuarant/v3");

        return TypedResults.Ok(restuarants);
    }
}
