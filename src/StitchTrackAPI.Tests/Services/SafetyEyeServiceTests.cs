using CrochetBusinessAPI.Data;
using CrochetBusinessAPI.Models;
using CrochetBusinessAPI.Repositories;
using CrochetBusinessAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace CrochetBusinessAPI.Tests.Services
{
    public class SafetyEyeServiceTests
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
                new SafetyEye
                {
                    SizeInMM = 6,
                    Color = "Black",
                    Shape = "Round",
                    Price = 0.50m,
                    NumberOfEyesOwned = 15
                },
                new SafetyEye
                {
                    SizeInMM = 9,
                    Color = "Blue",
                    Shape = "Round",
                    Price = 0.75m,
                    NumberOfEyesOwned = 30
                }
            );
            context.SaveChanges();
        }

        [Fact]
        public async Task GetAllSafetyEyeSizesAsync_ShouldReturnAllUniqueSizes()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var safetyEyeRepository = new SafetyEyeRepository(context);
            var safetyEyeService = new SafetyEyeService(safetyEyeRepository);

            //Act
            var sizes = await safetyEyeService.GetAllSafetyEyeSizesAsync();

            //Assert
            Assert.NotNull(sizes);
            Assert.Equal(2, sizes.Count); //Should return the 2 unique sizes
            Assert.Contains(6, sizes);
            Assert.Contains(9, sizes);
        }

        [Fact]
        public async Task GetAllSafetyEyeColorsAsync_ShouldReturnAllUniqueColors()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var safetyEyeRepository = new SafetyEyeRepository(context);
            var safetyEyeService = new SafetyEyeService(safetyEyeRepository);

            //Act
            var colors = await safetyEyeService.GetAllSafetyEyeColorsAsync();

            //Assert
            Assert.NotNull(colors);
            Assert.Equal(2, colors.Count); //Should return 2 unique colors
            Assert.Contains("Black", colors);
            Assert.Contains("Blue", colors);
        }

        [Fact]
        public async Task GetAllSafetyEyeShapesAsync_ShouldReturnAllUniqueShapes()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var safetyEyeRepository = new SafetyEyeRepository(context);
            var safetyEyeService = new SafetyEyeService(safetyEyeRepository);

            //Act
            var shapes = await safetyEyeService.GetAllSafetyEyeShapesAsync();

            //Assert
            Assert.NotNull(shapes);
            Assert.Single(shapes); //Should return only 1 unique shape: "Round"
            Assert.Contains("Round", shapes);
        }

        [Fact]
        public async Task GetSafetyEyesSortedByPriceAsync_ShouldReturnSafetyEyesSortedByPrice()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var safetyEyeRepository = new SafetyEyeRepository(context);
            var safetyEyeService = new SafetyEyeService(safetyEyeRepository);

            //Act
            var sortedSafetyEyes = await safetyEyeService.GetSafetyEyesSortedByPriceAsync();

            //Assert
            Assert.NotNull(sortedSafetyEyes);
            Assert.Equal(2, sortedSafetyEyes.Count);
            Assert.Equal("Black", sortedSafetyEyes[0].Color); //Black eyes should be cheaper ($0.50) than Blue eyes ($0.75)
            Assert.Equal("Blue", sortedSafetyEyes[1].Color);
        }

        [Fact]
        public async Task GetSafetyEyesSortedByStockAsync_ShouldReturnSafetyEyesSortedByNumberInStock()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var safetyEyeRepository = new SafetyEyeRepository(context);
            var safetyEyeService = new SafetyEyeService(safetyEyeRepository);

            //Act
            var sortedSafetyEyes = await safetyEyeService.GetSafetyEyesSortedByStockAsync();

            //Assert
            Assert.NotNull(sortedSafetyEyes);
            Assert.Equal(2, sortedSafetyEyes.Count);
            Assert.Equal("Black", sortedSafetyEyes[0].Color); //Black eyes have fewer pairs (15) than Blue eyes (30)
            Assert.Equal("Blue", sortedSafetyEyes[1].Color);
        }

        [Fact]
        public async Task DeleteSafetyEyesByDetailsAsync_ShouldDeleteAndReturnDeletedSafetyEye()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var safetyEyeRepository = new SafetyEyeRepository(context);
            var safetyEyeService = new SafetyEyeService(safetyEyeRepository);

            //Act
            var deletedSafetyEye = await safetyEyeService.DeleteSafetyEyesByDetailsAsync(6, "Black", "Round");

            //Assert
            Assert.NotNull(deletedSafetyEye);
            Assert.Equal(6, deletedSafetyEye?.SizeInMM);
            Assert.Equal("Black", deletedSafetyEye?.Color);

            var remainingSafetyEyes = await context.SafetyEyes.ToListAsync();
            Assert.Single(remainingSafetyEyes); //Only 1 safety eye should remain in the database
        }

        [Fact]
        public async Task SafetyEyeExistsAsync_ShouldReturnTrueIfSafetyEyeExists()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var safetyEyeRepository = new SafetyEyeRepository(context);
            var safetyEyeService = new SafetyEyeService(safetyEyeRepository);

            //Act
            var exists = await safetyEyeService.SafetyEyeExistsAsync(6, "Black", "Round");

            //Assert
            Assert.True(exists); //SafetyEye with size 6, color "Black", and shape "Round" exists
        }

        [Fact]
        public async Task SafetyEyeExistsAsync_ShouldReturnFalseIfSafetyEyeDoesNotExist()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var safetyEyeRepository = new SafetyEyeRepository(context);
            var safetyEyeService = new SafetyEyeService(safetyEyeRepository);

            //Act
            var exists = await safetyEyeService.SafetyEyeExistsAsync(12, "Red", "Oval");

            //Assert
            Assert.False(exists); //SafetyEye with size 12, color "Red", and shape "Oval" does not exist
        }

        [Fact]
        public async Task UpdateSafetyEyeQuantityAsync_ShouldUpdateQuantityOfExistingSafetyEye()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var safetyEyeRepository = new SafetyEyeRepository(context);
            var safetyEyeService = new SafetyEyeService(safetyEyeRepository);

            //Act
            var success = await safetyEyeService.UpdateSafetyEyeQuantityAsync(6, "Black", "Round", 50);

            //Assert
            Assert.True(success);
            var updatedSafetyEye = await context.SafetyEyes.FirstOrDefaultAsync(se => se.SizeInMM == 6 && se.Color == "Black" && se.Shape == "Round");
            Assert.Equal(50, updatedSafetyEye?.NumberOfEyesOwned); //Quantity should be updated to 50
        }

        [Fact]
        public async Task GetSafetyEyesIdBySpecificationsAsync_ShouldReturnCorrectSafetyEye()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var safetyEyeRepository = new SafetyEyeRepository(context);
            var safetyEyeService = new SafetyEyeService(safetyEyeRepository);

            //Act
            var safetyEye = await safetyEyeService.GetSafetyEyesIdBySpecificationsAsync(9, "Blue", "Round");

            //Assert
            Assert.NotNull(safetyEye);
            Assert.Equal(9, safetyEye?.SizeInMM);
            Assert.Equal("Blue", safetyEye?.Color);
        }
    }
}