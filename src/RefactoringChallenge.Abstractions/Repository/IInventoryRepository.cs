namespace RefactoringChallenge.Abstractions.Repository
{
    public interface IInventoryRepository
    {
        Task<int?> GetStockQuantityByProductIdAsync(int productId);
        Task<bool> UpdateStockQuantityByProductIdAsync(int productId, int addedStockQuantity);
    }
}
