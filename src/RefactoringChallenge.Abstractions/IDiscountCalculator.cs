namespace RefactoringChallenge.Abstractions
{
    public interface IDiscountCalculator
    {
        decimal CalculateDiscountPercent(bool isCustomerVip, int customerRegistrationYear, decimal totalAmount);
    }
}
