namespace RefactoringChallenge.Abstractions.Models.Entities
{
    /// <summary>
    /// Inventory database entity
    /// </summary>
    public class InventoryEntity
    {
        public int Id {  get; set; }

        /// <summary>
        /// stock quantity
        /// </summary>
        public int StockQuantity { get; set; }

        // navigation properties
        public int ProductId {  get; set; }
        public ProductEntity Product { get; set; }
    }
}
