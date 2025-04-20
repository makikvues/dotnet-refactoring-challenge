using Microsoft.EntityFrameworkCore;
using RefactoringChallenge.Abstractions.Models.Entities;

namespace RefactoringChallenge.Persistence.Database
{
    public class RefactoringDbContext : DbContext
    {
        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<OrderItemEntity> OrderItems { get; set; }
        public DbSet<InventoryEntity> Inventories { get; set; }
        public DbSet<OrderLogsEntity> OrderLogs { get; set; }
        
        public RefactoringDbContext(DbContextOptions<RefactoringDbContext> options) : base(options) 
        {
            
        }
    }
}
