using RefactoringChallenge.Abstractions.Models.Entities;

namespace RefactoringChallenge.Abstractions
{
    public interface ICustomerOrderProcessor
    {
        Task<IList<OrderEntity>> ProcessCustomerOrdersAsync(int customerId);
    }
}
