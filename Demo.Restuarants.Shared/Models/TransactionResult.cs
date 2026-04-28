using System.Text;

namespace Demo.Restuarants.Shared.Models;

/// <summary>
/// Definition of transaction details
/// </summary>
/// <param name="TransactionRun">Represents if the transaction was run</param>
/// <param name="IsAcknowledged">Represents if the transaction was acknowledged by the Database</param>
/// <param name="ExpectedRecordCount">The number of records that were expected to be handled in the transaction</param>
/// <param name="ActualRecordCount">The actual number of records that were impacted in the transaction</param>
/// <param name="OperationCompleted">The operation that was run.  Created, Updated, Deleted</param>
public record TransactionResult
(
    bool TransactionRun,
    bool IsAcknowledged,
    long ExpectedRecordCount,
    long ActualRecordCount,
    string OperationCompleted
)
{
    public TransactionResult(string operationCompleted)
        : this(false, false, 0, 0, operationCompleted) { }

    public TransactionResult(long expectedRecordCount, string operationCompleted)
        : this(false, false, expectedRecordCount, 0, operationCompleted) { }

    public TransactionResult(long expectedRecordCount, long actualRecordCount, string operationCompleted)
        : this(actualRecordCount > 0, actualRecordCount > 0, expectedRecordCount, actualRecordCount, operationCompleted) { }

    public TransactionResult(bool ackknowledged, long expectedRecordCount, long actualRecordCount, string operationCompleted)
        : this(actualRecordCount > 0, ackknowledged, expectedRecordCount, actualRecordCount, operationCompleted) { }

    /// <summary>
    /// Success indicator.  Success is defined as true if 
    /// 1. The transaction was not run
    /// 
    /// or
    /// 
    /// 1. If the transaction run was acknowleded
    /// 2. The number of actual impacted records is more than 0
    /// 3. The number of expected and actual records is the same
    /// </summary>
    public bool Success => !TransactionRun || (IsAcknowledged && ActualRecordCount > 0 && ExpectedRecordCount.Equals(ActualRecordCount));

    public string Message => GetMessage();

    private string GetMessage()
    {
        StringBuilder sb = new();

        if (TransactionRun)
        {
            sb.AppendLine("The transaction was run and completed successfully.");
        }
        else
        {
            sb.AppendLine("The transaction was not run.");
        }

        string expectedCountMsg = ExpectedRecordCount == 0
            ? $"No records were expected to be {OperationCompleted}."
            : ExpectedRecordCount > 1
                ? $"{ExpectedRecordCount} records were expected to be {OperationCompleted}."
                : $"{ExpectedRecordCount} record was expected to be {OperationCompleted}.";
        sb.AppendLine(expectedCountMsg);

        string actualCountMsg = ActualRecordCount == 0
            ? $"No records were {OperationCompleted}."
            : ActualRecordCount > 1
                ? $"{ActualRecordCount} records were {OperationCompleted}."
                : $"{ActualRecordCount} record was {OperationCompleted}.";
        sb.AppendLine(actualCountMsg);

        return sb.ToString();
    }
}