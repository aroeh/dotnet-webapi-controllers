using Demo.Restuarants.API.Models;
using Demo.Restuarants.API.SDK;
using Microsoft.AspNetCore.Mvc;
using WebApiHttpClient.HttpClientHelpers;

namespace WebApiHttpClient.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TestController(ILogger<TestController> log, IRestuarantsApi restuarantApi) : ControllerBase
{
    private readonly ILogger<TestController> logger = log;
    //private readonly HttpFactoryHelper httpFactory = factory;
    //private readonly HttpClientHelper clientHelper = client;
    private readonly IRestuarantsApi _restuarantApi = restuarantApi;

    /// <summary>
    /// Get All Restuarants
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IResult> GetRefitApi()
    {
        FilterQueryParameters queryParameters = new();
        var restuarants = await _restuarantApi.QueryRestuarantsAsync(queryParameters);

        return TypedResults.Ok(restuarants);
    }

    /// <summary>
    /// Get All Restuarants
    /// </summary>
    /// <returns></returns>
    [HttpGet("factory")]
    public async Task<IResult> GetUsingFactory()
    {
        //var restuarants = await httpFactory.GetAsync<PaginationResponse<RestuarantBO>>("api/restuarant/v3");

        return TypedResults.Ok();
    }

    /// <summary>
    /// Get All Restuarants
    /// </summary>
    /// <returns></returns>
    [HttpGet("client")]
    public async Task<IResult> GetUsingClient()
    {
        //clientHelper.SetHeader();
        //var restuarants = await clientHelper.GetAsync<PaginationResponse<RestuarantBO>>("api/restuarant/v3");

        return TypedResults.Ok();
    }
}
