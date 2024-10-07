using Microsoft.AspNetCore.Mvc;     
using Microsoft.EntityFrameworkCore;  
using CrochetBusinessAPI.Data;       
using CrochetBusinessAPI.Models;       
using CrochetBusinessAPI.Repositories;  
using CrochetBusinessAPI.Services;     
using CrochetBusinessAPI.Controllers;   
using Xunit.Abstractions; // Import the xUnit Test Output Helper namespace

namespace CrochetBusinessAPI.Tests.Controllers
{
    public class OrderProductControllerTests
    {
        private readonly ITestOutputHelper _output;

        public OrderProductControllerTests(ITestOutputHelper output)
        {
            _output = output; // Assign the output helper
        }
        
        //Helper method to create a new in-memory database context for each test
        private CrochetDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<CrochetDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) //Use a unique database name for each test
                .Options;

            var context = new CrochetDbContext(options);
            SeedData(context); //Seed initial data
            return context;
        }

        //Seed the in-memory database with sample data for testing
        private void SeedData(CrochetDbContext context)
        {
            context.FinishedProducts.AddRange(
                new FinishedProduct
                {
                    FinishedProductsID = 1,
                    Name = "Hat",
                    SalePrice = 20.00m,
                    NumberInStock = 15,
                    TimeToMake = new TimeSpan(1, 0, 0), //1 hour to make
                    TotalCostToMake = 10.0m
                },
                new FinishedProduct
                {
                    FinishedProductsID = 2,
                    Name = "Scarf",
                    SalePrice = 30.00m,
                    NumberInStock = 10,
                    TimeToMake = new TimeSpan(2, 0, 0), //2 hours to make
                    TotalCostToMake = 15.0m
                }
            );

            context.OrderProducts.AddRange(
                new OrderProduct
                {
                    OrderProductID = 1,
                    OrderID = 1,
                    FinishedProductsID = 1, // Hat
                    Quantity = 5
                },
                new OrderProduct
                {
                    OrderProductID = 2,
                    OrderID = 2,
                    FinishedProductsID = 2, // Scarf
                    Quantity = 3
                }
            );

            context.SaveChanges();

            // Debugging: Print out the seed data
            var products = context.FinishedProducts.ToList();
            var orders = context.OrderProducts.ToList();

            // Use the output helper to log the data
            _output.WriteLine("Seeded FinishedProducts:");
            foreach (var product in context.FinishedProducts)
            {
                _output.WriteLine($"ID: {product.FinishedProductsID}, Name: {product.Name}, Sale Price: {product.SalePrice}");
            }

            _output.WriteLine("Seeded OrderProducts:");
            foreach (var orderProduct in context.OrderProducts)
            {
                _output.WriteLine($"OrderProductID: {orderProduct.OrderProductID}, FinishedProductsID: {orderProduct.FinishedProductsID}, Quantity: {orderProduct.Quantity}");
            }
        }

        [Fact]
        public async Task GetOrderProductsByOrderId_ShouldReturnOrderProductsForGivenOrderId()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var orderProductRepository = new OrderProductRepository(context);
            var finishedProductRepository = new FinishedProductRepository(context);
            var service = new OrderProductService(orderProductRepository, finishedProductRepository);
            var controller = new OrderProductController(service);

            //Act
            var result = await controller.GetOrderProductsByOrderId(1); //Get OrderProducts for OrderID = 1

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result); //Assert that the result is of type OkObjectResult
            var orderProducts = Assert.IsType<List<OrderProduct>>(okResult.Value); //Assert that the value is a list of OrderProducts
            Assert.Single(orderProducts); //Only 1 OrderProduct should be returned for OrderID = 1
        }

        /*[Fact]
        public async Task GetOrderProductsByFinishedProductId_ShouldReturnCorrectOrderProducts()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var orderProductRepository = new OrderProductRepository(context);
            var finishedProductRepository = new FinishedProductRepository(context);
            var service = new OrderProductService(orderProductRepository, finishedProductRepository);
            var controller = new OrderProductController(service);

            //Act
            var result = await controller.GetOrderProductsByFinishedProductId(1); //Get OrderProducts for FinishedProductID = 1 (Hat)

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result); //Assert that the result is of type OkObjectResult
            var orderProducts = Assert.IsType<List<OrderProduct>>(okResult.Value); //Assert that the value is a list of OrderProducts
            Assert.Single(orderProducts); //1 order product should exist for FinishedProductID = 1
        }*/

        [Fact]
        public async Task GetTotalQuantityForFinishedProduct_ShouldReturnCorrectTotalQuantity()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var orderProductRepository = new OrderProductRepository(context);
            var finishedProductRepository = new FinishedProductRepository(context);
            var service = new OrderProductService(orderProductRepository, finishedProductRepository);
            var controller = new OrderProductController(service);

            //Act
            var result = await controller.GetTotalQuantityForFinishedProduct(1); //Get total quantity for FinishedProductID = 1 (Hat)

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result); //Assert that the result is of type OkObjectResult
            Assert.Equal(5, okResult.Value); //Total quantity for Hat should be 5
        }

        [Fact]
        public async Task GetSaleStatsForProduct_ShouldReturnCorrectStats()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var orderProductRepository = new OrderProductRepository(context);
            var finishedProductRepository = new FinishedProductRepository(context);
            var service = new OrderProductService(orderProductRepository, finishedProductRepository);
            var controller = new OrderProductController(service);

            // Act
            var result = await controller.GetSaleStatsForProduct("Hat");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);

            // Use reflection to check the properties of the anonymous type
            var stats = okResult.Value;
            Assert.NotNull(stats);

            var totalQuantityProperty = stats.GetType().GetProperty("totalQuantity");
            var totalRevenueProperty = stats.GetType().GetProperty("totalRevenue");

            // Ensure the properties exist and validate their values
            Assert.NotNull(totalQuantityProperty);
            Assert.NotNull(totalRevenueProperty);

            var totalQuantity = (int?)totalQuantityProperty.GetValue(stats) ?? 0;
            var totalRevenue = (decimal?)totalRevenueProperty.GetValue(stats) ?? 0.0m;

            Assert.Equal(5, totalQuantity);
            Assert.Equal(100.0m, totalRevenue);
        }

        [Fact]
        public async Task GetSaleStatsForProduct_ShouldReturnNotFoundForInvalidProductName()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var orderProductRepository = new OrderProductRepository(context);
            var finishedProductRepository = new FinishedProductRepository(context);
            var service = new OrderProductService(orderProductRepository, finishedProductRepository);
            var controller = new OrderProductController(service);

            // Act
            var result = await controller.GetSaleStatsForProduct("NonExistentProduct");

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Product with name 'NonExistentProduct' not found.", notFoundResult.Value);
        }
    }
}