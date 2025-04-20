using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RefactoringChallenge.Abstractions.Models.Entities;
using RefactoringChallenge.Abstractions.Repository;
using RefactoringChallenge.Persistence.Database;

namespace RefactoringChallenge.Persistence.Implementation.Repository
{
    /// <summary>
    /// Customer repository
    /// </summary>
    /// <param name="dbContext">database context</param>
    /// <param name="logger">logger</param>
    public class CustomerRepository(
        RefactoringDbContext dbContext,
        ILogger<CustomerRepository> logger) : ICustomerRepository
    {
        public Task<CustomerEntity> GetCustomerByIdAsync(int customerId) => dbContext.Customers.FirstOrDefaultAsync(c => c.Id == customerId);
    }
}
