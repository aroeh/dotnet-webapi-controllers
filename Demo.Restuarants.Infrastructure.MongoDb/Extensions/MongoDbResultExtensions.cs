using Demo.Restuarants.Shared.Models;
using MongoDB.Driver;

namespace Demo.Restuarants.Infrastructure.MongoDb.Extensions;

internal static class MongoDbResultExtensions
{
    internal static TransactionResult ToMongoTransactionResult(this ReplaceOneResult result, long expectedRecordCount = 1, bool transactionRun = true)
    {
        return new TransactionResult
        (
            transactionRun,
            result.IsAcknowledged,
            expectedRecordCount,
            result.ModifiedCount
        );
    }

    internal static TransactionResult ToMongoTransactionResult(this UpdateResult result, long expectedRecordCount = 1, bool transactionRun = true)
    {
        return new TransactionResult
        (
            transactionRun,
            result.IsAcknowledged,
            expectedRecordCount,
            result.ModifiedCount
        );
    }

    internal static TransactionResult ToMongoTransactionResult(this DeleteResult result, long expectedRecordCount, bool transactionRun = true)
    {
        return new TransactionResult
        (
            transactionRun,
            result.IsAcknowledged,
            expectedRecordCount,
            result.DeletedCount
        );
    }
}
