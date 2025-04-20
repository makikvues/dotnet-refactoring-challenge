using RefactoringChallenge.Abstractions.Models.Entities;
using RefactoringChallenge.Abstractions.Repository;
using RefactoringChallenge.Persistence.Database;

namespace RefactoringChallenge.Persistence.Implementation.Repository
{
    /// <summary>
    /// Order logs repository
    /// </summary>
    /// <param name="dbContext">database context</param>
    public class OrderLogsRepository(RefactoringDbContext dbContext) : IOrderLogsRepository
    {
        public async Task<bool> CreateAsync(int orderId, DateTime date, string message)
        {
            if (orderId < 0)
                throw new ArgumentOutOfRangeException(nameof(orderId));

            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message));

            await dbContext.AddAsync(
                new OrderLogsEntity
                {
                    OrderId = orderId,
                    LogDate = date,
                    Message = message
                });

            var result = await dbContext.SaveChangesAsync();

            return result > 0;
        }
    }
}
