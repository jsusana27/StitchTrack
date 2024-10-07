using System.Globalization;
using CrochetBusinessAPI.Data;
using CrochetBusinessAPI.Models;
using CrochetBusinessAPI.Repositories;
using CrochetBusinessAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace CrochetBusinessAPI.Tests.Services
{
    public class OrderServiceTests
    {
        //Helper method to create a new in-memory database context for each test
        private static CrochetDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<CrochetDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) //Use a unique database name for each test
                .Options;

            var context = new CrochetDbContext(options);
            SeedData(context); //Seed initial data
            return context;
        }

        //Seed the in-memory database with sample data for testing
        private static void SeedData(CrochetDbContext context)
        {
            context.Customers.AddRange(
                new Customer
                {
                    CustomerID = 1,
                    Name = "John Doe",
                    PhoneNumber = "123-456-7890",
                    EmailAddress = "123 Test St"
                }
            );

            context.FinishedProducts.AddRange(
                new FinishedProduct
                {
                    FinishedProductsID = 1,
                    Name = "Hat",
                    SalePrice = 20.0m,
                    NumberInStock = 15
                },
                new FinishedProduct
                {
                    FinishedProductsID = 2,
                    Name = "Scarf",
                    SalePrice = 25.0m,
                    NumberInStock = 10
                }
            );

            context.Orders.AddRange(
                new Order
                {
                    OrderID = 1,
                    CustomerID = 1,
                    OrderDate = DateTime.ParseExact("2023-09-15", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    FormOfPayment = "Cash",
                    TotalPrice = 50.0m
                }
            );

            context.OrderProducts.AddRange(
                new OrderProduct
                {
                    OrderID = 1,
                    FinishedProductsID = 1,
                    Quantity = 2
                }
            );

            context.SaveChanges();
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldCreateOrderWithCorrectDetails()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var orderRepository = new OrderRepository(context);
            var orderProductRepository = new OrderProductRepository(context);
            var customerPurchaseRepository = new CustomerPurchaseRepository(context);
            var customerService = new CustomerService(new CustomerRepository(context));
            var finishedProductService = new FinishedProductService(new FinishedProductRepository(context));
            var service = new OrderService(orderRepository, orderProductRepository, customerPurchaseRepository, customerService, finishedProductService);

            var productNames = new List<string> { "Hat", "Scarf" };
            var quantities = new List<int> { 1, 1 };

            //Act
            var isOrderCreated = await service.CreateOrderAsync("Jane Doe", "2023-09-16", "Credit", productNames, quantities);

            //Assert
            Assert.True(isOrderCreated);

            var createdOrder = await context.Orders.Include(o => o.OrderProducts).FirstOrDefaultAsync(o => o.Customer.Name == "Jane Doe");
            
            Assert.NotNull(createdOrder); //Ensure the order was created

            //Now that we have confirmed createdOrder is not null, no need for null-conditional access
            Assert.Equal("Credit", createdOrder.FormOfPayment);
            Assert.Equal(45.0m, createdOrder.TotalPrice); //Hat ($20) + Scarf ($25)

            //Verify OrderProduct entries
            Assert.NotNull(createdOrder.OrderProducts); //Ensure OrderProducts are not null
            Assert.Equal(2, createdOrder.OrderProducts.Count); //Check the number of order products
            Assert.Contains(createdOrder.OrderProducts, op => op.FinishedProductsID == 1 && op.Quantity == 1);
            Assert.Contains(createdOrder.OrderProducts, op => op.FinishedProductsID == 2 && op.Quantity == 1);
        }

        [Fact]
        public async Task DeleteOrderByCustomerAndDateAsync_ShouldDeleteOrderAndAssociatedData()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var orderRepository = new OrderRepository(context);
            var orderProductRepository = new OrderProductRepository(context);
            var customerPurchaseRepository = new CustomerPurchaseRepository(context);
            var customerService = new CustomerService(new CustomerRepository(context));
            var finishedProductService = new FinishedProductService(new FinishedProductRepository(context));
            var service = new OrderService(orderRepository, orderProductRepository, customerPurchaseRepository, customerService, finishedProductService);

            //Act
            var isDeleted = await service.DeleteOrderByCustomerAndDateAsync("John Doe", DateTime.ParseExact("2023-09-15", "yyyy-MM-dd", CultureInfo.InvariantCulture));

            //Assert
            Assert.True(isDeleted);

            //Verify the order is deleted
            var deletedOrder = await context.Orders.FirstOrDefaultAsync(o => o.CustomerID == 1 && o.OrderDate == DateTime.ParseExact("2023-09-15", "yyyy-MM-dd", CultureInfo.InvariantCulture));
            Assert.Null(deletedOrder);

            //Verify related OrderProducts are deleted
            var relatedOrderProducts = await context.OrderProducts.Where(op => op.OrderID == 1).ToListAsync();
            Assert.Empty(relatedOrderProducts);

            //Verify CustomerPurchase entries are deleted
            var customerPurchases = await context.CustomerPurchases.Where(cp => cp.CustomerID == 1).ToListAsync();
            Assert.Empty(customerPurchases);
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldReturnFalseForInvalidOrderDate()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var orderRepository = new OrderRepository(context);
            var orderProductRepository = new OrderProductRepository(context);
            var customerPurchaseRepository = new CustomerPurchaseRepository(context);
            var customerService = new CustomerService(new CustomerRepository(context));
            var finishedProductService = new FinishedProductService(new FinishedProductRepository(context));
            var service = new OrderService(orderRepository, orderProductRepository, customerPurchaseRepository, customerService, finishedProductService);

            var productNames = new List<string> { "Hat" };
            var quantities = new List<int> { 2 };

            //Act
            var isOrderCreated = await service.CreateOrderAsync("John Doe", "InvalidDate", "Credit", productNames, quantities);

            //Assert
            Assert.False(isOrderCreated); //Invalid date should cause the method to return false
        }
    }
}