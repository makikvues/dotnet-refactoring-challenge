namespace RefactoringChallenge.Abstractions.Models.Entities
{
    /// <summary>
    /// Customer database entity
    /// </summary>
    public class CustomerEntity
    {
        public int Id { get; set; }

        /// <summary>
        /// Customer name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Customer email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// is customer VIP
        /// </summary>
        public bool IsVip { get; set; }

        /// <summary>
        /// registration date
        /// </summary>
        public DateTime RegistrationDate { get; set; }

        // navigation properties
        public ICollection<OrderEntity> Orders { get; set; }
    }
}
