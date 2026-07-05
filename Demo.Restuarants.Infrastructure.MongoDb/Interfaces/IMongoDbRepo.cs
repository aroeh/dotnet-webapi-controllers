using Demo.Restuarants.Shared.Models;
using MongoDB.Driver;

namespace Demo.Restuarants.Infrastructure.MongoDb.Interfaces;

/// <summary>
/// Mongo DB Repository
/// </summary>
/// <remarks>
/// <para>
/// Methods for executing CRUD operations on a Mongo DB cluster
/// </para>
/// </remarks>
/// <typeparam name="TEntity">Entity matching collection model</typeparam>
public interface IMongoDbRepo<TEntity> where TEntity : class
{
    /// <summary>
    /// Gets a read only collection of entities
    /// </summary>
    /// <param name="filter">Filter Definition query</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Read only collection entities matching <paramref name="filter"/></returns>
    Task<IEnumerable<TEntity>> GetManyAsync(FilterDefinition<TEntity> filter, CancellationToken cancellationToken);

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
    Task<PaginationResponse<TEntity>> GetManyAsync(FilterDefinition<TEntity> filter, CancellationToken cancellationToken, int page = 1, int pageSize = 25);

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
    Task<PaginationResponse<TEntity>> GetManyAsync(FilterDefinition<TEntity> filter, PaginationQueryParametersBO pagination, CancellationToken cancellationToken);

    /// <summary>
    /// Get a single instance of an entity
    /// </summary>
    /// <param name="filter">Filter Definition query</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Instance of a entity matching <paramref name="filter"/></returns>
    Task<TEntity?> GetAsync(FilterDefinition<TEntity> filter, CancellationToken cancellationToken);

    /// <summary>
    /// Insert a new document into the specified collection
    /// </summary>
    /// <param name="document">Document to be created in the <see cref="IMongoCollection"/> collection</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns>Newly created document</returns>
    Task<TEntity> CreateOneAsync(TEntity document, CancellationToken cancellationToken);

    /// <summary>
    /// Insert multiple new documents into the specified collection
    /// </summary>
    /// <param name="documents">Documents to be created in the <see cref="IMongoCollection"/> collection</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns><see cref="TransactionResult"/> results of the Create operation</returns>
    Task<TransactionResult> CreateManyAsync(IEnumerable<TEntity> documents, CancellationToken cancellationToken);

    /// <summary>
    /// Replaces a document with a later version using a filter to match the record
    /// </summary>
    /// <param name="filter">Filter Definition query</param>
    /// <param name="document">Document to be replaced in the <see cref="IMongoCollection"/> collection</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns><see cref="TransactionResult"/> results of the Replace operation</returns>
    Task<TransactionResult> ReplaceOneAsync(FilterDefinition<TEntity> filter, TEntity document, CancellationToken cancellationToken);

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
    Task<TransactionResult> UpdateOneAsync(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update, CancellationToken cancellationToken);

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
    Task<TransactionResult> UpdateOneAsync(FilterDefinition<TEntity> filter, List<UpdateDefinition<TEntity>> updates, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a document
    /// </summary>
    /// <param name="filter">Filter Definition query</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns><see cref="TransactionResult"/> results of the Delete operation</returns>
    Task<TransactionResult> DeleteOneAsync(FilterDefinition<TEntity> filter, CancellationToken cancellationToken);

    /// <summary>
    /// Removes many documents
    /// </summary>
    /// <param name="expectedRecords">Expected Number of Records to remove</param>
    /// <param name="filter">Filter Definition query</param>
    /// <param name="cancellationToken">Token for handling cancellation requests</param>
    /// <returns><see cref="TransactionResult"/> results of the Delete operation</returns>
    Task<TransactionResult> DeleteManyAsync(long expectedRecords, FilterDefinition<TEntity> filter, CancellationToken cancellationToken);
}
