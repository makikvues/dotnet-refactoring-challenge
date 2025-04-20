using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RefactoringChallenge.Abstractions;
using RefactoringChallenge.Abstractions.Models.Entities;
using RefactoringChallenge.Abstractions.Models.Enums;
using RefactoringChallenge.Abstractions.Repository;
using RefactoringChallenge.Implementation;

namespace RefactoringChallenge.Tests
{
    public class CustomerOrderProcessorTests
    {
        [Test]
        public async Task ShouldProcessCorrectly()
        {
            // TODO:
            // we can also setup in-memory DB for tests (persistence layer), or have a docker container with sql for tests
            // add more tests to test edge cases, exceptions, etc.

            // arrange
            Mock<ICustomerRepository> customerRepositoryMock = new Mock<ICustomerRepository>(MockBehavior.Strict);
            Mock<IOrderRepository> orderRepositoryMock = new Mock<IOrderRepository>(MockBehavior.Strict);
            Mock<IOrderLogsRepository> orderLogsRepositoryMock = new Mock<IOrderLogsRepository>(MockBehavior.Strict);
            Mock<IInventoryRepository> inventoryRepositoryMock = new Mock<IInventoryRepository>(MockBehavior.Strict);
            Mock<IDiscountCalculator> discountCalculatorMock = new Mock<IDiscountCalculator>(MockBehavior.Strict);
            Mock<ILogger<CustomerOrderProcessor>> logger = new Mock<ILogger<CustomerOrderProcessor>>(MockBehavior.Strict);

            var quantity = 2;
            var unitPrice = 10;
            var discount = 20;
            var customerRegistrationDate = DateTime.Now.AddDays(-3);
            var order = new OrderEntity()
            {
                Id = 1,
                CustomerId = 1,
                Status = (int) OrderStatus.Pending,
                OrderItems = new List<OrderItemEntity>()
                    {
                        new OrderItemEntity()
                        {
                            ProductId = 1,
                            Quantity = quantity,
                            UnitPrice = unitPrice,
                        }
                    }
            };

            customerRepositoryMock.Setup(m => m.GetCustomerByIdAsync(1)).ReturnsAsync(new CustomerEntity() { Name = "user", IsVip = false, RegistrationDate = customerRegistrationDate });
            discountCalculatorMock.Setup(m => m.CalculateDiscountPercent(false, customerRegistrationDate.Year, quantity * unitPrice)).Returns(discount);
            orderRepositoryMock.Setup(m => m.GetPendingOrdersByCustomerId(1)).Returns(new List<OrderEntity>() { order });
            orderRepositoryMock.Setup(m => m.UpdateAsync(order)).ReturnsAsync(true);
            inventoryRepositoryMock.Setup(m => m.GetStockQuantityByProductIdAsync(1)).ReturnsAsync(10);
            inventoryRepositoryMock.Setup(m => m.UpdateStockQuantityByProductIdAsync(1, -2)).ReturnsAsync(true);
            orderLogsRepositoryMock.Setup(m => m.CreateAsync(order.Id, It.IsAny<DateTime>(), It.IsAny<string>())).ReturnsAsync(true);

            var sut = new CustomerOrderProcessor(
                customerRepositoryMock.Object,
                orderRepositoryMock.Object,
                orderLogsRepositoryMock.Object,
                inventoryRepositoryMock.Object,
                discountCalculatorMock.Object,
                logger.Object);

            // act
            var processedOrders = await sut.ProcessCustomerOrdersAsync(1);

            // assert
            processedOrders.Count.Should().Be(1);
            customerRepositoryMock.Verify(m => m.GetCustomerByIdAsync(1), Times.Exactly(1));
            orderRepositoryMock.Verify(m => m.GetPendingOrdersByCustomerId(1), Times.Exactly(1));
            orderRepositoryMock.Verify(m => m.UpdateAsync(order), Times.Exactly(2));
            inventoryRepositoryMock.Verify(m => m.GetStockQuantityByProductIdAsync(1), Times.Exactly(1));
            inventoryRepositoryMock.Verify(m => m.UpdateStockQuantityByProductIdAsync(1, -2), Times.Exactly(1));
            orderLogsRepositoryMock.Verify(m => m.CreateAsync(order.Id, It.IsAny<DateTime>(), It.IsAny<string>()), Times.Exactly(1));
        }
    }
}
