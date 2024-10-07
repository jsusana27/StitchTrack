using CrochetBusinessAPI.Data;                  
using CrochetBusinessAPI.Models;               
using CrochetBusinessAPI.Repositories;         
using CrochetBusinessAPI.Services;             
using Microsoft.EntityFrameworkCore;           

namespace CrochetBusinessAPI.Tests.Services
{
    public class FinishedProductMaterialServiceTests
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
            context.FinishedProductMaterials.AddRange(
                new FinishedProductMaterial
                {
                    MaterialType = "Yarn",
                    MaterialID = 1,
                    FinishedProductsID = 1,
                    QuantityUsed = 5
                },
                new FinishedProductMaterial
                {
                    MaterialType = "Safety Eyes",
                    MaterialID = 2,
                    FinishedProductsID = 1,
                    QuantityUsed = 2
                }
            );

            context.FinishedProducts.Add(new FinishedProduct
            {
                Name = "Scarf",
                FinishedProductsID = 1,
                TimeToMake = TimeSpan.FromHours(3),
                TotalCostToMake = 10.0m,
                SalePrice = 25.0m,
                NumberInStock = 5
            });

            context.SaveChanges();
        }

        [Fact]
        public async Task GetFinishedProductsByMaterialAsync_ShouldReturnAllProductsUsingMaterial()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductMaterialRepository(context);
            var service = new FinishedProductMaterialService(repository);

            //Act
            var products = await service.GetFinishedProductsByMaterialAsync("Yarn", 1);

            //Assert
            Assert.NotNull(products);
            Assert.Single(products);
            Assert.Equal("Scarf", products.First().Name); //Product "Scarf" should be returned for Yarn with ID 1
        }

        [Fact]
        public async Task GetMaterialsByFinishedProductIdAsync_ShouldReturnAllMaterialsForProduct()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductMaterialRepository(context);
            var service = new FinishedProductMaterialService(repository);

            //Act
            var materials = await service.GetMaterialsByFinishedProductIdAsync(1);

            //Assert
            Assert.NotNull(materials);
            Assert.Equal(2, materials.Count); //Should return 2 materials linked to FinishedProductId 1
        }

        [Fact]
        public async Task CanMakeProductAsync_ShouldReturnTrue_WhenEnoughMaterialIsAvailable()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductMaterialRepository(context);
            var service = new FinishedProductMaterialService(repository);

            //Act
            var result = await service.CanMakeProductAsync(1, 4.0m); //Enough Yarn quantity

            //Assert
            Assert.True(result); //Should return true as we have 5 units of Yarn
        }

        [Fact]
        public async Task CanMakeProductAsync_ShouldReturnFalse_WhenNotEnoughMaterialIsAvailable()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductMaterialRepository(context);
            var service = new FinishedProductMaterialService(repository);

            //Act
            var result = await service.CanMakeProductAsync(1, 10.0m); //Insufficient Yarn quantity

            //Assert
            Assert.False(result); //Should return false as required quantity exceeds available
        }

        [Fact]
        public async Task UpdateYarnQuantityAsync_ShouldUpdateQuantitySuccessfully()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductMaterialRepository(context);
            var service = new FinishedProductMaterialService(repository);

            //Act
            var updateResult = await service.UpdateYarnQuantityAsync(1, 1, 10.0m); //Update quantity of Yarn

            //Assert
            Assert.True(updateResult);

            //Instead of using FindAsync, use FirstOrDefaultAsync with a conditional query
            var material = await context.FinishedProductMaterials
                                        .FirstOrDefaultAsync(m => m.FinishedProductsID == 1 && m.MaterialID == 1);

            Assert.NotNull(material); //Ensure the material was found
            Assert.Equal(10.0m, material?.QuantityUsed); //Quantity should be updated to 10.0
        }

        [Fact]
        public async Task DeleteYarnMaterialAsync_ShouldRemoveMaterialSuccessfully()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductMaterialRepository(context);
            var service = new FinishedProductMaterialService(repository);

            //Act
            var deleteResult = await service.DeleteYarnMaterialAsync(1, 1); //Delete Yarn material

            //Assert
            Assert.True(deleteResult);
            var materials = await context.FinishedProductMaterials.ToListAsync();
            Assert.Single(materials); //Only one material should remain after deletion
        }

        [Fact]
        public async Task GetDetailedMaterialsByProductNameAsync_ShouldReturnDetails_WhenProductNameIsValid()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductMaterialRepository(context);
            var service = new FinishedProductMaterialService(repository);

            //Act
            var details = await service.GetDetailedMaterialsByProductNameAsync("Scarf");

            //Assert
            Assert.NotNull(details);
            Assert.Equal(2, details.Count); //Should return details for 2 materials
        }

        [Fact]
        public async Task GetDetailedMaterialsByProductNameAsync_ShouldReturnEmptyList_WhenProductNameIsInvalid()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductMaterialRepository(context);
            var service = new FinishedProductMaterialService(repository);

            //Act
            var details = await service.GetDetailedMaterialsByProductNameAsync("InvalidProduct");

            //Assert
            Assert.NotNull(details);
            Assert.Empty(details); //No details should be returned for invalid product name
        }
    }
}
