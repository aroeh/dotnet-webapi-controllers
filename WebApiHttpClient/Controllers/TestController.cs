using Microsoft.AspNetCore.Mvc;
using WebApiHttpClient.HttpClientHelpers;
using WebApiHttpClient.Models;

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
        List<Restuarant> restuarants = await httpFactory.GetAsync<List<Restuarant>>("/restuarant/v2");

        return TypedResults.Ok(restuarants);
    }

    /// <summary>
    /// Get All Restuarants
    /// </summary>
    /// <returns></returns>
    [HttpGet("client")]
    public async Task<IResult> GetUsingClient()
    {
        List<Restuarant> restuarants = await clientHelper.GetAsync<List<Restuarant>>("/restuarant/v2");

        return TypedResults.Ok(restuarants);
    }
}
