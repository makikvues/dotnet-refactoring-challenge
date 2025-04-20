namespace RefactoringChallenge.Abstractions.Models.Entities
{
    /// <summary>
    /// product database entity
    /// </summary>
    public class ProductEntity
    {
        public int Id { get; set; }

        /// <summary>
        /// product name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// product category
        /// </summary>
        public string Category { get; set; }

        // product price
        public decimal Price { get; set; }

        // navigation properties
        public ICollection<InventoryEntity> Inventories { get; set; }
        public ICollection<OrderItemEntity> OrderItems { get; set; }
    }
}
