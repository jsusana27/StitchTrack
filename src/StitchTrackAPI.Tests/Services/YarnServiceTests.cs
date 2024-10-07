using Microsoft.EntityFrameworkCore;
using CrochetBusinessAPI.Data;
using CrochetBusinessAPI.Models;
using CrochetBusinessAPI.Repositories;
using CrochetBusinessAPI.Services;

namespace CrochetBusinessAPI.Tests.Services
{
    public class YarnServiceTests
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
            context.Yarns.AddRange(
                new Yarn
                {
                    Brand = "BrandA",
                    FiberType = "Chenille Polyester",
                    FiberWeight = 3,
                    Color = "Red",
                    Price = 5.99m,
                    NumberOfSkeinsOwned = 10,
                    YardagePerSkein = 100,
                    GramsPerSkein = 50
                },
                new Yarn
                {
                    Brand = "BrandB",
                    FiberType = "Chenille Polyester",
                    FiberWeight = 4,
                    Color = "Blue",
                    Price = 7.99m,
                    NumberOfSkeinsOwned = 20,
                    YardagePerSkein = 200,
                    GramsPerSkein = 100
                }
            );
            context.SaveChanges();
        }

        [Fact]
        public async Task GetAllYarnBrandsAsync_ShouldReturnAllBrands()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var yarnRepository = new YarnRepository(context);
            var yarnService = new YarnService(yarnRepository);

            //Act
            var brands = await yarnService.GetAllYarnBrandsAsync();

            //Assert
            Assert.NotNull(brands);
            Assert.Equal(2, brands.Count);
            Assert.Contains("BrandA", brands);
            Assert.Contains("BrandB", brands);
        }

        [Fact]
        public async Task GetAllYarnColorsAsync_ShouldReturnAllColors()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var yarnRepository = new YarnRepository(context);
            var yarnService = new YarnService(yarnRepository);

            //Act
            var colors = await yarnService.GetAllYarnColorsAsync();

            //Assert
            Assert.NotNull(colors);
            Assert.Equal(2, colors.Count);
            Assert.Contains("Red", colors);
            Assert.Contains("Blue", colors);
        }

        [Fact]
        public async Task GetAllYarnFiberTypesAsync_ShouldReturnAllFiberTypes()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var yarnRepository = new YarnRepository(context);
            var yarnService = new YarnService(yarnRepository);

            //Act
            var fiberTypes = await yarnService.GetAllYarnFiberTypesAsync();

            //Assert
            Assert.NotNull(fiberTypes);
            Assert.Single(fiberTypes); //Both have the same fiber type "Chenille Polyester"
            Assert.Contains("Chenille Polyester", fiberTypes);
        }

        [Fact]
        public async Task GetAllYarnFiberWeightsAsync_ShouldReturnAllFiberWeights()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var yarnRepository = new YarnRepository(context);
            var yarnService = new YarnService(yarnRepository);

            //Act
            var fiberWeights = await yarnService.GetAllYarnFiberWeightsAsync();

            //Assert
            Assert.NotNull(fiberWeights);
            Assert.Equal(2, fiberWeights.Count);
            Assert.Contains(3, fiberWeights);
            Assert.Contains(4, fiberWeights);
        }

        [Fact]
        public async Task AddAsync_ShouldAddNewYarn()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var yarnRepository = new YarnRepository(context);
            var yarnService = new YarnService(yarnRepository);
            var newYarn = new Yarn
            {
                Brand = "BrandC",
                FiberType = "Acrylic",
                FiberWeight = 5,
                Color = "Yellow",
                Price = 8.0m,
                NumberOfSkeinsOwned = 30,
                YardagePerSkein = 250,
                GramsPerSkein = 125
            };

            //Act
            await yarnService.AddAsync(newYarn);
            var yarns = await context.Yarns.ToListAsync();

            //Assert
            Assert.Equal(3, yarns.Count); //2 initial + 1 new yarn
            Assert.Contains(yarns, y => y.Brand == "BrandC");
        }

        [Fact]
        public async Task CheckYarnExistsAsync_ShouldReturnTrueIfYarnExists()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var yarnRepository = new YarnRepository(context);
            var yarnService = new YarnService(yarnRepository);

            //Act
            var exists = await yarnService.CheckYarnExistsAsync("BrandA", "Chenille Polyester", 3, "Red");

            //Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task CheckYarnExistsAsync_ShouldReturnFalseIfYarnDoesNotExist()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var yarnRepository = new YarnRepository(context);
            var yarnService = new YarnService(yarnRepository);

            //Act
            var exists = await yarnService.CheckYarnExistsAsync("NonExistentBrand", "Acrylic", 5, "Yellow");

            //Assert
            Assert.False(exists);
        }

        [Fact]
        public async Task UpdateYarnQuantityAsync_ShouldUpdateYarnQuantity()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var yarnRepository = new YarnRepository(context);
            var yarnService = new YarnService(yarnRepository);

            //Act
            var updated = await yarnService.UpdateYarnQuantityAsync("BrandA", "Chenille Polyester", 3, "Red", 50);
            var yarn = await context.Yarns.FirstAsync(y => y.Brand == "BrandA" && y.Color == "Red");

            //Assert
            Assert.True(updated);
            Assert.Equal(50, yarn.NumberOfSkeinsOwned);
        }

        [Fact]
        public async Task DeleteYarnByDetailsAsync_ShouldDeleteYarnIfExists()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var yarnRepository = new YarnRepository(context);
            var yarnService = new YarnService(yarnRepository);

            //Act
            var deletedYarn = await yarnService.DeleteYarnByDetailsAsync("BrandA", "Chenille Polyester", 3, "Red");
            var yarns = await context.Yarns.ToListAsync();

            //Assert
            Assert.NotNull(deletedYarn);
            Assert.Single(yarns); //Should be reduced to 1 after deletion
        }

                [Fact]
        public async Task GetAllYarnSortedByPriceAsync_ShouldReturnYarnsSortedByPrice()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var yarnRepository = new YarnRepository(context);
            var yarnService = new YarnService(yarnRepository);

            //Act
            var sortedYarns = await yarnService.GetAllYarnSortedByPriceAsync();

            //Assert
            Assert.NotNull(sortedYarns);
            Assert.Equal(2, sortedYarns.Count);
            Assert.Equal("BrandA", sortedYarns[0].Brand); //BrandA has a lower price ($5.99) than BrandB ($7.99)
            Assert.Equal("BrandB", sortedYarns[1].Brand);
        }

        [Fact]
        public async Task GetAllYarnSortedByYardagePerSkeinAsync_ShouldReturnYarnsSortedByYardage()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var yarnRepository = new YarnRepository(context);
            var yarnService = new YarnService(yarnRepository);

            //Act
            var sortedYarns = await yarnService.GetAllYarnSortedByYardagePerSkeinAsync();

            //Assert
            Assert.NotNull(sortedYarns);
            Assert.Equal(2, sortedYarns.Count);
            Assert.Equal("BrandA", sortedYarns[0].Brand); //BrandA has a lower yardage (100) than BrandB (200)
            Assert.Equal("BrandB", sortedYarns[1].Brand);
        }

        [Fact]
        public async Task GetAllYarnSortedByGramsPerSkeinAsync_ShouldReturnYarnsSortedByGrams()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var yarnRepository = new YarnRepository(context);
            var yarnService = new YarnService(yarnRepository);

            //Act
            var sortedYarns = await yarnService.GetAllYarnSortedByGramsPerSkeinAsync();

            //Assert
            Assert.NotNull(sortedYarns);
            Assert.Equal(2, sortedYarns.Count);
            Assert.Equal("BrandA", sortedYarns[0].Brand); //BrandA has a lower weight (50 grams) than BrandB (100 grams)
            Assert.Equal("BrandB", sortedYarns[1].Brand);
        }

        [Fact]
        public async Task GetAllYarnSortedByNumberInStockAsync_ShouldReturnYarnsSortedByNumberInStock()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var yarnRepository = new YarnRepository(context);
            var yarnService = new YarnService(yarnRepository);

            //Act
            var sortedYarns = await yarnService.GetAllYarnSortedByNumberInStockAsync();

            //Assert
            Assert.NotNull(sortedYarns);
            Assert.Equal(2, sortedYarns.Count);
            Assert.Equal("BrandA", sortedYarns[0].Brand); //BrandA has fewer skeins (10) than BrandB (20)
            Assert.Equal("BrandB", sortedYarns[1].Brand);
        }

        [Fact]
        public async Task GetYarnIdBySpecificationsAsync_ShouldReturnCorrectYarn()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var yarnRepository = new YarnRepository(context);
            var yarnService = new YarnService(yarnRepository);

            //Act
            var yarn = await yarnService.GetYarnIdBySpecificationsAsync("BrandA", "Chenille Polyester", 3, "Red");

            //Assert
            Assert.NotNull(yarn);
            Assert.Equal("BrandA", yarn?.Brand);
            Assert.Equal("Red", yarn?.Color);
            Assert.Equal(3, yarn?.FiberWeight);
        }

        [Fact]
        public async Task GetYarnIdBySpecificationsAsync_ShouldReturnNullForNonExistentYarn()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var yarnRepository = new YarnRepository(context);
            var yarnService = new YarnService(yarnRepository);

            //Act
            var yarn = await yarnService.GetYarnIdBySpecificationsAsync("NonExistentBrand", "Acrylic", 5, "Yellow");

            //Assert
            Assert.Null(yarn); //Should return null since the specified yarn doesn't exist
        }
    }
}