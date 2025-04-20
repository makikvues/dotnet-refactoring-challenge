namespace RefactoringChallenge.Abstractions.Repository
{
    public interface IOrderLogsRepository
    {
        Task<bool> CreateAsync(int orderId, DateTime date, string message);
    }
}
