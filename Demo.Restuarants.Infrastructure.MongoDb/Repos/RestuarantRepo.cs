using Demo.Restuarants.Infrastructure.MongoDb.Documents;
using Demo.Restuarants.Infrastructure.MongoDb.Extensions;
using Demo.Restuarants.Infrastructure.MongoDb.Interfaces;
using Demo.Restuarants.Shared.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Demo.Restuarants.Infrastructure.MongoDb.Repos;

public class RestuarantRepo
(
    ILogger<RestuarantRepo> log,
    IMongoDbRepo<RestuarantDocument> mongo
) : IRestuarantRepo
{
    private readonly ILogger<RestuarantRepo> _logger = log;
    private readonly IMongoDbRepo<RestuarantDocument> _mongo = mongo;

    private const string _collection = "restuarants";

    /// <summary>
    /// Query restuarants
    /// </summary>
    /// <param name="queryParameters">Optional - Query parameters to filter restuarants</param>
    /// <returns>Paginated collection of restuarant records matching <paramref name="queryParameters"/></returns>
    public async Task<PaginationResponse<RestuarantBO>> QueryRestuarants(FilterQueryParametersBO queryParameters)
    {
        FilterDefinition<RestuarantDocument> filter = ConfigureFilter(queryParameters);

        _logger.LogInformation("Querying restuarants");
        var results = await _mongo.GetManyAsync(_collection, filter, queryParameters);
        return new PaginationResponse<RestuarantBO>([.. results.Data.Select(_ => _.ToRestuarant())], results.MetaData);
    }

    private static FilterDefinition<RestuarantDocument> ConfigureFilter(FilterQueryParametersBO queryParameters)
    {
        FilterDefinitionBuilder<RestuarantDocument> builder = Builders<RestuarantDocument>.Filter;
        FilterDefinition<RestuarantDocument> filter = builder.Empty;

        if (queryParameters.Names is not null)
        {
            List<FilterDefinition<RestuarantDocument>> nameFilters = [];
            foreach (var name in queryParameters.Names)
            {
                var namePattern = $"{name}";
                var nameRegEx = new BsonRegularExpression(namePattern, "i");
                nameFilters.Add(builder.Regex(d => d.Name, nameRegEx));
            }
            filter &= builder.Or([.. nameFilters]);
        }

        if (queryParameters.CuisineType is not null)
        {
            List<FilterDefinition<RestuarantDocument>> cuisineFilters = [];
            var cuisinePattern = $"{queryParameters.CuisineType}";
            var cuisineRegEx = new BsonRegularExpression(cuisinePattern, "i");
            cuisineFilters.Add(builder.Regex(d => d.CuisineType, cuisineRegEx));
            filter &= builder.Or([.. cuisineFilters]);
        }

        return filter;
    }

    /// <summary>
    /// Get restuarant by id
    /// </summary>
    /// <param name="id">Id of the restuarant</param>
    /// <returns>Restuarant if not <see langword="null"/></returns>
    public async Task<RestuarantBO?> GetRestuarant(string id)
    {
        FilterDefinition<RestuarantDocument> filter = Builders<RestuarantDocument>.Filter
            .Eq(d => d.Id, id);

        _logger.LogInformation("Finding restuarant by id");
        RestuarantDocument? restuarant = await _mongo.GetAsync(_collection, filter);
        return restuarant?.ToRestuarant();
    }

    /// <summary>
    /// Creates a new Restuarant
    /// </summary>
    /// <param name="restuarant">Restuarant properties and data</param>
    /// <returns>Restuarant object updated with the new id</returns>
    public async Task<RestuarantBO> CreateRestuarant(RestuarantBO restuarant)
    {
        _logger.LogInformation("Adding new restuarant");
        RestuarantDocument document = restuarant.ToRestuarantDocument();
        await _mongo.CreateOneAsync(_collection, document);

        return document.ToRestuarant();
    }

    /// <summary>
    /// Create many new Restuarants
    /// </summary>
    /// <param name="restuarants">Collection of new restuarants</param>
    /// <returns>MongoDb results for the transaction</returns>
    public async Task<TransactionResult> CreateManyRestuarants(RestuarantBO[] restuarants)
    {
        _logger.LogInformation("Adding new restuarants");
        RestuarantDocument[] documents = [.. restuarants.Select(_ => _.ToRestuarantDocument())];
        return await _mongo.CreateManyAsync(_collection, documents);
    }

    /// <summary>
    /// Update an existing restuarant
    /// </summary>
    /// <param name="id">Id of the restuarant</param>
    /// <param name="request">Restuarant properties to update</param>
    /// <returns>MongoDb results for the transaction</returns>
    public async Task<TransactionResult> UpdateRestuarant(string id, UpdateRestuarantRequestBO request)
    {
        _logger.LogInformation("Updating restuarant");

        FilterDefinition<RestuarantDocument> filter = Builders<RestuarantDocument>.Filter
            .Eq(d => d.Id, id);

        var update = Builders<RestuarantDocument>.Update;
        List<UpdateDefinition<RestuarantDocument>> updates = [];

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            updates.Add(update.Set(d => d.Name, request.Name));
        }

        if (!string.IsNullOrWhiteSpace(request.CuisineType))
        {
            updates.Add(update.Set(d => d.CuisineType, request.CuisineType));
        }

        if (request.Website is not null)
        {
            updates.Add(update.Set(d => d.Website, request.Website.ToString()));
        }

        if (!string.IsNullOrWhiteSpace(request.Phone))
        {
            updates.Add(update.Set(d => d.Phone, request.Phone));
        }

        if (request.Address is not null)
        {
            if (!string.IsNullOrWhiteSpace(request.Address.Street))
            {
                updates.Add(update.Set(d => d.Address.Street, request.Address.Street));
            }

            if (!string.IsNullOrWhiteSpace(request.Address.City))
            {
                updates.Add(update.Set(d => d.Address.City, request.Address.City));
            }

            if (!string.IsNullOrWhiteSpace(request.Address.State))
            {
                updates.Add(update.Set(d => d.Address.State, request.Address.State));
            }

            if (!string.IsNullOrWhiteSpace(request.Address.Country))
            {
                updates.Add(update.Set(d => d.Address.Country, request.Address.Country));
            }

            if (!string.IsNullOrWhiteSpace(request.Address.ZipCode))
            {
                updates.Add(update.Set(d => d.Address.ZipCode, request.Address.ZipCode));
            }
        }

        return await _mongo.UpdateOneAsync(_collection, filter, updates);
    }

    /// <summary>
    /// Removes a restuarant from the database
    /// </summary>
    /// <param name="id">Id of the restuarant</param>
    /// <returns>MongoDb results for the transaction</returns>
    public async Task<TransactionResult> RemoveRestuarant(string id)
    {
        FilterDefinition<RestuarantDocument> filter = Builders<RestuarantDocument>.Filter
            .Eq(d => d.Id, id);

        return await _mongo.DeleteOneAsync(_collection, filter);
    }
}
