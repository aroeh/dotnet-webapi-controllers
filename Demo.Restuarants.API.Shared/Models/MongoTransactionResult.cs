namespace Demo.Restuarants.API.Shared.Models;

public record MongoTransactionResult
{
    /// <summary>
    /// Represents if the transaction was run
    /// </summary>
    public bool TransactionRun { get; set; }

    /// <summary>
    /// Represents if the transaction was acknowledged by the MongoDB Database
    /// </summary>
    public bool IsAcknowledged { get; set; }

    /// <summary>
    /// The number of records that were expected to be handled in the transaction
    /// </summary>
    public long ExpectedRecordCount { get; set; }

    /// <summary>
    /// The actual number of records that were impacted in the transaction
    /// </summary>
    public long ActualRecordCount { get; set; }

    /// <summary>
    /// Success indicator.  Success is defined as true if 
    /// 1. The transaction was not run
    /// 
    /// or
    /// 
    /// 1. If the transaction was run acknowleded
    /// 2. The number of actual impacted records is more than 0
    /// 3. The number of expected and actual records is the same
    /// </summary>
    public bool Success => !TransactionRun || (IsAcknowledged && ActualRecordCount > 0 && ExpectedRecordCount.Equals(ActualRecordCount));
}