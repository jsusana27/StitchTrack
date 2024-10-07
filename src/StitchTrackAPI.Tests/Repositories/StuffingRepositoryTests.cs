using Microsoft.EntityFrameworkCore;
using CrochetBusinessAPI.Data;
using CrochetBusinessAPI.Models;
using CrochetBusinessAPI.Repositories;

namespace CrochetBusinessAPI.Tests.Repositories
{
    public class StuffingRepositoryTests
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
                new Stuffing { Brand = "BrandA", Type = "Polyester", PricePerFivelbs = 12.99m },
                new Stuffing { Brand = "BrandB", Type = "Cotton", PricePerFivelbs = 15.99m },
                new Stuffing { Brand = "BrandA", Type = "Wool", PricePerFivelbs = 25.99m }
            );
            context.SaveChanges();
        }

        [Fact]
        public async Task GetAllStuffingBrandsAsync_ShouldReturnDistinctBrands()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new StuffingRepository(context);

            //Act
            var brands = await repository.GetAllStuffingBrandsAsync();

            //Assert
            Assert.NotNull(brands);
            Assert.Equal(2, brands.Count); //Should return BrandA and BrandB
        }

        [Fact]
        public async Task GetAllStuffingTypesAsync_ShouldReturnDistinctTypes()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new StuffingRepository(context);

            //Act
            var types = await repository.GetAllStuffingTypesAsync();

            //Assert
            Assert.NotNull(types);
            Assert.Equal(3, types.Count); //Should return Polyester, Cotton, and Wool
        }

        [Fact]
        public async Task GetStuffingSortedByPricePer5LbsAsync_ShouldReturnStuffingSortedByPrice()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new StuffingRepository(context);

            //Act
            var stuffingList = await repository.GetStuffingSortedByPricePer5LbsAsync();

            //Assert
            Assert.NotNull(stuffingList);
            Assert.Equal(3, stuffingList.Count);
            Assert.Equal(12.99m, stuffingList[0].PricePerFivelbs);  //Lowest price first
            Assert.Equal(25.99m, stuffingList[2].PricePerFivelbs);  //Highest price last
        }

        [Fact]
        public async Task DeleteStuffingByDetailsAsync_ShouldDeleteSpecificStuffing()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new StuffingRepository(context);

            //Act
            var deletedStuffing = await repository.DeleteStuffingByDetailsAsync("BrandA", "Polyester");

            //Assert
            Assert.NotNull(deletedStuffing);
            Assert.Equal("Polyester", deletedStuffing.Type);
            var exists = await repository.GetStuffingBySpecificationsAsync("BrandA", "Polyester");
            Assert.Null(exists);  //Should no longer exist
        }

        [Fact]
        public async Task DeleteStuffingByDetailsAsync_ShouldReturnNullIfStuffingDoesNotExist()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new StuffingRepository(context);

            //Act
            var deletedStuffing = await repository.DeleteStuffingByDetailsAsync("BrandC", "Silk");

            //Assert
            Assert.Null(deletedStuffing);  //Should return null since it doesn't exist
        }

        [Fact]
        public async Task GetStuffingBySpecificationsAsync_ShouldReturnCorrectStuffing()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new StuffingRepository(context);

            //Act
            var stuffing = await repository.GetStuffingBySpecificationsAsync("BrandB", "Cotton");

            //Assert
            Assert.NotNull(stuffing);
            Assert.Equal("Cotton", stuffing?.Type);
        }

        [Fact]
        public async Task GetStuffingBySpecificationsAsync_ShouldReturnNullIfStuffingDoesNotExist()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new StuffingRepository(context);

            //Act
            var stuffing = await repository.GetStuffingBySpecificationsAsync("BrandC", "Silk");

            //Assert
            Assert.Null(stuffing);  //Should return null since it doesn't exist
        }
    }
}