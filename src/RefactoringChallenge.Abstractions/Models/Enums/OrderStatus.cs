namespace RefactoringChallenge.Abstractions.Models.Enums
{
    /// <summary>
    /// order status
    /// </summary>
    public enum OrderStatus
    {
        // order is pending
        Pending,

        // order is ready to be processed
        Ready,

        // order is on hold, e.g. some stock items were not available
        OnHold,

        // order is processed successfully
        Processed,
    }
}
