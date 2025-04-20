using RefactoringChallenge.Abstractions;

namespace RefactoringChallenge.Implementation
{
    /// <summary>
    /// Calculates the discount for customer
    /// </summary>
    internal class DiscountCalculator : IDiscountCalculator
    {
        public decimal CalculateDiscountPercent(bool isCustomerVip, int customerRegistrationYear, decimal totalAmount)
        {
            if (customerRegistrationYear <= 2000 || customerRegistrationYear > DateTime.Now.Year) 
                throw new ArgumentOutOfRangeException(nameof(customerRegistrationYear));

            if (totalAmount <= 0)
                throw new ArgumentOutOfRangeException(nameof(totalAmount));

            decimal discountPercent = 0;

            if (isCustomerVip)
            {
                discountPercent += 10;
            }

            int yearsAsCustomer = DateTime.Now.Year - customerRegistrationYear;
            if (yearsAsCustomer >= 5)
            {
                discountPercent += 5;
            }
            else if (yearsAsCustomer >= 2)
            {
                discountPercent += 2;
            }

            if (totalAmount > 10000)
            {
                discountPercent += 15;
            }
            else if (totalAmount > 5000)
            {
                discountPercent += 10;
            }
            else if (totalAmount > 1000)
            {
                discountPercent += 5;
            }

            if (discountPercent > 25)
            {
                discountPercent = 25;
            }

            return discountPercent;
        }
    }
}
