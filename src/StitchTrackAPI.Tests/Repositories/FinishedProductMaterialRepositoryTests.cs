using Microsoft.EntityFrameworkCore;
using CrochetBusinessAPI.Data;
using CrochetBusinessAPI.Models;
using CrochetBusinessAPI.Repositories;

namespace CrochetBusinessAPI.Tests.Repositories
{
    public class FinishedProductMaterialRepositoryTests
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
            //Seed sample data for Yarns, SafetyEyes, and Stuffings
            context.Yarns.AddRange(
                new Yarn
                {
                    YarnID = 1,
                    Brand = "BrandA",
                    FiberType = "Cotton",
                    FiberWeight = 3,
                    Color = "Red",
                    Price = 5.99m
                }
            );

            context.SafetyEyes.AddRange(
                new SafetyEye
                {
                    SafetyEyesID = 1,
                    SizeInMM = 10,
                    Color = "Black",
                    Shape = "Round",
                    Price = 0.5m
                }
            );

            context.Stuffings.AddRange(
                new Stuffing
                {
                    StuffingID = 1,
                    Brand = "Poly-Fil",
                    Type = "Polyester",
                    PricePerFivelbs = 15.0m
                }
            );

            //Seed Finished Products and related FinishedProductMaterials
            context.FinishedProducts.AddRange(
                new FinishedProduct
                {
                    FinishedProductsID = 1,
                    Name = "Hat",
                    TimeToMake = TimeSpan.FromHours(5),
                    TotalCostToMake = 8.99m,
                    SalePrice = 15.00m,
                    NumberInStock = 10
                }
            );

            context.FinishedProductMaterials.AddRange(
                new FinishedProductMaterial
                {
                    FinishedProductsID = 1,
                    MaterialID = 1,
                    MaterialType = "Yarn",
                    QuantityUsed = 5
                },
                new FinishedProductMaterial
                {
                    FinishedProductsID = 1,
                    MaterialID = 1,
                    MaterialType = "SafetyEyes",
                    QuantityUsed = 2
                }
            );

            context.SaveChanges();
        }

        [Fact]
        public async Task GetFinishedProductsByMaterialAsync_ShouldReturnProductsUsingMaterial()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductMaterialRepository(context);

            //Act
            var finishedProducts = await repository.GetFinishedProductsByMaterialAsync("Yarn", 1);

            //Assert
            Assert.NotNull(finishedProducts);
            Assert.Single(finishedProducts); //Should return one finished product using Yarn ID 1
            Assert.Equal("Hat", finishedProducts[0].Name);
        }

        [Fact]
        public async Task GetMaterialsByFinishedProductIdAsync_ShouldReturnMaterialsForGivenProduct()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductMaterialRepository(context);

            //Act
            var materials = await repository.GetMaterialsByFinishedProductIdAsync(1);

            //Assert
            Assert.NotNull(materials);
            Assert.Equal(2, materials.Count); //Hat has two materials: Yarn and SafetyEyes
        }

        [Fact]
        public async Task CanMakeProductAsync_ShouldReturnTrueIfEnoughMaterial()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductMaterialRepository(context);

            //Act
            var canMake = await repository.CanMakeProductAsync(1, 5);

            //Assert
            Assert.True(canMake); //Should return true because there are 5 units of Yarn
        }

        [Fact]
        public async Task GetDetailedMaterialsByProductNameAsync_ShouldReturnDetailedMaterialInfo()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductMaterialRepository(context);

            //Act
            var detailedMaterials = await repository.GetDetailedMaterialsByProductNameAsync("Hat");

            //Assert
            Assert.NotNull(detailedMaterials);
            Assert.Equal(2, detailedMaterials.Count); //Hat has two materials: Yarn and SafetyEyes
        }

        [Fact]
        public async Task UpdateYarnQuantityAsync_ShouldUpdateQuantity()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductMaterialRepository(context);

            //Act
            var updated = await repository.UpdateYarnQuantityAsync(1, 1, 10);

            //Assert
            Assert.True(updated);
            var material = await context.FinishedProductMaterials
                .FirstOrDefaultAsync(fpm => fpm.FinishedProductsID == 1 && fpm.MaterialID == 1 && fpm.MaterialType == "Yarn");
            Assert.Equal(10, material?.QuantityUsed);
        }

        [Fact]
        public async Task DeleteYarnMaterialAsync_ShouldRemoveYarnFromProduct()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductMaterialRepository(context);

            //Act
            var deleted = await repository.DeleteYarnMaterialAsync(1, 1);

            //Assert
            Assert.True(deleted);
            var exists = await context.FinishedProductMaterials
                .AnyAsync(fpm => fpm.FinishedProductsID == 1 && fpm.MaterialID == 1 && fpm.MaterialType == "Yarn");
            Assert.False(exists); //Yarn should no longer exist for Hat
        }

        [Fact]
        public async Task DeleteSafetyEyesMaterialAsync_ShouldRemoveSafetyEyesFromProduct()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductMaterialRepository(context);

            //Act
            var deleted = await repository.DeleteSafetyEyesMaterialAsync(1, 1);

            //Assert
            Assert.True(deleted);
            var exists = await context.FinishedProductMaterials
                .AnyAsync(fpm => fpm.FinishedProductsID == 1 && fpm.MaterialID == 1 && fpm.MaterialType == "SafetyEyes");
            Assert.False(exists); //SafetyEyes should no longer exist for Hat
        }

        [Fact]
        public async Task DeleteStuffingFromProductAsync_ShouldReturnFalseIfMaterialNotFound()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductMaterialRepository(context);

            //Act
            var result = await repository.DeleteStuffingFromProductAsync(1, -1); //Invalid ID

            //Assert
            Assert.False(result); //Should return false as the material is not found
        }
    }
}
