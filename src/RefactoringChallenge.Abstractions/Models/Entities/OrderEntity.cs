namespace RefactoringChallenge.Abstractions.Models.Entities
{
    /// <summary>
    /// order database entity
    /// </summary>
    public class OrderEntity
    {
        public int Id { get; set; }

        /// <summary>
        /// order date
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// total amount
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// discount percent
        /// </summary>
        public decimal DiscountPercent { get; set; }

        /// <summary>
        /// discount amount
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// status
        /// </summary>
        public int Status { get; set; }

        // navigation properties
        public int CustomerId { get; set; }
        public CustomerEntity Customer { get; set; }
        public ICollection<OrderItemEntity> OrderItems { get; set; }
        public ICollection<OrderLogsEntity> OrderLogs { get; set; }
    }
}
