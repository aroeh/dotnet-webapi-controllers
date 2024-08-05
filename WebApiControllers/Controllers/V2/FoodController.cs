using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace WebApiControllers.Controllers.V2
{
    /// <summary>
    /// This API has been expanded greatly over the V1 version.
    /// This is just an example of how certain endpoints may be added or altered safely
    /// 
    /// This version demonstrats some simple validation, using IResult and Typed Results,
    /// and OutputCache
    /// </summary>
    [ApiController]
    [ApiVersion("2.0")]
    [Produces("application/json")]
    [Route("[controller]/v{version:apiVersion}")]
    public class FoodController(ILogger<FoodController> log) : ControllerBase
    {
        private readonly ILogger<FoodController> logger = log;

        /// <summary>
        /// Get All Restuarants
        /// </summary>
        /// <returns></returns>
        [OutputCache]
        [HttpGet]
        public IResult Get()
        {
            logger.LogInformation("Get all foods request received");
            List<string> foods =
            [
                "cookies",
                "cake",
                "bananas"
            ];

            logger.LogInformation("Get all foods request complete...returning results");
            return TypedResults.Ok(foods);
        }
    }
}
