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
    /// <param name="collectionName">Name of the Collection</param>
    /// <param name="filter">Filter Definition query</param>
    /// <returns>Read only collection entities matching <paramref name="filter"/></returns>
    Task<IEnumerable<TEntity>> GetManyAsync(string collectionName, FilterDefinition<TEntity> filter);

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
    Task<PaginationResponse<TEntity>> GetManyAsync(string collectionName, FilterDefinition<TEntity> filter, PaginationQueryParametersBO pagination);

    /// <summary>
    /// Get a single instance of an entity
    /// </summary>
    /// <param name="collectionName">Name of the Collection</param>
    /// <param name="filter">Filter Definition query</param>
    /// <returns>Instance of a entity matching <paramref name="filter"/></returns>
    Task<TEntity?> GetAsync(string collectionName, FilterDefinition<TEntity> filter);

    /// <summary>
    /// Insert a new document into the specified collection
    /// </summary>
    /// <param name="collectionName">Name of the Collection</param>
    /// <param name="document">Document to be created in the <paramref name="collectionName"/> collection</param>
    /// <returns>Newly created document</returns>
    Task<TEntity> CreateOneAsync(string collectionName, TEntity document);

    /// <summary>
    /// Insert multiple new documents into the specified collection
    /// </summary>
    /// <param name="collectionName">Name of the Collection</param>
    /// <param name="documents">Documents to be created in the <paramref name="collectionName"/> collection</param>
    /// <returns>Operation success result of true or false</returns>
    Task<TransactionResult> CreateManyAsync(string collectionName, IEnumerable<TEntity> documents);

    /// <summary>
    /// Replaces a document with a later version using a filter to match the record
    /// </summary>
    /// <param name="collectionName">Name of the Collection</param>
    /// <param name="filter">Filter Definition query</param>
    /// <param name="document">Document to be replaced in the <paramref name="collectionName"/> collection</param>
    /// <returns><see cref="TransactionResult"/> results of the Replace operation</returns>
    Task<TransactionResult> ReplaceOneAsync(string collectionName, FilterDefinition<TEntity> filter, TEntity document);

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
    Task<TransactionResult> UpdateOneAsync(string collectionName, FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update);

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
    Task<TransactionResult> UpdateOneAsync(string collectionName, FilterDefinition<TEntity> filter, List<UpdateDefinition<TEntity>> updates);

    /// <summary>
    /// Removes a document
    /// </summary>
    /// <param name="collectionName">Name of the Collection</param>
    /// <param name="filter">Filter Definition query</param>
    /// <returns><see cref="TransactionResult"/> results of the Delete operation</returns>
    Task<TransactionResult> DeleteOneAsync(string collectionName, FilterDefinition<TEntity> filter);

    /// <summary>
    /// Removes many documents
    /// </summary>
    /// <param name="collectionName">Name of the Collection</param>
    /// <param name="expectedRecords">Expected Number of Records to remove</param>
    /// <param name="filter">Filter Definition query</param>
    /// <returns><see cref="TransactionResult"/> results of the Delete operation</returns>
    Task<TransactionResult> DeleteManyAsync(string collectionName, long expectedRecords, FilterDefinition<TEntity> filter);
}
