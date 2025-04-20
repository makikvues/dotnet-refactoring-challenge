using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RefactoringChallenge.Abstractions.Models.Entities;
using RefactoringChallenge.Abstractions.Models.Enums;
using RefactoringChallenge.Abstractions.Repository;
using RefactoringChallenge.Persistence.Database;

namespace RefactoringChallenge.Persistence.Implementation.Repository
{
    /// <summary>
    /// Order repository
    /// </summary>
    /// <param name="dbContext">database context</param>
    /// <param name="logger">logger</param>
    public class OrderRepository(RefactoringDbContext dbContext, ILogger<OrderRepository> logger) : IOrderRepository
    {
        public IEnumerable<OrderEntity> GetPendingOrdersByCustomerId(int customerId)
        {
            var pendingOrders = dbContext.Orders
                .Where(o => o.CustomerId == customerId && o.Status == (int) OrderStatus.Pending)
                .Include(o => o.OrderItems);

            return pendingOrders;
        }

        public async Task<bool> UpdateAsync(OrderEntity order)
        {
            dbContext.Orders.Update(order);
            var result = await dbContext.SaveChangesAsync();

            return result > 0;
        }
    }
}
