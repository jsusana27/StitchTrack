using Microsoft.EntityFrameworkCore;
using CrochetBusinessAPI.Models;
using CrochetBusinessAPI.Data;
using CrochetBusinessAPI.Repositories;

namespace CrochetBusinessAPI.Tests.Repositories
{
    public class OrderProductRepositoryTests
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
            var product1 = new FinishedProduct
            {
                FinishedProductsID = 1,
                Name = "Hat",
                TotalCostToMake = 8.99m,
                SalePrice = 15.00m,
                NumberInStock = 10
            };

            var product2 = new FinishedProduct
            {
                FinishedProductsID = 2,
                Name = "Scarf",
                TotalCostToMake = 5.99m,
                SalePrice = 10.00m,
                NumberInStock = 5
            };

            var order1 = new Order
            {
                OrderID = 1,
                CustomerID = 1,
                OrderDate = DateTime.Now.AddDays(-1)
            };

            var order2 = new Order
            {
                OrderID = 2,
                CustomerID = 2,
                OrderDate = DateTime.Now
            };

            context.FinishedProducts.AddRange(product1, product2);
            context.Orders.AddRange(order1, order2);

            context.OrderProducts.AddRange(
                new OrderProduct
                {
                    OrderID = 1,
                    FinishedProductsID = 1,
                    Quantity = 2,
                    FinishedProduct = product1
                },
                new OrderProduct
                {
                    OrderID = 2,
                    FinishedProductsID = 2,
                    Quantity = 3,
                    FinishedProduct = product2
                }
            );

            context.SaveChanges();
        }

        [Fact]
        public async Task GetOrderProductsByOrderIdAsync_ShouldReturnCorrectOrderProducts()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new OrderProductRepository(context);

            //Act
            var orderProducts = await repository.GetOrderProductsByOrderIdAsync(1);

            //Assert
            Assert.NotNull(orderProducts);
            Assert.Single(orderProducts); //Only one OrderProduct should be related to OrderID 1
            Assert.Equal(2, orderProducts.First().Quantity); //Verify the quantity
        }

        [Fact]
        public async Task GetOrderProductsByFinishedProductIdAsync_ShouldReturnCorrectOrderProducts()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new OrderProductRepository(context);

            //Act
            var orderProducts = await repository.GetOrderProductsByFinishedProductIdAsync(2);

            //Assert
            Assert.NotNull(orderProducts);
            Assert.Single(orderProducts); //Only one OrderProduct should be related to FinishedProductID 2
            Assert.Equal(3, orderProducts.First().Quantity); //Verify the quantity
        }

        [Fact]
        public async Task GetTotalQuantitySoldAsync_ShouldReturnTotalQuantitySold()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new OrderProductRepository(context);

            //Act
            var totalQuantity = await repository.GetTotalQuantitySoldAsync(1);

            //Assert
            Assert.Equal(2, totalQuantity); //Should return the total quantity sold for FinishedProductID 1
        }

        [Fact]
        public async Task GetTotalRevenueForFinishedProductAsync_ShouldReturnTotalRevenue()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new OrderProductRepository(context);

            //Act
            var totalRevenue = await repository.GetTotalRevenueForFinishedProductAsync(1);

            //Assert
            Assert.Equal(30.00m, totalRevenue); //(2 * 15.00m) = 30.00m total revenue for FinishedProductID 1
        }

        [Fact]
        public async Task AddOrderProductAsync_ShouldAddOrderProductToDatabase()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new OrderProductRepository(context);

            var newOrderProduct = new OrderProduct
            {
                OrderID = 2,
                FinishedProductsID = 1,
                Quantity = 4
            };

            //Act
            var addedOrderProduct = await repository.AddOrderProductAsync(newOrderProduct);

            //Assert
            var orderProducts = await context.OrderProducts.ToListAsync();
            Assert.Equal(3, orderProducts.Count); //2 initial + 1 new OrderProduct
            Assert.NotNull(addedOrderProduct);
            Assert.Equal(4, addedOrderProduct.Quantity);
        }
    }
}