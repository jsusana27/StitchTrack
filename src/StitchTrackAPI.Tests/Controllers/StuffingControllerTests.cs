using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrochetBusinessAPI.Controllers;
using CrochetBusinessAPI.Data;
using CrochetBusinessAPI.Models;
using CrochetBusinessAPI.Repositories;
using CrochetBusinessAPI.Services;

namespace CrochetBusinessAPI.Tests.Controllers
{
    public class StuffingControllerTests
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
                    StuffingID = 1,
                    Brand = "BrandA",
                    Type = "Poly-fil",
                    PricePerFivelbs = 20.0m
                },
                new Stuffing
                {
                    StuffingID = 2,
                    Brand = "BrandB",
                    Type = "Wool",
                    PricePerFivelbs = 30.0m
                }
            );
            context.SaveChanges();
        }

        [Fact]
        public async Task GetAllStuffingBrands_ShouldReturnAllBrands()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var stuffingRepository = new StuffingRepository(context);
            var stuffingService = new StuffingService(stuffingRepository);
            var controller = new StuffingController(stuffingService);

            //Act
            var result = await controller.GetAllStuffingBrands();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var brands = Assert.IsType<List<string>>(okResult.Value);
            Assert.Equal(2, brands.Count); //Should return 2 brands
            Assert.Contains("BrandA", brands);
            Assert.Contains("BrandB", brands);
        }

        [Fact]
        public async Task GetAllStuffingTypes_ShouldReturnAllTypes()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var stuffingRepository = new StuffingRepository(context);
            var stuffingService = new StuffingService(stuffingRepository);
            var controller = new StuffingController(stuffingService);

            //Act
            var result = await controller.GetAllStuffingTypes();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var types = Assert.IsType<List<string>>(okResult.Value);
            Assert.Equal(2, types.Count); //Should return 2 stuffing types
            Assert.Contains("Poly-fil", types);
            Assert.Contains("Wool", types);
        }

        [Fact]
        public async Task GetStuffingSortedByPricePer5Lbs_ShouldReturnStuffingSortedByPrice()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var stuffingRepository = new StuffingRepository(context);
            var stuffingService = new StuffingService(stuffingRepository);
            var controller = new StuffingController(stuffingService);

            //Act
            var result = await controller.GetStuffingSortedByPricePer5Lbs();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var stuffingList = Assert.IsType<List<Stuffing>>(okResult.Value);
            Assert.Equal(2, stuffingList.Count); //Should return 2 stuffing entries
            Assert.Equal("BrandA", stuffingList[0].Brand); //"BrandA" should be the first (lowest price)
            Assert.Equal("BrandB", stuffingList[1].Brand); //"BrandB" should be second (higher price)
        }

        [Fact]
        public async Task DeleteStuffingByDetails_ShouldReturnDeletedStuffing()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var stuffingRepository = new StuffingRepository(context);
            var stuffingService = new StuffingService(stuffingRepository);
            var controller = new StuffingController(stuffingService);

            //Act
            var result = await controller.DeleteStuffingByDetails("BrandA", "Poly-fil");

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var deletedStuffing = Assert.IsType<Stuffing>(okResult.Value);
            Assert.Equal("BrandA", deletedStuffing.Brand);
            Assert.Equal("Poly-fil", deletedStuffing.Type);

            var remainingStuffings = await context.Stuffings.ToListAsync();
            Assert.Single(remainingStuffings); //Only 1 stuffing should remain
        }

        [Fact]
        public async Task DeleteStuffingByDetails_ShouldReturnNotFound_WhenStuffingNotFound()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var stuffingRepository = new StuffingRepository(context);
            var stuffingService = new StuffingService(stuffingRepository);
            var controller = new StuffingController(stuffingService);

            //Act
            var result = await controller.DeleteStuffingByDetails("NonExistentBrand", "NonExistentType");

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal("Stuffing not found with the specified brand and type.", notFoundResult.Value);
        }
    }
}