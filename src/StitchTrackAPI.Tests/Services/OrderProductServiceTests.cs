using CrochetBusinessAPI.Data;
using CrochetBusinessAPI.Models; 
using CrochetBusinessAPI.Repositories;
using CrochetBusinessAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace CrochetBusinessAPI.Tests.Services
{
    public class OrderProductServiceTests
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
            //Seed Orders
            context.Orders.AddRange(
                new Order { OrderID = 1, CustomerID = 1, OrderDate = DateTime.Now, FormOfPayment = "Zelle" },
                new Order { OrderID = 2, CustomerID = 1, OrderDate = DateTime.Now, FormOfPayment = "Cash" }
            );

            //Seed FinishedProducts
            context.FinishedProducts.AddRange(
                new FinishedProduct
                {
                    FinishedProductsID = 1,
                    Name = "Hat",
                    SalePrice = 20.0m,
                    NumberInStock = 10
                },
                new FinishedProduct
                {
                    FinishedProductsID = 2,
                    Name = "Scarf",
                    SalePrice = 25.0m,
                    NumberInStock = 5
                }
            );

            //Seed OrderProducts with correct OrderID and FinishedProductsID
            context.OrderProducts.AddRange(
                new OrderProduct
                {
                    OrderID = 1,
                    FinishedProductsID = 1,
                    Quantity = 2 //2 Hats sold
                },
                new OrderProduct
                {
                    OrderID = 1,
                    FinishedProductsID = 2,
                    Quantity = 1 //1 Scarf sold
                },
                new OrderProduct
                {
                    OrderID = 2,
                    FinishedProductsID = 1,
                    Quantity = 3 //3 more Hats sold
                }
            );

            context.SaveChanges();
        }

        [Fact]
        public async Task GetOrderProductsByOrderIdAsync_ShouldReturnCorrectOrderProducts()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var orderProductRepository = new OrderProductRepository(context);
            var finishedProductRepository = new FinishedProductRepository(context);
            var service = new OrderProductService(orderProductRepository, finishedProductRepository);

            //Act
            var orderProducts = await service.GetOrderProductsByOrderIdAsync(1); //Get order products for OrderID = 1

            //Assert
            Assert.NotNull(orderProducts);
            Assert.Equal(2, orderProducts.Count); //Should have 2 order products for OrderID = 1
        }

        [Fact]
        public async Task GetOrderProductsByFinishedProductIdAsync_ShouldReturnCorrectOrderProducts()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var orderProductRepository = new OrderProductRepository(context);
            var finishedProductRepository = new FinishedProductRepository(context);
            var service = new OrderProductService(orderProductRepository, finishedProductRepository);

            //Act
            var orderProducts = await service.GetOrderProductsByFinishedProductIdAsync(1); //Get order products for FinishedProductID = 1 (Hat)

            //Assert
            Assert.NotNull(orderProducts);
            Assert.Equal(2, orderProducts.Count); //2 entries should be found for Hats
        }

        [Fact]
        public async Task GetTotalQuantityForFinishedProductAsync_ShouldReturnCorrectTotalQuantity()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var orderProductRepository = new OrderProductRepository(context);
            var finishedProductRepository = new FinishedProductRepository(context);
            var service = new OrderProductService(orderProductRepository, finishedProductRepository);

            //Act
            var totalQuantity = await service.GetTotalQuantityForFinishedProductAsync(1); //Get total quantity for Hat (FinishedProductID = 1)

            //Assert
            Assert.Equal(5, totalQuantity); //Total 5 Hats sold
        }

        [Fact]
        public async Task GetTotalRevenueForFinishedProductAsync_ShouldReturnCorrectTotalRevenue()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var orderProductRepository = new OrderProductRepository(context);
            var finishedProductRepository = new FinishedProductRepository(context);
            var service = new OrderProductService(orderProductRepository, finishedProductRepository);

            //Act
            var totalRevenue = await service.GetTotalRevenueForFinishedProductAsync(1); //Get total revenue for Hat

            //Assert
            Assert.Equal(100.0m, totalRevenue); //5 Hats * $20 each = $100 total revenue
        }

        [Fact]
        public async Task GetSaleStatsForProductAsync_ShouldReturnCorrectStatistics()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var orderProductRepository = new OrderProductRepository(context);
            var finishedProductRepository = new FinishedProductRepository(context);
            var service = new OrderProductService(orderProductRepository, finishedProductRepository);

            //Act
            var stats = await service.GetSaleStatsForProductAsync("Hat");

            //Assert
            Assert.NotNull(stats);
            Assert.Equal(5, (int)stats.GetType().GetProperty("totalQuantity")?.GetValue(stats)!); //5 Hats sold
            Assert.Equal(100.0m, (decimal)stats.GetType().GetProperty("totalRevenue")?.GetValue(stats)!); //$100 revenue
        }
    }
}
