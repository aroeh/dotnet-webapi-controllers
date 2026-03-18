using Demo.Restuarants.API.Shared.Models;
using MongoDB.Driver;

namespace Demo.Restuarants.API.MongoDb.Infrastructure.Extensions;

internal static class MongoDbResultExtensions
{
    internal static MongoTransactionResult ToMongoTransactionResult(this ReplaceOneResult result, long expectedRecordCount = 1, bool transactionRun = true)
    {
        return new MongoTransactionResult
        {
            TransactionRun = transactionRun,
            IsAcknowledged = result.IsAcknowledged,
            ExpectedRecordCount = expectedRecordCount,
            ActualRecordCount = result.ModifiedCount
        };
    }

    internal static MongoTransactionResult ToMongoTransactionResult(this UpdateResult result, long expectedRecordCount = 1, bool transactionRun = true)
    {
        return new MongoTransactionResult
        {
            TransactionRun = transactionRun,
            IsAcknowledged = result.IsAcknowledged,
            ExpectedRecordCount = expectedRecordCount,
            ActualRecordCount = result.ModifiedCount
        };
    }

    internal static MongoTransactionResult ToMongoTransactionResult(this DeleteResult result, long expectedRecordCount, bool transactionRun = true)
    {
        return new MongoTransactionResult
        {
            TransactionRun = transactionRun,
            IsAcknowledged = result.IsAcknowledged,
            ExpectedRecordCount = expectedRecordCount,
            ActualRecordCount = result.DeletedCount
        };
    }
}
