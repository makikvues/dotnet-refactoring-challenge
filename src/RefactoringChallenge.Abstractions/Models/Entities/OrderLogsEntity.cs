
namespace RefactoringChallenge.Abstractions.Models.Entities
{
    /// <summary>
    /// Order logs database entity
    /// </summary>
    public class OrderLogsEntity
    {
        public int Id { get; set; }

        /// <summary>
        /// log date
        /// </summary>
        public DateTime LogDate { get; set; }

        /// <summary>
        /// log message
        /// </summary>
        public string Message { get; set; }

        // navigation properties
        public int OrderId { get; set; }
        public OrderEntity Order { get; set; }
    }
}
