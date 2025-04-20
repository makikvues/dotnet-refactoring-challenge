namespace RefactoringChallenge.Tests
{
    using FluentAssertions;
    using NUnit.Framework;
    using RefactoringChallenge.Implementation;

    public class DiscountCalculatorTests
    {
        [TestCase(true, 2020, 100, 15)]
        // add more test cases, test edge cases, catch exceptions, etc.
        public void ShouldCalculateDiscountCorrectly(bool isCustomerVip, int customerRegistrationYear, decimal totalAmount, decimal expectedDiscountPercent)
        {
            // arrange
            var sut = new DiscountCalculator();

            // act
            var calculatedDiscount = sut.CalculateDiscountPercent(isCustomerVip, customerRegistrationYear, totalAmount);

            // assert
            calculatedDiscount.Should().Be(expectedDiscountPercent);
        }
    }
}