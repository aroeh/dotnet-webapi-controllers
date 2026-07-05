using Demo.Restuarants.Infrastructure.MongoDb.Documents;
using Demo.Restuarants.Infrastructure.MongoDb.Extensions;
using Demo.Restuarants.Infrastructure.MongoDb.Interfaces;
using Demo.Restuarants.Shared.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Demo.Restuarants.Infrastructure.MongoDb.Repos;

public class RestuarantRepo
(
    ILogger<RestuarantRepo> log,
    IMongoDbRepo<RestuarantDocument> mongo
) : IRestuarantRepo
{
    private readonly ILogger<RestuarantRepo> _logger = log;
    private readonly IMongoDbRepo<RestuarantDocument> _mongo = mongo;

    /// <summary>
    /// Query restuarants
    /// </summary>
    /// <param name="queryParameters">Optional - Query parameters to filter restuarants</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Paginated collection of restuarant records matching <paramref name="queryParameters"/></returns>
    public async Task<PaginationResponse<RestuarantBO>> QueryRestuarantsAsync(FilterQueryParametersBO queryParameters, CancellationToken cancellationToken)
    {
        FilterDefinition<RestuarantDocument> filter = ConfigureFilter(queryParameters);

        _logger.LogInformation("Querying restuarants");
        var results = await _mongo.GetManyAsync(filter, queryParameters, cancellationToken);
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
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Restuarant if not <see langword="null"/></returns>
    public async Task<RestuarantBO?> GetRestuarantAsync(string id, CancellationToken cancellationToken)
    {
        FilterDefinition<RestuarantDocument> filter = Builders<RestuarantDocument>.Filter
            .Eq(d => d.Id, id);

        _logger.LogInformation("Finding restuarant by id");
        RestuarantDocument? restuarant = await _mongo.GetAsync(filter, cancellationToken);
        return restuarant?.ToRestuarant();
    }

    /// <summary>
    /// Creates a new Restuarant
    /// </summary>
    /// <param name="restuarant">Restuarant properties and data</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Restuarant object updated with the new id</returns>
    public async Task<RestuarantBO> CreateRestuarantAsync(RestuarantBO restuarant, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Adding new restuarant");
        RestuarantDocument document = restuarant.ToRestuarantDocument();
        await _mongo.CreateOneAsync(document, cancellationToken);

        return document.ToRestuarant();
    }

    /// <summary>
    /// Create many new Restuarants
    /// </summary>
    /// <param name="restuarants">Collection of new restuarants</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Results for the transaction</returns>
    public async Task<TransactionResult> CreateManyRestuarantsAsync(RestuarantBO[] restuarants, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Adding new restuarants");
        RestuarantDocument[] documents = [.. restuarants.Select(_ => _.ToRestuarantDocument())];
        return await _mongo.CreateManyAsync(documents, cancellationToken);
    }

    /// <summary>
    /// Update an existing restuarant
    /// </summary>
    /// <param name="id">Id of the restuarant</param>
    /// <param name="request">Restuarant properties to update</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Results for the transaction</returns>
    public async Task<TransactionResult> UpdateRestuarantAsync(string id, UpdateRestuarantRequestBO request, CancellationToken cancellationToken)
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

        return await _mongo.UpdateOneAsync(filter, updates, cancellationToken);
    }

    /// <summary>
    /// Update the location an existing restuarant
    /// </summary>
    /// <param name="id">Id of the restuarant</param>
    /// <param name="request">Restuarant location properties to update</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Results for the transaction</returns>
    public async Task<TransactionResult> UpdateRestuarantLocationAsync(string id, UpdateLocationRequestBO request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating restuarant location");

        FilterDefinition<RestuarantDocument> filter = Builders<RestuarantDocument>.Filter
            .Eq(d => d.Id, id);

        var update = Builders<RestuarantDocument>.Update;
        List<UpdateDefinition<RestuarantDocument>> updates = [];

        if (!string.IsNullOrWhiteSpace(request.Street))
        {
            updates.Add(update.Set(d => d.Address.Street, request.Street));
        }

        if (!string.IsNullOrWhiteSpace(request.City))
        {
            updates.Add(update.Set(d => d.Address.City, request.City));
        }

        if (!string.IsNullOrWhiteSpace(request.State))
        {
            updates.Add(update.Set(d => d.Address.State, request.State));
        }

        if (!string.IsNullOrWhiteSpace(request.Country))
        {
            updates.Add(update.Set(d => d.Address.Country, request.Country));
        }

        if (!string.IsNullOrWhiteSpace(request.ZipCode))
        {
            updates.Add(update.Set(d => d.Address.ZipCode, request.ZipCode));
        }

        return await _mongo.UpdateOneAsync(filter, updates, cancellationToken);
    }

    /// <summary>
    /// Removes a restuarant from the database
    /// </summary>
    /// <param name="id">Id of the restuarant</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Results for the transaction</returns>
    public async Task<TransactionResult> RemoveRestuarantAsync(string id, CancellationToken cancellationToken)
    {
        FilterDefinition<RestuarantDocument> filter = Builders<RestuarantDocument>.Filter
            .Eq(d => d.Id, id);

        return await _mongo.DeleteOneAsync(filter, cancellationToken);
    }

    /// <summary>
    /// List business hours for a restuarant
    /// </summary>
    /// <param name="restuarantId">Unique Identifier for a restuarant</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Collection of business hours for the restuarant matching <paramref name="restuarantId"/></returns>
    public async Task<List<RestuarantBusinessHourBO>> ListBusinessHoursAsync(string restuarantId, CancellationToken cancellationToken)
    {
        var restuarant = await GetRestuarantAsync(restuarantId, cancellationToken);

        return restuarant is null
            ? []
            : restuarant.BusinessHours;
    }

    /// <summary>
    /// Get business hour for a restuarant by id
    /// </summary>
    /// <param name="restuarantId">Unique Identifier for a restuarant</param>
    /// <param name="businessHourId">Unique Identifier for a restuarant business hour record</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Business hour for the restuarant matching <paramref name="restuarantId"/> and <paramref name="businessHourId"/> if not <see langword="null"/></returns>
    public async Task<RestuarantBusinessHourBO?> GetBusinessHourAsync(string restuarantId, string businessHourId, CancellationToken cancellationToken)
    {
        FilterDefinition<RestuarantDocument> filter = Builders<RestuarantDocument>.Filter
            .And(
                Builders<RestuarantDocument>.Filter.Eq(d => d.Id, restuarantId),
                Builders<RestuarantDocument>.Filter.ElemMatch(d => d.BusinessHours, hour => hour.Id == businessHourId)
            );

        _logger.LogInformation("Finding restuarant by id");
        var restuarant = await _mongo.GetAsync(filter, cancellationToken);

        if (restuarant is null || restuarant.BusinessHours.Count == 0)
        {
            return null;
        }

        return restuarant.ToRestuarant().BusinessHours.FirstOrDefault(_ => _.Id == businessHourId);
    }

    /// <summary>
    /// Add a new business hour entry to a restuarant
    /// </summary>
    /// <param name="restuarantId">Id of the Restuarant to add the business hour record to.</param>
    /// <param name="businessHour">Restuarant business hour properties and data</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Business hour object updated with the new id</returns>
    public async Task<RestuarantBusinessHourBO> AddBusinessHourAsync(string restuarantId, RestuarantBusinessHourBO businessHour, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Adding new restuarant business hour");

        BusinessHourDocument document = businessHour.ToBusinessHourDocument();

        FilterDefinition<RestuarantDocument> filter = Builders<RestuarantDocument>.Filter
            .Eq(d => d.Id, restuarantId);

        UpdateDefinition<RestuarantDocument> update = Builders<RestuarantDocument>.Update
            .AddToSet(d => d.BusinessHours, document);

        await _mongo.UpdateOneAsync(filter, update, cancellationToken);
        return document.ToRestuarantBusinessHourBO();
    }

    /// <summary>
    /// Add many new business hour entries to a restuarant
    /// </summary>
    /// <param name="restuarantId">Id of the Restuarant to add the business hour record to.</param>
    /// <param name="businessHours">Collection of restuarant business hour requests</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Collection of business hours added to the restuarant</returns>
    public async Task<List<RestuarantBusinessHourBO>> AddManyBusinessHoursAsync(string restuarantId, RestuarantBusinessHourBO[] businessHours, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Adding new restuarant business hour");

        BusinessHourDocument[] documents = [.. businessHours.Select(_ => _.ToBusinessHourDocument())];

        FilterDefinition<RestuarantDocument> filter = Builders<RestuarantDocument>.Filter
            .Eq(d => d.Id, restuarantId);

        UpdateDefinition<RestuarantDocument> update = Builders<RestuarantDocument>.Update
            .AddToSetEach(d => d.BusinessHours, documents);

        await _mongo.UpdateOneAsync(filter, update, cancellationToken);
        return [.. documents.Select(_ => _.ToRestuarantBusinessHourBO())];
    }

    /// <summary>
    /// Updates a business hour entry for a restuarant
    /// </summary>
    /// <param name="restuarantId">Id of the Restuarant to add the business hour record to.</param>
    /// <param name="businessHourId">Unique Identifier for a restuarant business hour</param>
    /// <param name="request">Restuarant business hour properties and data to update</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Results of the update transaction</returns>
    public async Task<TransactionResult> UpdateBusinessHourAsync(string restuarantId, string businessHourId, UpdateBusinessHourRequestBO request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("updating restuarant business hour document");

        FilterDefinition<RestuarantDocument> filter = Builders<RestuarantDocument>.Filter
            .And(
                Builders<RestuarantDocument>.Filter.Eq(d => d.Id, restuarantId),
                Builders<RestuarantDocument>.Filter.ElemMatch(d => d.BusinessHours, hour => hour.Id == businessHourId)
            );

        var update = Builders<RestuarantDocument>.Update;
        List<UpdateDefinition<RestuarantDocument>> updates = [];

        if (request.DayOfWeek is not null)
        {
            updates.Add(update.Set(doc => doc.BusinessHours.FirstMatchingElement().DayOfWeek, request.DayOfWeek.Value.ToString()));
        }

        if (request.OpenTime is not null)
        {
            updates.Add(update.Set(doc => doc.BusinessHours.FirstMatchingElement().OpenTime, request.OpenTime.Value));
        }

        if (request.CloseTime is not null)
        {
            updates.Add(update.Set(doc => doc.BusinessHours.FirstMatchingElement().CloseTime, request.CloseTime.Value));
        }

        return await _mongo.UpdateOneAsync(filter, updates, cancellationToken);
    }

    /// <summary>
    /// Removes a business hour entry from a restuarant
    /// </summary>
    /// <param name="restuarantId">Id of the Restuarant to add the business hour record to.</param>
    /// <param name="businessHourId">Unique Identifier for a restuarant business hour</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Results of the update transaction</returns>
    public async Task<TransactionResult> RemoveBusinessHourAsync(string restuarantId, string businessHourId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("removing restuarant business hour document");

        FilterDefinition<RestuarantDocument> filter = Builders<RestuarantDocument>.Filter
            .Eq(d => d.Id, restuarantId);

        UpdateDefinition<RestuarantDocument> update = Builders<RestuarantDocument>.Update
            .PullFilter(d => d.BusinessHours, hour => hour.Id == businessHourId);

        return await _mongo.UpdateOneAsync(filter, update, cancellationToken);
    }
}
