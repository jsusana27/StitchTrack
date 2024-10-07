using Microsoft.EntityFrameworkCore;
using CrochetBusinessAPI.Data;
using CrochetBusinessAPI.Models;
using CrochetBusinessAPI.Repositories;

namespace CrochetBusinessAPI.Tests.Repositories
{
    public class YarnRepositoryTests
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
                },
                new Yarn
                {
                    Brand = "BrandA",
                    FiberType = "Cotton",
                    FiberWeight = 3,
                    Color = "Green",
                    Price = 6.99m,
                    NumberOfSkeinsOwned = 5,
                    YardagePerSkein = 150,
                    GramsPerSkein = 75
                }
            );
            context.SaveChanges();
        }

        [Fact]
        public async Task GetAllYarnBrandsAsync_ShouldReturnDistinctBrands()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var yarnRepository = new YarnRepository(context);

            //Act
            var brands = await yarnRepository.GetAllYarnBrandsAsync();

            //Assert
            Assert.NotNull(brands);
            Assert.Equal(2, brands.Count); //Should be BrandA and BrandB
        }

        [Fact]
        public async Task GetAllYarnColorsAsync_ShouldReturnDistinctColors()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var yarnRepository = new YarnRepository(context);

            //Act
            var colors = await yarnRepository.GetAllYarnColorsAsync();

            //Assert
            Assert.NotNull(colors);
            Assert.Equal(3, colors.Count); //Should be Red, Blue, and Green
        }

                [Fact]
        public async Task GetAllYarnFiberTypesAsync_ShouldReturnDistinctFiberTypes()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var yarnRepository = new YarnRepository(context);

            //Act
            var fiberTypes = await yarnRepository.GetAllYarnFiberTypesAsync();

            //Assert
            Assert.NotNull(fiberTypes);
            Assert.Equal(2, fiberTypes.Count); //Should be "Chenille Polyester" and "Cotton"
        }

        [Fact]
        public async Task GetAllYarnFiberWeightsAsync_ShouldReturnDistinctFiberWeights()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var yarnRepository = new YarnRepository(context);

            //Act
            var fiberWeights = await yarnRepository.GetAllYarnFiberWeightsAsync();

            //Assert
            Assert.NotNull(fiberWeights);
            Assert.Equal(2, fiberWeights.Count); //Should be 3 and 4
        }

        [Fact]
        public async Task GetAllYarnSortedByPriceAsync_ShouldReturnYarnsSortedByPrice()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var yarnRepository = new YarnRepository(context);

            //Act
            var yarns = await yarnRepository.GetAllYarnSortedByPriceAsync();

            //Assert
            Assert.NotNull(yarns);
            Assert.Equal(3, yarns.Count);
            Assert.Equal(5.99m, yarns.First().Price); //First item should have the lowest price: 5.99
            Assert.Equal(7.99m, yarns.Last().Price); //Last item should have the highest price: 7.99
        }

        [Fact]
        public async Task GetAllYarnSortedByYardagePerSkeinAsync_ShouldReturnYarnsSortedByYardage()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var yarnRepository = new YarnRepository(context);

            //Act
            var yarns = await yarnRepository.GetAllYarnSortedByYardagePerSkeinAsync();

            //Assert
            Assert.NotNull(yarns);
            Assert.Equal(3, yarns.Count);
            Assert.Equal(100, yarns.First().YardagePerSkein); //First item should have the smallest yardage: 100
            Assert.Equal(200, yarns.Last().YardagePerSkein); //Last item should have the largest yardage: 200
        }

        [Fact]
        public async Task GetAllYarnSortedByGramsPerSkeinAsync_ShouldReturnYarnsSortedByGrams()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var yarnRepository = new YarnRepository(context);

            //Act
            var yarns = await yarnRepository.GetAllYarnSortedByGramsPerSkeinAsync();

            //Assert
            Assert.NotNull(yarns);
            Assert.Equal(3, yarns.Count);
            Assert.Equal(50, yarns.First().GramsPerSkein); //First item should have the lowest grams: 50
            Assert.Equal(100, yarns.Last().GramsPerSkein); //Last item should have the highest grams: 100
        }

        [Fact]
        public async Task GetAllYarnSortedByNumberInStockAsync_ShouldReturnYarnsSortedByStock()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var yarnRepository = new YarnRepository(context);

            //Act
            var yarns = await yarnRepository.GetAllYarnSortedByNumberInStockAsync();

            //Assert
            Assert.NotNull(yarns);
            Assert.Equal(3, yarns.Count);
            Assert.Equal(5, yarns.First().NumberOfSkeinsOwned); //First item should have the fewest skeins: 5
            Assert.Equal(20, yarns.Last().NumberOfSkeinsOwned); //Last item should have the most skeins: 20
        }

        [Fact]
        public async Task CheckYarnExistsAsync_ShouldReturnTrueIfYarnExists()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var yarnRepository = new YarnRepository(context);

            //Act
            var exists = await yarnRepository.CheckYarnExistsAsync("BrandA", "Chenille Polyester", 3, "Red");

            //Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task CheckYarnExistsAsync_ShouldReturnFalseIfYarnDoesNotExist()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var yarnRepository = new YarnRepository(context);

            //Act
            var exists = await yarnRepository.CheckYarnExistsAsync("BrandA", "Cotton", 3, "Purple");

            //Assert
            Assert.False(exists);
        }

        [Fact]
        public async Task AddAsync_ShouldAddYarnToDatabase()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var yarnRepository = new YarnRepository(context);

            //Arrange
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
            await yarnRepository.AddAsync(newYarn);

            //Assert
            var yarns = await context.Yarns.ToListAsync();
            Assert.Equal(4, yarns.Count); //Initial 3 + 1 new yarn
        }

        [Fact]
        public async Task UpdateYarnQuantityAsync_ShouldUpdateQuantity()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var yarnRepository = new YarnRepository(context);

            //Act
            var updated = await yarnRepository.UpdateYarnQuantityAsync("BrandA", "Chenille Polyester", 3, "Red", 50);

            //Assert
            Assert.True(updated);
            var yarn = await context.Yarns.FirstOrDefaultAsync(y => y.Brand == "BrandA" && y.Color == "Red");
            Assert.Equal(50, yarn?.NumberOfSkeinsOwned);
        }

        [Fact]
        public async Task DeleteYarnByDetailsAsync_ShouldDeleteYarn()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var yarnRepository = new YarnRepository(context);

            //Act
            var deletedYarn = await yarnRepository.DeleteYarnByDetailsAsync("BrandA", "Chenille Polyester", 3, "Red");

            //Assert
            Assert.NotNull(deletedYarn);
            var exists = await yarnRepository.CheckYarnExistsAsync("BrandA", "Chenille Polyester", 3, "Red");
            Assert.False(exists);
        }

        [Fact]
        public async Task GetYarnBySpecificationsAsync_ShouldReturnCorrectYarn()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var yarnRepository = new YarnRepository(context);

            //Act
            var yarn = await yarnRepository.GetYarnBySpecificationsAsync("BrandA", "Chenille Polyester", 3, "Red");

            //Assert
            Assert.NotNull(yarn);
            Assert.Equal("Red", yarn?.Color);
        }
    }
}