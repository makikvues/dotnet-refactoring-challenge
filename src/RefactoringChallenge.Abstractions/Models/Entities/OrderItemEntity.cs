namespace RefactoringChallenge.Abstractions.Models.Entities
{
    /// <summary>
    /// Order item database entity
    /// </summary>
    public class OrderItemEntity
    {
        public int Id { get; set; }

        /// <summary>
        /// quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// unit price
        /// </summary>
        public decimal UnitPrice { get; set; }
       
        // navigation properties
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public OrderEntity Order { get; set; }
        public ProductEntity Product { get; set; }
    }
}
