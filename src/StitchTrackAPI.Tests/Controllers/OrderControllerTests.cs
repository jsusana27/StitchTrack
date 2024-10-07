using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrochetBusinessAPI.Controllers;
using CrochetBusinessAPI.Data;
using CrochetBusinessAPI.Models;
using CrochetBusinessAPI.Repositories;
using CrochetBusinessAPI.Services;

namespace CrochetBusinessAPI.Tests.Controllers
{
    public class OrderControllerTests
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
            //Seed Customer data
            context.Customers.AddRange(
                new Customer 
                { 
                    CustomerID = 1, 
                    Name = "John Doe", 
                    PhoneNumber = "0123456789", 
                    EmailAddress = "john@example.com" 
                },
                new Customer 
                { 
                    CustomerID = 2, 
                    Name = "Jane Doe", 
                    PhoneNumber = "9876543210", 
                    EmailAddress = "jane@example.com" 
                }
            );

            //Seed FinishedProduct data
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
                    SalePrice = 30.0m, 
                    NumberInStock = 10 
                }
            );

            //Seed Order data
            context.Orders.AddRange(
                new Order 
                { 
                    OrderID = 1, 
                    CustomerID = 1, 
                    OrderDate = new DateTime(2023, 9, 16), 
                    FormOfPayment = "Credit", 
                    TotalPrice = 50.0m 
                }
            );

            //Seed OrderProduct data
            context.OrderProducts.AddRange(
                new OrderProduct 
                { 
                    OrderID = 1, 
                    FinishedProductsID = 1, 
                    Quantity = 2 
                },
                new OrderProduct 
                { 
                    OrderID = 1, 
                    FinishedProductsID = 2, 
                    Quantity = 1 
                }
            );

            context.SaveChanges();
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllOrders()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var orderRepository = new OrderRepository(context);
            var orderProductRepository = new OrderProductRepository(context);
            var customerPurchaseRepository = new CustomerPurchaseRepository(context);
            var customerService = new CustomerService(new CustomerRepository(context));
            var finishedProductService = new FinishedProductService(new FinishedProductRepository(context));
            var service = new OrderService(orderRepository, orderProductRepository, customerPurchaseRepository, customerService, finishedProductService);
            var controller = new OrderController(service);

            //Act
            var result = await controller.GetAll();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var orders = Assert.IsType<List<Order>>(okResult.Value);
            Assert.Single(orders); //There should be 1 order in the seeded data
        }

        [Fact]
        public async Task DeleteOrderByCustomerAndDate_ShouldDeleteOrderSuccessfully()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var orderRepository = new OrderRepository(context);
            var orderProductRepository = new OrderProductRepository(context);
            var customerPurchaseRepository = new CustomerPurchaseRepository(context);
            var customerService = new CustomerService(new CustomerRepository(context));
            var finishedProductService = new FinishedProductService(new FinishedProductRepository(context));
            var service = new OrderService(orderRepository, orderProductRepository, customerPurchaseRepository, customerService, finishedProductService);
            var controller = new OrderController(service);

            //Act
            var result = await controller.DeleteOrderByCustomerAndDate("John Doe", "2023-09-16");

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Order for customer 'John Doe' on date '2023-09-16' deleted successfully.", okResult.Value);

            //Verify deletion
            var order = await context.Orders.FindAsync(1);
            Assert.Null(order); //Order should be deleted
        }

        [Fact]
        public async Task CreateOrder_ShouldCreateOrderSuccessfully()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var orderRepository = new OrderRepository(context);
            var orderProductRepository = new OrderProductRepository(context);
            var customerPurchaseRepository = new CustomerPurchaseRepository(context);
            var customerService = new CustomerService(new CustomerRepository(context));
            var finishedProductService = new FinishedProductService(new FinishedProductRepository(context));
            var service = new OrderService(orderRepository, orderProductRepository, customerPurchaseRepository, customerService, finishedProductService);
            var controller = new OrderController(service);

            var productNames = new List<string> { "Hat", "Scarf" };
            var quantities = new List<int> { 1, 2 };

            //Act
            var result = await controller.CreateOrder("Jane Doe", "2023-09-20", "Cash", productNames, quantities);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Order created successfully!", okResult.Value);

            //Verify the new order was created
            var createdOrder = await context.Orders.Include(o => o.OrderProducts).FirstOrDefaultAsync(o => o.Customer.Name == "Jane Doe");
            Assert.NotNull(createdOrder);
            Assert.Equal(80.0m, createdOrder.TotalPrice); //Hat ($20 * 1) + Scarf ($30 * 2) = $80 total
            Assert.Equal(2, createdOrder.OrderProducts.Count); //Should have 2 products
        }

        [Fact]
        public async Task DeleteOrderByCustomerAndDate_ShouldReturnNotFoundForNonExistentOrder()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var orderRepository = new OrderRepository(context);
            var orderProductRepository = new OrderProductRepository(context);
            var customerPurchaseRepository = new CustomerPurchaseRepository(context);
            var customerService = new CustomerService(new CustomerRepository(context));
            var finishedProductService = new FinishedProductService(new FinishedProductRepository(context));
            var service = new OrderService(orderRepository, orderProductRepository, customerPurchaseRepository, customerService, finishedProductService);
            var controller = new OrderController(service);

            //Act
            var result = await controller.DeleteOrderByCustomerAndDate("Nonexistent Customer", "2023-09-16");

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No order found for customer 'Nonexistent Customer' on date '2023-09-16'.", notFoundResult.Value);
        }
    }
}