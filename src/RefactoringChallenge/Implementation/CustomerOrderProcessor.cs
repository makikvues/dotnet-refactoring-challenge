using Microsoft.Extensions.Logging;
using RefactoringChallenge.Abstractions;
using RefactoringChallenge.Abstractions.Models.Entities;
using RefactoringChallenge.Abstractions.Models.Enums;
using RefactoringChallenge.Abstractions.Repository;

namespace RefactoringChallenge.Implementation
{
    /// <summary>
    /// Processes order for a customer
    /// </summary>
    /// <param name="customerRepository">customer repository</param>
    /// <param name="orderRepository">order repository</param>
    /// <param name="orderLogsRepository">order logs repository</param>
    /// <param name="inventoryRepository">inventory repository</param>
    /// <param name="discountCalculator">discount calculator</param>
    /// <param name="logger">logger</param>
    public class CustomerOrderProcessor(
        ICustomerRepository customerRepository,
        IOrderRepository orderRepository,
        IOrderLogsRepository orderLogsRepository,
        IInventoryRepository inventoryRepository,
        IDiscountCalculator discountCalculator,
        ILogger<CustomerOrderProcessor> logger) : ICustomerOrderProcessor
    {
        /// <summary>
        /// Process all new orders for specific customer. Update discount and status.
        /// </summary>
        /// <param name="customerId">Customer ID</param>
        /// <returns>List of processed orders</returns>
        public async Task<IList<OrderEntity>> ProcessCustomerOrdersAsync(int customerId)
        {
            if (customerId <= 0)
                throw new ArgumentException("ID zákazníka musí být kladné číslo.", nameof(customerId));       

            CustomerEntity customer = await customerRepository.GetCustomerByIdAsync(customerId);

            if (customer == null)
                throw new Exception($"Zákazník s ID {customerId} nebyl nalezen.");

            var processedOrders = new List<OrderEntity>();
            var pendingOrders = orderRepository.GetPendingOrdersByCustomerId(customerId).ToList();

            foreach (var order in pendingOrders)
            {
                await UpdateOrderDiscountAsync(order, customer);

                // probably should be in a transaction if there is high concurrency,
                // so we are guaranteed that we first read all the available products from inventory and then
                // the count is consistent once we are creating the order - e.g. updating stock quantities
                // -- transaction start -- 
                bool isAllProductsAvailable = await CheckAllProductsAvailableAsync(order.OrderItems);
                var logMessage = "Order on hold. Some items are not on stock.";
                order.Status = (int) OrderStatus.OnHold;

                if (isAllProductsAvailable)
                {
                    logMessage = $"Order completed with {order.DiscountPercent}% discount. Total price: {order.TotalAmount}";
                    order.Status = (int) OrderStatus.Ready;

                    foreach (var item in order.OrderItems)
                    {
                        await inventoryRepository.UpdateStockQuantityByProductIdAsync(item.ProductId, -item.Quantity);
                    }
                }
                // -- transaction end --

                await orderRepository.UpdateAsync(order);
                await orderLogsRepository.CreateAsync(order.Id, DateTime.Now, logMessage);

                processedOrders.Add(order);
            }

            return processedOrders;
        }

        private async Task UpdateOrderDiscountAsync(OrderEntity order, CustomerEntity customer)
        {
            decimal totalAmount = 0;
            foreach (var item in order.OrderItems)
            {
                var subtotal = item.Quantity * item.UnitPrice;
                totalAmount += subtotal;
            }

            decimal discountPercent = discountCalculator.CalculateDiscountPercent(customer.IsVip, customer.RegistrationDate.Year, totalAmount);
            decimal discountAmount = totalAmount * (discountPercent / 100);
            decimal finalAmount = totalAmount - discountAmount;

            order.DiscountPercent = discountPercent;
            order.DiscountAmount = discountAmount;
            order.TotalAmount = finalAmount;
            order.Status = (int) OrderStatus.Processed;

            await orderRepository.UpdateAsync(order);
        }

        private async Task<bool> CheckAllProductsAvailableAsync(ICollection<OrderItemEntity> orderItems)
        {
            // we could also load all product items at once, but I don't think we'll have a lot of product items typically,
            // I could ask about this though and optimize it a bit more, if needed
            bool isAllProductsAvailable = true;
            foreach (var item in orderItems)
            {
                var stockQuantity = await inventoryRepository.GetStockQuantityByProductIdAsync(item.ProductId);

                if (stockQuantity == null || stockQuantity < item.Quantity)
                {
                    isAllProductsAvailable = false;
                    break;
                }
            }

            return isAllProductsAvailable;
        }
    }
}
