using Demo.Restuarants.Infrastructure.MongoDb.Extensions;
using Demo.Restuarants.Infrastructure.MongoDb.Interfaces;
using Demo.Restuarants.Infrastructure.MongoDb.Options;
using Demo.Restuarants.Shared.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Demo.Restuarants.Infrastructure.MongoDb.Repos;

/// <summary>
/// Mongo DB Repository
/// </summary>
/// <remarks>
/// <para>
/// Methods for executing CRUD operations on a Mongo DB cluster
/// </para>
/// </remarks>
/// <typeparam name="TEntity">Entity matching collection model</typeparam>
public class MongoDbRepo<TEntity> : IMongoDbRepo<TEntity> where TEntity : class
{
    private readonly MongoClient _client;
    private readonly IMongoDatabase _database;
    private readonly ILogger<MongoDbRepo<TEntity>> _logger;

    /// <summary>
    /// For this project depending on your resources available - you will need to setup the connection string
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="config"></param>
    public MongoDbRepo(ILogger<MongoDbRepo<TEntity>> logger, IOptions<MongoDbOptions> options)
    {
        _logger = logger;

        _logger.LogInformation("Configuring MongoDB Client");
        _client = new(options.Value.ConnectionString);

        _logger.LogInformation("Configuring MongoDB Database");
        _database = _client.GetDatabase(options.Value.DatabaseName);

        _logger.LogInformation("MongoDB Connection established and service ready");
    }

    /// <summary>
    /// Gets a read only collection of entities
    /// </summary>
    /// <param name="collectionName">Name of the Collection</param>
    /// <param name="filter">Filter Definition query</param>
    /// <returns>Read only collection of entities matching <paramref name="filter"/></returns>
    public async Task<IEnumerable<TEntity>> GetManyAsync(string collectionName, FilterDefinition<TEntity> filter)
    {
        var collection = _database.GetCollection<TEntity>(collectionName);

        _logger.LogInformation("Finding items by Filter");
        return await collection.Find(filter).ToListAsync();
    }

    /// <summary>
    /// Gets a paginated collection of entities
    /// </summary>
    /// <param name="collectionName">Name of the Collection</param>
    /// <param name="filter">Filter Definition query</param>
    /// <param name="pagination">Pagination parameters</param>
    /// <remarks>
    /// Uses Offset pagination implementation
    /// </remarks>
    /// <returns>Paginated collection of entities matching <paramref name="filter"/></returns>
    public async Task<PaginationResponse<TEntity>> GetManyAsync(string collectionName, FilterDefinition<TEntity> filter, PaginationQueryParametersBO pagination)
    {
        var collection = _database.GetCollection<TEntity>(collectionName);

        var totalCount = await collection.EstimatedDocumentCountAsync();
        var skipToPosition = pagination.Page == 1 ? 0 : (pagination.Page - 1) * pagination.PageSize;

        _logger.LogInformation("Finding items by Filter");
        var results = await collection
            .Find(filter)
            .Skip(skipToPosition)
            .Limit(pagination.PageSize)
            .ToListAsync();

        PaginationMetaData metaData = new(pagination.Page, results.Count, pagination.PageSize, totalCount);
        return new PaginationResponse<TEntity>(results, metaData);
    }

