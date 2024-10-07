using CrochetBusinessAPI.Data;
using CrochetBusinessAPI.Models;
using CrochetBusinessAPI.Repositories;
using CrochetBusinessAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace CrochetBusinessAPI.Tests.Services
{
    public class StuffingServiceTests
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
            context.Stuffings.AddRange(
                new Stuffing
                {
                    Brand = "BrandA",
                    Type = "Polyfill",
                    PricePerFivelbs = 20.0m,
                },
                new Stuffing
                {
                    Brand = "BrandB",
                    Type = "Wool",
                    PricePerFivelbs = 30.0m,
                }
            );
            context.SaveChanges();
        }

        [Fact]
        public async Task GetAllStuffingBrandsAsync_ShouldReturnAllUniqueBrands()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var stuffingRepository = new StuffingRepository(context);
            var stuffingService = new StuffingService(stuffingRepository);

            //Act
            var brands = await stuffingService.GetAllStuffingBrandsAsync();

            //Assert
            Assert.NotNull(brands);
            Assert.Equal(2, brands.Count); //Should return the 2 unique brands
            Assert.Contains("BrandA", brands);
            Assert.Contains("BrandB", brands);
        }

        [Fact]
        public async Task GetAllStuffingTypesAsync_ShouldReturnAllUniqueTypes()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var stuffingRepository = new StuffingRepository(context);
            var stuffingService = new StuffingService(stuffingRepository);

            //Act
            var types = await stuffingService.GetAllStuffingTypesAsync();

            //Assert
            Assert.NotNull(types);
            Assert.Equal(2, types.Count); //Should return 2 unique types
            Assert.Contains("Polyfill", types);
            Assert.Contains("Wool", types);
        }

        [Fact]
        public async Task GetStuffingSortedByPricePer5LbsAsync_ShouldReturnStuffingSortedByPrice()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var stuffingRepository = new StuffingRepository(context);
            var stuffingService = new StuffingService(stuffingRepository);

            //Act
            var sortedStuffings = await stuffingService.GetStuffingSortedByPricePer5LbsAsync();

            //Assert
            Assert.NotNull(sortedStuffings);
            Assert.Equal(2, sortedStuffings.Count);
            Assert.Equal("BrandA", sortedStuffings[0].Brand); //BrandA Polyfill ($20.0 per 5 lbs) should come before BrandB Wool ($30.0 per 5 lbs)
            Assert.Equal("BrandB", sortedStuffings[1].Brand);
        }

        [Fact]
        public async Task DeleteStuffingByDetailsAsync_ShouldDeleteAndReturnDeletedStuffing()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var stuffingRepository = new StuffingRepository(context);
            var stuffingService = new StuffingService(stuffingRepository);

            //Act
            var deletedStuffing = await stuffingService.DeleteStuffingByDetailsAsync("BrandA", "Polyfill");

            //Assert
            Assert.NotNull(deletedStuffing);
            Assert.Equal("BrandA", deletedStuffing?.Brand);
            Assert.Equal("Polyfill", deletedStuffing?.Type);

            var remainingStuffings = await context.Stuffings.ToListAsync();
            Assert.Single(remainingStuffings); //Only 1 stuffing should remain in the database
        }

        [Fact]
        public async Task GetStuffingIdBySpecificationsAsync_ShouldReturnCorrectStuffing()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var stuffingRepository = new StuffingRepository(context);
            var stuffingService = new StuffingService(stuffingRepository);

            //Act
            var stuffing = await stuffingService.GetStuffingIdBySpecificationsAsync("BrandB", "Wool");

            //Assert
            Assert.NotNull(stuffing);
            Assert.Equal("BrandB", stuffing?.Brand);
            Assert.Equal("Wool", stuffing?.Type);
        }

        [Fact]
        public async Task GetStuffingIdBySpecificationsAsync_ShouldReturnNullForNonExistentStuffing()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var stuffingRepository = new StuffingRepository(context);
            var stuffingService = new StuffingService(stuffingRepository);

            //Act
            var stuffing = await stuffingService.GetStuffingIdBySpecificationsAsync("NonExistentBrand", "Foam");

            //Assert
            Assert.Null(stuffing); //No stuffing should exist with the given brand and type
        }
    }
}