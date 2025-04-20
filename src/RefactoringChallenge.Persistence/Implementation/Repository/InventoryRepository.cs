using Microsoft.EntityFrameworkCore;
using RefactoringChallenge.Abstractions.Repository;
using RefactoringChallenge.Persistence.Database;

namespace RefactoringChallenge.Persistence.Implementation.Repository
{
    /// <summary>
    /// Inventory repository
    /// </summary>
    /// <param name="dbContext">database context</param>
    public class InventoryRepository(RefactoringDbContext dbContext) : IInventoryRepository
    {
        public async Task<int?> GetStockQuantityByProductIdAsync(int productId)
        {
            var inventory = await dbContext.Inventories.FirstOrDefaultAsync(x => x.ProductId == productId);

            if (inventory != null)
            {
                return inventory.StockQuantity;
            }

            return null;
        }

        public async Task<bool> UpdateStockQuantityByProductIdAsync(int productId, int addedStockQuantity)
        {
            var inventory = await dbContext.Inventories.FirstOrDefaultAsync(x => x.ProductId == productId);

            if (inventory == null)
                throw new ArgumentException($"Inventory with product id: '{productId}' could not be found.");

            inventory.StockQuantity += addedStockQuantity;

            var result = await dbContext.SaveChangesAsync();

            return result > 0;
        }
    }
}
