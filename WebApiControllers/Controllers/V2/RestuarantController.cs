using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using WebApiControllers.Models;
using WebApiControllers.Repos;

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
    public class RestuarantController(ILogger<RestuarantController> log, IRestuarantRepo repo) : ControllerBase
    {
        private readonly ILogger<RestuarantController> logger = log;
        private readonly IRestuarantRepo restuarantRepo = repo;

        /// <summary>
        /// Get All Restuarants
        /// </summary>
        /// <returns></returns>
        [OutputCache]
        [HttpGet]
        public async Task<IResult> Get()
        {
            logger.LogInformation("Get all restuarants request received");
            List<Restuarant> restuarants = await restuarantRepo.GetAllRestuarants();

            if(restuarants == null || restuarants.Count == 0)
            {
                return TypedResults.NotFound();
            }

            logger.LogInformation("Get all restuarants request complete...returning results");
            return TypedResults.Ok(restuarants);
        }

        /// <summary>
        /// Find Restuarants using matching criteria from query strings
        /// </summary>
        /// <returns></returns>
        [HttpPost("find")]
        public async Task<IResult> Find([FromBody] SearchCriteria search)
        {
            logger.LogInformation("Find restuarants request received");
            List<Restuarant> restuarants = await restuarantRepo.FindRestuarants(search.Name, search.Cuisine);

            if (restuarants == null || restuarants.Count == 0)
            {
                return TypedResults.NotFound();
            }

            logger.LogInformation("Find restuarants request complete...returning results");
            return TypedResults.Ok(restuarants);
        }

        /// <summary>
        /// Get a Restuarant using the provided id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IResult> Restuarant(string id)
        {
            logger.LogInformation("Get restuarant request received");
            Restuarant restuarant = await restuarantRepo.GetRestuarant(id);

            if(restuarant == null || string.IsNullOrWhiteSpace(restuarant.Id))
            {
                return TypedResults.NotFound();
            }

            logger.LogInformation("Get restuarant request complete...returning results");
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
            logger.LogInformation("Add restuarant request received");
            bool success = await restuarantRepo.InsertRestuarant(restuarant);

            logger.LogInformation("Add restuarant request complete...returning results");
            return TypedResults.Ok(success);
        }

        /// <summary>
        /// Updates an existing restuarant
        /// </summary>
        /// <param name="restuarant"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResult> Put([FromBody] Restuarant restuarant)
        {
            logger.LogInformation("Update restuarant request received");
            bool success = await restuarantRepo.UpdateRestuarant(restuarant);

            logger.LogInformation("Update restuarant request complete...returning results");
            return TypedResults.Ok(success);
        }
    }
}