    /// <summary>
    /// Get a single instance of an entity
    /// </summary>
    /// <param name="collectionName">Name of the Collection</param>
    /// <param name="filter">Filter Definition query</param>
    /// <returns>Instance of a entity matching <paramref name="filter"/></returns>
    public async Task<TEntity?> GetAsync(string collectionName, FilterDefinition<TEntity> filter)
    {
        var collection = _database.GetCollection<TEntity>(collectionName);

        _logger.LogInformation("Finding items by Filter");
        return await collection.Find(filter).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Insert a new document into the specified collection
    /// </summary>
    /// <param name="collectionName">Name of the Collection</param>
    /// <param name="document">Document to be created in the <paramref name="collectionName"/> collection</param>
    /// <returns>Newly created document</returns>
    public async Task<TEntity> CreateOneAsync(string collectionName, TEntity document)
    {
        var collection = _database.GetCollection<TEntity>(collectionName);

        _logger.LogInformation("inserting new document");
        await collection.InsertOneAsync(document);
        return document;
    }

    /// <summary>
    /// Insert multiple new documents into the specified collection
    /// </summary>
    /// <param name="collectionName">Name of the Collection</param>
    /// <param name="documents">Documents to be created in the <paramref name="collectionName"/> collection</param>
    /// <returns>Operation success result of true or false</returns>
    public async Task<TransactionResult> CreateManyAsync(string collectionName, IEnumerable<TEntity> documents)
    {
        var collection = _database.GetCollection<TEntity>(collectionName);

        _logger.LogInformation("inserting new documents");
        await collection.InsertManyAsync(documents);
        return new TransactionResult
        {
            TransactionRun = true,
            IsAcknowledged = true,
            ExpectedRecordCount = documents.Count(),
            ActualRecordCount = documents.Count()
        };
    }

    /// <summary>
    /// Replaces a document with a later version using a filter to match the record
    /// </summary>
    /// <param name="collectionName">Name of the Collection</param>
    /// <param name="filter">Filter Definition query</param>
    /// <param name="document">Document to be replaced in the <paramref name="collectionName"/> collection</param>
    /// <returns><see cref="TransactionResult"/> results of the Replace operation</returns>
    public async Task<TransactionResult> ReplaceOneAsync(string collectionName, FilterDefinition<TEntity> filter, TEntity document)
    {
        var collection = _database.GetCollection<TEntity>(collectionName);

        _logger.LogInformation("starting replace operation");
        ReplaceOneResult result = await collection.ReplaceOneAsync(filter, document);

        _logger.LogInformation("operation completed...returning result");
        return result.ToMongoTransactionResult();
    }

    /// <summary>
    /// Updates a document
    /// </summary>
    /// <remarks>
    /// Requires there be an update operation
    /// </remarks>
    /// <param name="collectionName">Name of the Collection</param>
    /// <param name="filter">Filter Definition query</param>
    /// <param name="update">Update Definition</param>
    /// <returns><see cref="TransactionResult"/> results of the Update operation</returns>
    public async Task<TransactionResult> UpdateOneAsync(string collectionName, FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update)
    {
        var collection = _database.GetCollection<TEntity>(collectionName);

        UpdateResult result = await collection.UpdateOneAsync(filter, update);
        return result.ToMongoTransactionResult();
    }

    /// <summary>
    /// Updates a document
    /// </summary>
    /// <remarks>
    /// Use for a multiple updates on a document
    /// </remarks>
    /// <param name="collectionName">Name of the Collection</param>
    /// <param name="filter">Filter Definition query</param>
    /// <param name="updates">Collection of Update Definitions for a model</param>
    /// <returns><see cref="TransactionResult"/> results of the Update operation</returns>
    public async Task<TransactionResult> UpdateOneAsync(string collectionName, FilterDefinition<TEntity> filter, List<UpdateDefinition<TEntity>> updates)
    {
        if (updates.Count == 0)
        {
            _logger.LogInformation("There are no updates to perform...returning result");
            return new TransactionResult
            {
                TransactionRun = false
            };
        }

        var update = Builders<TEntity>.Update;
        return await UpdateOneAsync(collectionName, filter, update.Combine(updates));
    }

    /// <summary>
    /// Removes a document
    /// </summary>
    /// <param name="collectionName">Name of the Collection</param>
    /// <param name="filter">Filter Definition query</param>
    /// <returns><see cref="TransactionResult"/> results of the Delete operation</returns>
    public async Task<TransactionResult> DeleteOneAsync(string collectionName, FilterDefinition<TEntity> filter)
    {
        var collection = _database.GetCollection<TEntity>(collectionName);

        DeleteResult result = await collection.DeleteOneAsync(filter);
        return result.ToMongoTransactionResult(1);
    }

    /// <summary>
    /// Removes many documents
    /// </summary>
    /// <param name="collectionName">Name of the Collection</param>
    /// <param name="expectedRecords">Expected Number of Records to remove</param>
    /// <param name="filter">Filter Definition query</param>
    /// <returns><see cref="TransactionResult"/> results of the Delete operation</returns>
    public async Task<TransactionResult> DeleteManyAsync(string collectionName, long expectedRecords, FilterDefinition<TEntity> filter)
    {
        var collection = _database.GetCollection<TEntity>(collectionName);

        DeleteResult result = await collection.DeleteManyAsync(filter);
        return result.ToMongoTransactionResult(expectedRecords);
    }
}
