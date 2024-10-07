using CrochetBusinessAPI.Data;
using CrochetBusinessAPI.Models;
using CrochetBusinessAPI.Repositories;
using CrochetBusinessAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace CrochetBusinessAPI.Tests.Services
{
    public class FinishedProductServiceTests
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
            context.FinishedProducts.AddRange(
                new FinishedProduct
                {
                    Name = "Scarf",
                    TimeToMake = TimeSpan.FromHours(2),
                    TotalCostToMake = 15.0m,
                    SalePrice = 25.0m,
                    NumberInStock = 5
                },
                new FinishedProduct
                {
                    Name = "Hat",
                    TimeToMake = TimeSpan.FromHours(1.5),
                    TotalCostToMake = 10.0m,
                    SalePrice = 20.0m,
                    NumberInStock = 10
                }
            );
            context.SaveChanges();
        }

        [Fact]
        public async Task GetAllProductNamesAsync_ShouldReturnAllUniqueProductNames()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var finishedProductRepository = new FinishedProductRepository(context);
            var service = new FinishedProductService(finishedProductRepository);

            //Act
            var productNames = await service.GetAllProductNamesAsync();

            //Assert
            Assert.NotNull(productNames);
            Assert.Equal(2, productNames.Count); //Should return 2 unique product names
            Assert.Contains("Scarf", productNames);
            Assert.Contains("Hat", productNames);
        }

        [Fact]
        public async Task GetProductsSortedByTimeToMakeAsync_ShouldReturnProductsSortedByTime()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var finishedProductRepository = new FinishedProductRepository(context);
            var service = new FinishedProductService(finishedProductRepository);

            //Act
            var sortedProducts = await service.GetProductsSortedByTimeToMakeAsync();

            //Assert
            Assert.NotNull(sortedProducts);
            Assert.Equal(2, sortedProducts.Count);
            Assert.Equal("Hat", sortedProducts[0].Name); //Hat should come before Scarf because it takes less time
            Assert.Equal("Scarf", sortedProducts[1].Name);
        }

        [Fact]
        public async Task GetProductsSortedByCostToMakeAsync_ShouldReturnProductsSortedByCost()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var finishedProductRepository = new FinishedProductRepository(context);
            var service = new FinishedProductService(finishedProductRepository);

            //Act
            var sortedProducts = await service.GetProductsSortedByCostToMakeAsync();

            //Assert
            Assert.NotNull(sortedProducts);
            Assert.Equal(2, sortedProducts.Count);
            Assert.Equal("Hat", sortedProducts[0].Name); //Hat should come before Scarf because it costs less to make
            Assert.Equal("Scarf", sortedProducts[1].Name);
        }

        [Fact]
        public async Task GetProductsSortedBySalePriceAsync_ShouldReturnProductsSortedByPrice()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var finishedProductRepository = new FinishedProductRepository(context);
            var service = new FinishedProductService(finishedProductRepository);

            //Act
            var sortedProducts = await service.GetProductsSortedBySalePriceAsync();

            //Assert
            Assert.NotNull(sortedProducts);
            Assert.Equal(2, sortedProducts.Count);
            Assert.Equal("Hat", sortedProducts[0].Name); //Hat should come before Scarf because it has a lower sale price
            Assert.Equal("Scarf", sortedProducts[1].Name);
        }

        [Fact]
        public async Task GetProductsSortedByNumberInStockAsync_ShouldReturnProductsSortedByStock()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var finishedProductRepository = new FinishedProductRepository(context);
            var service = new FinishedProductService(finishedProductRepository);

            //Act
            var sortedProducts = await service.GetProductsSortedByNumberInStockAsync();

            //Assert
            Assert.NotNull(sortedProducts);
            Assert.Equal(2, sortedProducts.Count);
            Assert.Equal("Scarf", sortedProducts[0].Name); //Scarf should come before hat because it has less in stock
            Assert.Equal("Hat", sortedProducts[1].Name);
        }

        [Fact]
        public async Task GetProductByNameAsync_ShouldReturnCorrectProduct()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var finishedProductRepository = new FinishedProductRepository(context);
            var service = new FinishedProductService(finishedProductRepository);

            //Act
            var product = await service.GetProductByNameAsync("Hat");

            //Assert
            Assert.NotNull(product);
            Assert.Equal("Hat", product?.Name);
        }

        [Fact]
        public async Task GetProductByNameAsync_ShouldReturnNullForNonExistentProduct()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var finishedProductRepository = new FinishedProductRepository(context);
            var service = new FinishedProductService(finishedProductRepository);

            //Act
            var product = await service.GetProductByNameAsync("NonExistentProduct");

            //Assert
            Assert.Null(product);
        }

        [Fact]
        public async Task DeleteProductByNameAsync_ShouldDeleteAndReturnDeletedProduct()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var finishedProductRepository = new FinishedProductRepository(context);
            var service = new FinishedProductService(finishedProductRepository);

            //Act
            var deletedProduct = await service.DeleteProductByNameAsync("Scarf");

            //Assert
            Assert.NotNull(deletedProduct);
            Assert.Equal("Scarf", deletedProduct?.Name);

            var remainingProducts = await context.FinishedProducts.ToListAsync();
            Assert.Single(remainingProducts); //Only 1 product should remain in the database
        }

        [Fact]
        public async Task CheckFinishedProductExistsAsync_ShouldReturnTrueIfExists()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var finishedProductRepository = new FinishedProductRepository(context);
            var service = new FinishedProductService(finishedProductRepository);

            //Act
            var exists = await service.CheckFinishedProductExistsAsync("Scarf");

            //Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task CheckFinishedProductExistsAsync_ShouldReturnFalseIfNotExists()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var finishedProductRepository = new FinishedProductRepository(context);
            var service = new FinishedProductService(finishedProductRepository);

            //Act
            var exists = await service.CheckFinishedProductExistsAsync("NonExistentProduct");

            //Assert
            Assert.False(exists);
        }

        [Fact]
        public async Task UpdateFinishedProductQuantityAsync_ShouldUpdateQuantity()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var finishedProductRepository = new FinishedProductRepository(context);
            var service = new FinishedProductService(finishedProductRepository);

            //Act
            var updateResult = await service.UpdateFinishedProductQuantityAsync("Scarf", 20);

            //Assert
            Assert.True(updateResult);
            var updatedProduct = await context.FinishedProducts.FirstOrDefaultAsync(p => p.Name == "Scarf");
            Assert.NotNull(updatedProduct);
            Assert.Equal(20, updatedProduct?.NumberInStock); //Quantity should be updated to 20
        }
    }
}