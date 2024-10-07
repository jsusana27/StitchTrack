using Microsoft.EntityFrameworkCore;
using CrochetBusinessAPI.Data;
using CrochetBusinessAPI.Models;
using CrochetBusinessAPI.Repositories;

namespace CrochetBusinessAPI.Tests.Repositories
{
    public class FinishedProductRepositoryTests
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
                Name = "Hat",
                TimeToMake = TimeSpan.FromHours(5),
                TotalCostToMake = 8.99m,
                SalePrice = 15.00m,
                NumberInStock = 10
            },
            new FinishedProduct
            {
                Name = "Scarf",
                TimeToMake = TimeSpan.FromHours(3),
                TotalCostToMake = 5.99m,
                SalePrice = 10.00m,
                NumberInStock = 5
            },
            new FinishedProduct
            {
                Name = "Gloves",
                TimeToMake = TimeSpan.FromHours(2),
                TotalCostToMake = 4.50m,
                SalePrice = 7.00m,
                NumberInStock = 2
            }
            );
            context.SaveChanges();
        }

        [Fact]
        public async Task GetAllProductNamesAsync_ShouldReturnAllProductNames()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductRepository(context);

            //Act
            var productNames = await repository.GetAllProductNamesAsync();

            //Assert
            Assert.NotNull(productNames);
            Assert.Equal(3, productNames.Count); //Should return 3 product names
        }

        [Fact]
        public async Task GetProductsSortedByTimeToMakeAsync_ShouldReturnProductsSortedByTimeToMake()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductRepository(context);

            //Act
            var products = await repository.GetProductsSortedByTimeToMakeAsync();

            //Assert
            Assert.NotNull(products);
            Assert.Equal(3, products.Count);
            Assert.Equal("Gloves", products[0].Name);  //Shortest time first
            Assert.Equal("Hat", products[2].Name);     //Longest time last
        }

        [Fact]
        public async Task GetProductsSortedByCostToMakeAsync_ShouldReturnProductsSortedByCost()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductRepository(context);

            //Act
            var products = await repository.GetProductsSortedByCostToMakeAsync();

            //Assert
            Assert.NotNull(products);
            Assert.Equal(3, products.Count);
            Assert.Equal(4.50m, products[0].TotalCostToMake);  //Lowest cost first
            Assert.Equal(8.99m, products[2].TotalCostToMake);  //Highest cost last
        }

        [Fact]
        public async Task GetProductsSortedBySalePriceAsync_ShouldReturnProductsSortedBySalePrice()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductRepository(context);

            //Act
            var products = await repository.GetProductsSortedBySalePriceAsync();

            //Assert
            Assert.NotNull(products);
            Assert.Equal(3, products.Count);
            Assert.Equal(7.00m, products[0].SalePrice);  //Lowest price first
            Assert.Equal(15.00m, products[2].SalePrice); //Highest price last
        }

        [Fact]
        public async Task GetProductsSortedByNumberInStockAsync_ShouldReturnProductsSortedByStock()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductRepository(context);

            //Act
            var products = await repository.GetProductsSortedByNumberInStockAsync();

            //Assert
            Assert.NotNull(products);
            Assert.Equal(3, products.Count);
            Assert.Equal(2, products[0].NumberInStock);  //Lowest stock first
            Assert.Equal(10, products[2].NumberInStock); //Highest stock last
        }

        [Fact]
        public async Task GetProductByNameAsync_ShouldReturnCorrectProduct()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductRepository(context);

            //Act
            var product = await repository.GetProductByNameAsync("Scarf");

            //Assert
            Assert.NotNull(product);
            Assert.Equal("Scarf", product?.Name);
        }

        [Fact]
        public async Task GetProductByNameAsync_ShouldReturnNullIfProductDoesNotExist()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductRepository(context);

            //Act
            var product = await repository.GetProductByNameAsync("NonExistentProduct");

            //Assert
            Assert.Null(product); //Should return null since the product doesn't exist
        }

        [Fact]
        public async Task CheckFinishedProductExistsAsync_ShouldReturnTrueIfExists()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductRepository(context);

            //Act
            var exists = await repository.CheckFinishedProductExistsAsync("Hat");

            //Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task CheckFinishedProductExistsAsync_ShouldReturnFalseIfNotExists()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductRepository(context);

            //Act
            var exists = await repository.CheckFinishedProductExistsAsync("NonExistentProduct");

            //Assert
            Assert.False(exists);
        }

        [Fact]
        public async Task UpdateFinishedProductQuantityAsync_ShouldUpdateQuantity()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductRepository(context);

            //Act
            var updated = await repository.UpdateFinishedProductQuantityAsync("Hat", 20);

            //Assert
            Assert.True(updated);
            var product = await repository.GetProductByNameAsync("Hat");
            Assert.Equal(20, product?.NumberInStock);  //Quantity should be updated
        }

        [Fact]
        public async Task DeleteProductByNameAsync_ShouldDeleteSpecificProduct()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductRepository(context);

            //Act
            var deletedProduct = await repository.DeleteProductByNameAsync("Gloves");

            //Assert
            Assert.NotNull(deletedProduct);
            Assert.Equal("Gloves", deletedProduct?.Name);
            var exists = await repository.CheckFinishedProductExistsAsync("Gloves");
            Assert.False(exists);  //Should no longer exist
        }

        [Fact]
        public async Task DeleteProductByNameAsync_ShouldReturnNullIfProductDoesNotExist()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductRepository(context);

            //Act
            var deletedProduct = await repository.DeleteProductByNameAsync("NonExistentProduct");

            //Assert
            Assert.Null(deletedProduct);  //Should return null since the product doesn't exist
        }
    }
}
