using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using WebApiControllers.HttpClientHelpers;
using WebApiControllers.Models;

namespace WebApiControllers.Controllers.V2;

/// <summary>
/// This is a controller that demonstrates how the http clients can be injected into a class for use
/// The test.http file can be used to trigger tests to this controller.
/// 
/// Currently this is setup to demonstrate the usage of models in the solution.  
/// 
/// While is is a bit clunky this can be tested against the Restuarant controller endpoints in a container.
/// 1. Docker Compose up the entire solution
/// 2. Then adjust launch settings and change one of the ports in the http or https profile to one that is different from the port specified in the docker compose.
/// 3. Then debug/run the solution and test via the test.http file
/// </summary>
[ApiController]
[ApiVersion("2.0")]
[Produces("application/json")]
[Route("[controller]/v{version:apiVersion}")]
public class TestController(ILogger<TestController> log, HttpFactoryHelper http) : ControllerBase
{
    private readonly ILogger<TestController> logger = log;
    private readonly HttpFactoryHelper httpHelper = http;

    /// <summary>
    /// Get All Restuarants
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IResult> Get()
    {
        Restuarant addRestuarant = new()
        {
            Name = "Test",
            CuisineType = "Test",
            Website = new Uri("https://www.google.com/"),
            Phone = "1112223333",
            Address = new()
            {
                Street = "123 Test Street",
                City = "Somewhere",
                State = "KY",
                ZipCode = "12345",
                Country = "United States"
            }
        };

        bool addResult = await httpHelper.PostAsync<Restuarant, bool>("/restuarant/v2", addRestuarant);

        List<Restuarant> restuarants = await httpHelper.GetAsync<List<Restuarant>>("/restuarant/v2");

        
        //logger.LogInformation("Get all foods request received");
        //List<string> foods =
        //[
        //    "cookies",
        //    "cake",
        //    "bananas"
        //];

        //logger.LogInformation("Get all foods request complete...returning results");
        return TypedResults.Ok(restuarants);
    }
}
