using RefactoringChallenge.Abstractions.Models.Entities;

namespace RefactoringChallenge.Abstractions.Repository
{
    public interface ICustomerRepository
    {
        Task<CustomerEntity> GetCustomerByIdAsync(int customerId);
    }
}
