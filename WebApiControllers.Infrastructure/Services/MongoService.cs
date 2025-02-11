﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MongoDB.Driver;
using System.Diagnostics;
using WebApiControllers.Infrastructure.Constants;
using WebApiControllers.Shared.Models;

namespace WebApiControllers.Infrastructure.Services;

internal class MongoService
{
    /// <summary>
    /// Instance of the MongoClient object
    /// </summary>
    private readonly MongoClient client;

    /// <summary>
    /// Database instance from the client connection
    /// </summary>
    private readonly IMongoDatabase database;

    private readonly ILogger<MongoService> logger;

    /// <summary>
    /// For this project depending on your resources available - you will need to setup the connection string
    /// </summary>
    /// <param name="log"></param>
    /// <param name="config"></param>
    internal MongoService(ILoggerFactory logFactory, IConfiguration config)
    {
        logFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });
        logger = logFactory.CreateLogger<MongoService>();

        // provide the key value to use to lookup the connection string from secrets

        // Local secrets - uncomment this line if using local secrets config to store the connection string
        // client = new(config.GetValue<string>(DataAccessConstants.MongoConn));

        // Environment Variable - uncomment this line if passing in the connection string via an env variable
        // probably most commonly used with local containers for simplicity.  Ideally, this will pulled from secrets
        logger.LogInformation("Retrieving MongoDB Connection string from ENV Variables");
        logger.LogInformation("Configuring MongoDB Client");
        client = new(Environment.GetEnvironmentVariable(DataAccessConstants.MongoConn));

        // configure the client and set a database name ideally from a constants file
        logger.LogInformation("Configuring MongoDB Database");
        database = client.GetDatabase(DataAccessConstants.MongoDatabase);

        logger.LogInformation("MongoDB Connection established and service ready");
    }

    /// <summary>
    /// Checks the connection to the database and returns basic data parameters
    /// </summary>
    /// <returns>Dictionary of Connection Results</returns>
    internal async Task<Dictionary<string, object>> ConnectionEstablished()
    {
        try
        {
            CancellationToken token = new();

            long connectionTestStart = Stopwatch.GetTimestamp();
            var dbNames = await client.ListDatabaseNamesAsync(token);
            TimeSpan connectionTestDuration = Stopwatch.GetElapsedTime(connectionTestStart);

            Dictionary<string, object> data = [];

            if (dbNames is not null)
            {
                data.Add("Connected", true);
                data.Add("TestDuration", connectionTestDuration);
            }

            return data;
        }
        catch (Exception ex)
        {
            logger.LogError(99, ex, "Unable to establish database connection");
            Dictionary<string, object> errorData = new()
            {
                { "ErrorCode", 99 },
                { "Connected", false }
            };
            return errorData;
        }
    }

    /// <summary>
    /// Finds items in the specified collection using a filter definition
    /// </summary>
    /// <typeparam name="T">Generic Type for the input and output objects</typeparam>
    /// <param name="collectionName">MongoDb Collection Name</param>
    /// <param name="filter">Expression Filter to match documents</param>
    /// <returns>A Task value containing a List of Documents of type T</returns>
    internal Task<List<T>> FindMany<T>(string collectionName, FilterDefinition<T> filter)
    {
        var collection = database.GetCollection<T>(collectionName);

        logger.LogInformation("Finding items by Filter");
        return collection.Find(filter).ToListAsync();
    }

    /// <summary>
    /// Finds one item in the specified collection using a filter definition
    /// </summary>
    /// <typeparam name="T">Generic Type for the input and output objects</typeparam>
    /// <param name="collectionName">MongoDb Collection Name</param>
    /// <param name="filter">Expression Filter to match documents</param>
    /// <returns>Task with a value of type T</returns>
    internal Task<T> FindOne<T>(string collectionName, FilterDefinition<T> filter)
    {
        var collection = database.GetCollection<T>(collectionName);

        logger.LogInformation("Finding items by Filter");
        return collection.Find(filter).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Insert a new document into the specified collection
    /// </summary>
    /// <typeparam name="T">Generic Type for the input and output objects</typeparam>
    /// <param name="collectionName"></param>
    /// <param name="document"></param>
    /// <returns>Task T with updated id from the insert operation</returns>
    internal async Task<T> InsertOne<T>(string collectionName, T document)
    {
        var collection = database.GetCollection<T>(collectionName);

        logger.LogInformation("inserting new document");
        await collection.InsertOneAsync(document);

        // Might have to update this and return an id
        return document;
    }

    /// <summary>
    /// Insert multiple new documents into the specified collection
    /// </summary>
    /// <typeparam name="T">Generic Type for the input and output objects</typeparam>
    /// <param name="collectionName">MongoDb Collection Name</param>
    /// <param name="documents">New Documents to be inserted</param>
    /// <returns>Task Enumerable of T with updated ids from the insert operation</returns>
    internal async Task<IEnumerable<T>> InsertMany<T>(string collectionName, IEnumerable<T> documents)
    {
        var collection = database.GetCollection<T>(collectionName);

        logger.LogInformation("inserting new documents");
        await collection.InsertManyAsync(documents);
        return documents;
    }

    /// <summary>
    /// Replaces a document with a later version using a filter to match the record
    /// </summary>
    /// <typeparam name="T">Generic Type for the input and output objects</typeparam>
    /// <param name="collectionName">MongoDb Collection Name</param>
    /// <param name="filter">Expression Filter to match documents</param>
    /// <param name="document">Latest version of the document</param>
    /// <returns>MongoUpdateResult</returns>
    internal async Task<MongoUpdateResult> ReplaceOne<T>(string collectionName, FilterDefinition<T> filter, T document)
    {
        var collection = database.GetCollection<T>(collectionName);

        logger.LogInformation("starting replace operation");
        ReplaceOneResult result = await collection.ReplaceOneAsync(filter, document);

        logger.LogInformation("operation completed...returning result");
        return new MongoUpdateResult
        {
            IsAcknowledged = result.IsAcknowledged,
            ModifiedCount = result.ModifiedCount
        };
    }
}
