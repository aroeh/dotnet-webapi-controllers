using Demo.Restuarants.Infrastructure.MongoDb.Constants;
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
    private readonly IMongoCollection<TEntity> _collection;
    private readonly ILogger<MongoDbRepo<TEntity>> _logger;

    /// <summary>
    /// Create a new instance of the MongoDbRepo with connection options
    /// </summary>
    /// <param name="logger">Logging instance</param>
    /// <param name="options">Configured MongoDb options</param>
    /// <param name="collectionName">Name of the registered collection to use with the connected database</param>
    public MongoDbRepo(ILogger<MongoDbRepo<TEntity>> logger, IOptions<MongoDbOptions> options, string collectionName)
    {
        _logger = logger;

        _logger.LogInformation("Configuring MongoDB Client");
        MongoClient client = new(options.Value.ConnectionString);

        _logger.LogInformation("Configuring MongoDB Database");
        IMongoDatabase database = client.GetDatabase(options.Value.DatabaseName);

        _logger.LogInformation("Retrieving collection from the MongoDB Database");
        _collection = database.GetCollection<TEntity>(collectionName);

        _logger.LogInformation("MongoDB Connection established and service ready");
    }

    /// <summary>
    /// Gets a read only collection of entities
    /// </summary>
    /// <param name="filter">Filter Definition query</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Read only collection of entities matching <paramref name="filter"/></returns>
    public async Task<IEnumerable<TEntity>> GetManyAsync(FilterDefinition<TEntity> filter, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Finding items by Filter");
        return await _collection.Find(filter).ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Gets a paginated collection of entities
    /// </summary>
    /// <param name="filter">Filter Definition query</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <param name="page">Page number to retrieve.  Defaults to 1.</param>
    /// <param name="pageSize">Number of results to return for each page.  Defaults to 25.</param>
    /// <remarks>
    /// Uses Offset pagination implementation
    /// </remarks>
    /// <returns>Paginated collection of entities matching <paramref name="filter"/></returns>
    public async Task<PaginationResponse<TEntity>> GetManyAsync(FilterDefinition<TEntity> filter, CancellationToken cancellationToken, int page = 1, int pageSize = 25)
    {
        var totalCount = await _collection.EstimatedDocumentCountAsync(null, cancellationToken);
        var skipToPosition = page == 1 ? 0 : (page - 1) * pageSize;

        _logger.LogInformation("Finding items by Filter");
        var results = await _collection
            .Find(filter)
            .Skip(skipToPosition)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);

        PaginationMetaData metaData = new(page, results.Count, pageSize, totalCount);
        return new PaginationResponse<TEntity>(results, metaData);
    }

    /// <summary>
    /// Gets a paginated collection of entities
    /// </summary>
    /// <param name="filter">Filter Definition query</param>
    /// <param name="pagination">Pagination parameters</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <remarks>
    /// Uses Offset pagination implementation
    /// </remarks>
    /// <returns>Paginated collection of entities matching <paramref name="filter"/></returns>
    public async Task<PaginationResponse<TEntity>> GetManyAsync(FilterDefinition<TEntity> filter, PaginationQueryParametersBO pagination, CancellationToken cancellationToken)
    {
        return await GetManyAsync(filter, cancellationToken, pagination.Page, pagination.PageSize);
    }

    /// <summary>
    /// Get a single instance of an entity
    /// </summary>
    /// <param name="filter">Filter Definition query</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Instance of a entity matching <paramref name="filter"/></returns>
    public async Task<TEntity?> GetAsync(FilterDefinition<TEntity> filter, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Finding items by Filter");
        return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Insert a new document into the specified collection
    /// </summary>
    /// <param name="document">Document to be created in the <see cref="IMongoCollection"/> collection</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Newly created document</returns>
    public async Task<TEntity> CreateOneAsync(TEntity document, CancellationToken cancellationToken)
    {
        _logger.LogInformation("inserting new document");
        await _collection.InsertOneAsync(document, null, cancellationToken);
        return document;
    }

    /// <summary>
    /// Insert multiple new documents into the specified collection
    /// </summary>
    /// <param name="documents">Documents to be created in the <see cref="IMongoCollection"/> collection</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns><see cref="TransactionResult"/> results of the Create operation</returns>
    public async Task<TransactionResult> CreateManyAsync(IEnumerable<TEntity> documents, CancellationToken cancellationToken)
    {
        if (!documents.Any())
        {
            _logger.LogInformation("There are no new documents to create...returning result");
            return new TransactionResult(DatabaseConstants.Created);
        }

        _logger.LogInformation("inserting new documents");
        await _collection.InsertManyAsync(documents, null, cancellationToken);
        return new TransactionResult
        (
            documents.Count(),
            documents.Count(),
            DatabaseConstants.Created
        );
    }

    /// <summary>
    /// Replaces a document with a later version using a filter to match the record
    /// </summary>
    /// <param name="filter">Filter Definition query</param>
    /// <param name="document">Document to be replaced in the <see cref="IMongoCollection"/> collection</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns><see cref="TransactionResult"/> results of the Replace operation</returns>
    public async Task<TransactionResult> ReplaceOneAsync(FilterDefinition<TEntity> filter, TEntity document, CancellationToken cancellationToken)
    {
        _logger.LogInformation("starting replace operation");
        ReplaceOneResult result = await _collection.ReplaceOneAsync(filter, document, (ReplaceOptions?)null, cancellationToken);

        _logger.LogInformation("operation completed...returning result");
        return result.ToTransactionResult();
    }

    /// <summary>
    /// Updates a document
    /// </summary>
    /// <remarks>
    /// Requires there be an update operation
    /// </remarks>
    /// <param name="filter">Filter Definition query</param>
    /// <param name="update">Update Definition</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns><see cref="TransactionResult"/> results of the Update operation</returns>
    public async Task<TransactionResult> UpdateOneAsync(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update, CancellationToken cancellationToken)
    {
        UpdateResult result = await _collection.UpdateOneAsync(filter, update, null, cancellationToken);
        return result.ToTransactionResult();
    }

    /// <summary>
    /// Updates a document
    /// </summary>
    /// <remarks>
    /// Use for a multiple updates on a document
    /// </remarks>
    /// <param name="filter">Filter Definition query</param>
    /// <param name="updates">Collection of Update Definitions for a model</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns><see cref="TransactionResult"/> results of the Update operation</returns>
    public async Task<TransactionResult> UpdateOneAsync(FilterDefinition<TEntity> filter, List<UpdateDefinition<TEntity>> updates, CancellationToken cancellationToken)
    {
        if (updates.Count == 0)
        {
            _logger.LogInformation("There are no updates to perform...returning result");
            return new TransactionResult(DatabaseConstants.Updated);
        }

        var update = Builders<TEntity>.Update;
        return await UpdateOneAsync(filter, update.Combine(updates), cancellationToken);
    }

    /// <summary>
    /// Removes a document
    /// </summary>
    /// <param name="filter">Filter Definition query</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns><see cref="TransactionResult"/> results of the Delete operation</returns>
    public async Task<TransactionResult> DeleteOneAsync(FilterDefinition<TEntity> filter, CancellationToken cancellationToken)
    {
        DeleteResult result = await _collection.DeleteOneAsync(filter, cancellationToken);
        return result.ToTransactionResult(1);
    }

    /// <summary>
    /// Removes many documents
    /// </summary>
    /// <param name="expectedRecords">Expected Number of Records to remove</param>
    /// <param name="filter">Filter Definition query</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns><see cref="TransactionResult"/> results of the Delete operation</returns>
    public async Task<TransactionResult> DeleteManyAsync(long expectedRecords, FilterDefinition<TEntity> filter, CancellationToken cancellationToken)
    {
        DeleteResult result = await _collection.DeleteManyAsync(filter, cancellationToken);
        return result.ToTransactionResult(expectedRecords);
    }
}
