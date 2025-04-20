using RefactoringChallenge.Abstractions.Models.Entities;

namespace RefactoringChallenge.Abstractions.Repository
{
    public interface IOrderRepository
    {
        IEnumerable<OrderEntity> GetPendingOrdersByCustomerId(int customerId);
        Task<bool> UpdateAsync(OrderEntity order);
    }
}
