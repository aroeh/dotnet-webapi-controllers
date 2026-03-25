namespace Demo.Restuarants.Shared.Models;

/// <summary>
/// Definition of transaction details
/// </summary>
/// <param name="TransactionRun">Represents if the transaction was run</param>
/// <param name="IsAcknowledged">Represents if the transaction was acknowledged by the Database</param>
/// <param name="ExpectedRecordCount">The number of records that were expected to be handled in the transaction</param>
/// <param name="ActualRecordCount">The actual number of records that were impacted in the transaction</param>
public record TransactionResult
(
    bool TransactionRun,
    bool IsAcknowledged,
    long ExpectedRecordCount,
    long ActualRecordCount
)
{
    public TransactionResult() : this(false, false, 0, 0)
    { }

    public TransactionResult(long expectedRecordCount) : this(false, false, expectedRecordCount, 0)
    { }

    public TransactionResult(bool transactionRun, long expectedRecordCount, long actualRecordCount) : this(transactionRun, false, expectedRecordCount, actualRecordCount)
    { }

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