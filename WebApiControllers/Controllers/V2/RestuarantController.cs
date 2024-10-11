using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using System.Text.Encodings.Web;
using System.Text.Json;
using WebApiControllers.Middleware;
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
    public class RestuarantController : ControllerBase
    {
        private readonly ILogger<RestuarantController> logger;
        private readonly IRestuarantRepo restuarantRepo;

        public RestuarantController(ILoggerFactory logFactory, IRestuarantRepo repo)
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
            restuarantRepo = repo;
        }

        /// <summary>
        /// Get All Restuarants
        /// </summary>
        /// <returns></returns>
        [OutputCache]
        [HttpGet]
        public async Task<IResult> Get()
        {
            logger.GetAllRestuarants();
            List<Restuarant> restuarants = await restuarantRepo.GetAllRestuarants();

            if(restuarants == null || restuarants.Count == 0)
            {
                return TypedResults.NotFound();
            }

            logger.GetAllRestuarantsComplete();
            return TypedResults.Ok(restuarants);
        }

        /// <summary>
        /// Find Restuarants using matching criteria from query strings
        /// </summary>
        /// <returns></returns>
        [HttpPost("find")]
        public async Task<IResult> Find([FromBody] SearchCriteria search)
        {
            logger.FindRestuarants(JsonSerializer.Serialize(search));
            List<Restuarant> restuarants = await restuarantRepo.FindRestuarants(search.Name, search.Cuisine);

            if (restuarants == null || restuarants.Count == 0)
            {
                return TypedResults.NotFound();
            }

            logger.FindRestuarantsComplete();
            return TypedResults.Ok(restuarants);
        }

        /// <summary>
        /// Find Restuarants using matching criteria from query strings
        /// Demonstrates the same retrieval, but using HTTP GET and passing a complex object as the 
        /// input from the query
        /// 
        /// ex: GET {{controllerAddress}}/getfind?name=Test&cuisine=Test
        /// .Net will parse the query string parameters into the object properties
        /// </summary>
        /// <returns></returns>
        [HttpGet("getfind")]
        public async Task<IResult> GetFind([FromQuery] SearchCriteria search)
        {
            logger.FindRestuarants(JsonSerializer.Serialize(search));
            List<Restuarant> restuarants = await restuarantRepo.FindRestuarants(search.Name, search.Cuisine);

            if (restuarants == null || restuarants.Count == 0)
            {
                return TypedResults.NotFound();
            }

            logger.FindRestuarantsComplete();
            return TypedResults.Ok(restuarants);
        }

        /// <summary>
        /// Get a Restuarant using the provided id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IResult> Restuarant(string id)
        {
            logger.RestuarantById(id);
            Restuarant restuarant = await restuarantRepo.GetRestuarant(id);

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
            bool success = await restuarantRepo.InsertRestuarant(restuarant);

            logger.AddRestuarantComplete();
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
            logger.UpdateRestuarant(JsonSerializer.Serialize(restuarant));
            bool success = await restuarantRepo.UpdateRestuarant(restuarant);

            logger.UpdateRestuarantComplete();
            return TypedResults.Ok(success);
        }
    }
}
