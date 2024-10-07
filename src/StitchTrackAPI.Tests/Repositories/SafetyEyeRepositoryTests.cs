using Microsoft.EntityFrameworkCore;
using CrochetBusinessAPI.Data;
using CrochetBusinessAPI.Models;
using CrochetBusinessAPI.Repositories;

namespace CrochetBusinessAPI.Tests.Repositories
{
    public class SafetyEyeRepositoryTests
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
            context.SafetyEyes.AddRange(
                new SafetyEye { SizeInMM = 8, Color = "Black", Shape = "Round", Price = 0.5m, NumberOfEyesOwned = 100 },
                new SafetyEye { SizeInMM = 10, Color = "Blue", Shape = "Round", Price = 0.7m, NumberOfEyesOwned = 50 },
                new SafetyEye { SizeInMM = 12, Color = "Green", Shape = "Oval", Price = 1.0m, NumberOfEyesOwned = 20 }
            );
            context.SaveChanges();
        }

        [Fact]
        public async Task GetAllSafetyEyeSizesAsync_ShouldReturnDistinctSizes()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new SafetyEyeRepository(context);

            //Act
            var sizes = await repository.GetAllSafetyEyeSizesAsync();

            //Assert
            Assert.NotNull(sizes);
            Assert.Equal(3, sizes.Count); //Should return sizes 8, 10, and 12
        }

        [Fact]
        public async Task GetAllSafetyEyeColorsAsync_ShouldReturnDistinctColors()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new SafetyEyeRepository(context);

            //Act
            var colors = await repository.GetAllSafetyEyeColorsAsync();

            //Assert
            Assert.NotNull(colors);
            Assert.Equal(3, colors.Count); //Should return colors Black, Blue, and Green
        }

        [Fact]
        public async Task GetAllSafetyEyeShapesAsync_ShouldReturnDistinctShapes()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new SafetyEyeRepository(context);

            //Act
            var shapes = await repository.GetAllSafetyEyeShapesAsync();

            //Assert
            Assert.NotNull(shapes);
            Assert.Equal(2, shapes.Count); //Should return shapes Round and Oval
        }

        [Fact]
        public async Task GetSafetyEyesSortedByPriceAsync_ShouldReturnEyesSortedByPrice()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new SafetyEyeRepository(context);

            //Act
            var safetyEyes = await repository.GetSafetyEyesSortedByPriceAsync();

            //Assert
            Assert.NotNull(safetyEyes);
            Assert.Equal(3, safetyEyes.Count);
            Assert.Equal(0.5m, safetyEyes[0].Price);  //Lowest price first
            Assert.Equal(1.0m, safetyEyes[2].Price);  //Highest price last
        }

        [Fact]
        public async Task GetSafetyEyesSortedByStockAsync_ShouldReturnEyesSortedByStock()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new SafetyEyeRepository(context);

            //Act
            var safetyEyes = await repository.GetSafetyEyesSortedByStockAsync();

            //Assert
            Assert.NotNull(safetyEyes);
            Assert.Equal(3, safetyEyes.Count);
            Assert.Equal(20, safetyEyes[0].NumberOfEyesOwned);  //Lowest stock first
            Assert.Equal(100, safetyEyes[2].NumberOfEyesOwned);  //Highest stock last
        }

        [Fact]
        public async Task CheckSafetyEyeExistsAsync_ShouldReturnTrueIfExists()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new SafetyEyeRepository(context);

            //Act
            var exists = await repository.CheckSafetyEyeExistsAsync(8, "Black", "Round");

            //Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task CheckSafetyEyeExistsAsync_ShouldReturnFalseIfNotExists()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new SafetyEyeRepository(context);

            //Act
            var exists = await repository.CheckSafetyEyeExistsAsync(14, "Red", "Round");

            //Assert
            Assert.False(exists);
        }

        [Fact]
        public async Task DeleteSafetyEyesByDetailsAsync_ShouldDeleteSpecificSafetyEye()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new SafetyEyeRepository(context);

            //Act
            var deletedEye = await repository.DeleteSafetyEyesByDetailsAsync(10, "Blue", "Round");

            //Assert
            Assert.NotNull(deletedEye);
            Assert.Equal("Blue", deletedEye.Color);
            var exists = await repository.CheckSafetyEyeExistsAsync(10, "Blue", "Round");
            Assert.False(exists);  //Should no longer exist
        }

        [Fact]
        public async Task UpdateSafetyEyeQuantityAsync_ShouldUpdateQuantity()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new SafetyEyeRepository(context);
            var initialEye = context.SafetyEyes.First(se => se.SizeInMM == 8 && se.Color == "Black");

            //Act
            var updated = await repository.UpdateSafetyEyeQuantityAsync(8, "Black", "Round", 150);

            //Assert
            Assert.True(updated);
            var updatedEye = await context.SafetyEyes.FindAsync(initialEye.SafetyEyesID); //Assuming `SafetyEyeID` is the primary key
            Assert.Equal(150, updatedEye?.NumberOfEyesOwned);
        }

        [Fact]
        public async Task GetSafetyEyesBySpecificationsAsync_ShouldReturnCorrectSafetyEye()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new SafetyEyeRepository(context);

            //Act
            var safetyEye = await repository.GetSafetyEyesBySpecificationsAsync(12, "Green", "Oval");

            //Assert
            Assert.NotNull(safetyEye);
            Assert.Equal("Green", safetyEye?.Color);
        }
    }
}