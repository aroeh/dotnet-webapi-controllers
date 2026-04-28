using Demo.Restuarants.Infrastructure.MongoDb.Constants;
using Demo.Restuarants.Shared.Models;
using MongoDB.Driver;

namespace Demo.Restuarants.Infrastructure.MongoDb.Extensions;

internal static class MongoDbResultExtensions
{
    internal static TransactionResult ToTransactionResult(this ReplaceOneResult result, long expectedRecordCount = 1, bool transactionRun = true)
    {
        return new TransactionResult
        (
            transactionRun,
            result.IsAcknowledged,
            expectedRecordCount,
            result.ModifiedCount,
            DatabaseConstants.Updated
        );
    }

    internal static TransactionResult ToTransactionResult(this UpdateResult result, long expectedRecordCount = 1, bool transactionRun = true)
    {
        return new TransactionResult
        (
            transactionRun,
            result.IsAcknowledged,
            expectedRecordCount,
            result.ModifiedCount,
            DatabaseConstants.Updated
        );
    }

    internal static TransactionResult ToTransactionResult(this DeleteResult result, long expectedRecordCount, bool transactionRun = true)
    {
        return new TransactionResult
        (
            transactionRun,
            result.IsAcknowledged,
            expectedRecordCount,
            result.DeletedCount,
            DatabaseConstants.Deleted
        );
    }
}
