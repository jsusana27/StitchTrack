using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrochetBusinessAPI.Controllers;
using CrochetBusinessAPI.Data;
using CrochetBusinessAPI.Models;
using CrochetBusinessAPI.Repositories;
using CrochetBusinessAPI.Services;

namespace CrochetBusinessAPI.Tests.Controllers
{
    public class YarnControllerTests
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
            //Seed Yarn data
            context.Yarns.AddRange(
                new Yarn
                {
                    YarnID = 1,
                    Brand = "BrandA",
                    FiberType = "Wool",
                    FiberWeight = 2,
                    Color = "Red",
                    NumberOfSkeinsOwned = 10
                },
                new Yarn
                {
                    YarnID = 2,
                    Brand = "BrandB",
                    FiberType = "Cotton",
                    FiberWeight = 3,
                    Color = "Blue",
                    NumberOfSkeinsOwned = 15
                }
            );

            context.SaveChanges(); //Save changes to the context
        }

        [Fact]
        public async Task GetYarnBrands_ShouldReturnListOfBrands()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new YarnRepository(context);
            var service = new YarnService(repository);
            var controller = new YarnController(service);

            //Act
            var result = await controller.GetYarnBrands();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var brands = Assert.IsType<List<string>>(okResult.Value);
            Assert.Contains("BrandA", brands);
            Assert.Contains("BrandB", brands);
        }

        [Fact]
        public async Task GetAllYarnColors_ShouldReturnListOfColors()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new YarnRepository(context);
            var service = new YarnService(repository);
            var controller = new YarnController(service);

            //Act
            var result = await controller.GetAllYarnColors();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var colors = Assert.IsType<List<string>>(okResult.Value);
            Assert.Contains("Red", colors);
            Assert.Contains("Blue", colors);
        }

        [Fact]
        public async Task GetAllYarnFiberTypes_ShouldReturnListOfFiberTypes()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new YarnRepository(context);
            var service = new YarnService(repository);
            var controller = new YarnController(service);

            //Act
            var result = await controller.GetAllYarnFiberTypes();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var fiberTypes = Assert.IsType<List<string>>(okResult.Value);
            Assert.Contains("Wool", fiberTypes);
            Assert.Contains("Cotton", fiberTypes);
        }

        [Fact]
        public async Task CheckYarnExistence_ShouldReturnTrueIfExists()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new YarnRepository(context);
            var service = new YarnService(repository);
            var controller = new YarnController(service);

            //Act
            var result = await controller.CheckYarnExistence("BrandA", "Wool", 2, "Red");

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value); //Ensure that the value is not null before accessing properties

            //Use reflection to access the anonymous type property
            var existsProperty = okResult.Value?.GetType().GetProperty("exists"); //Use null-conditional operator to safely access the property
            Assert.NotNull(existsProperty); //Ensure that the 'exists' property is present

            var existsValue = existsProperty?.GetValue(okResult.Value); //Safely get the value of the property
            Assert.NotNull(existsValue); //Ensure that the value is not null before casting
            Assert.True((bool)existsValue); //Cast to bool and check if it is true
        }

        [Fact]
        public async Task GetYarnId_ShouldReturnYarnIdWhenFound()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new YarnRepository(context);
            var service = new YarnService(repository);
            var controller = new YarnController(service);

            //Act
            var result = await controller.GetYarnId("BrandA", "Wool", 2, "Red");

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value); //Ensure that the value is not null

            //Debug the actual type and properties of the returned value
            Console.WriteLine($"Returned type: {okResult.Value.GetType()}");
            foreach (var prop in okResult.Value.GetType().GetProperties())
            {
                Console.WriteLine($"Property Name: {prop.Name}, Value: {prop.GetValue(okResult.Value)}");
            }

            //Use reflection to access the property if needed
            var yarnIdProperty = okResult.Value.GetType().GetProperty("yarnId");
            Assert.NotNull(yarnIdProperty); //Ensure that 'yarnId' property exists
            var yarnId = (int?)yarnIdProperty.GetValue(okResult.Value);
            Assert.Equal(1, yarnId); //Assert that the value of yarnId is 1
        }

        [Fact]
        public async Task GetYarnId_ShouldReturnNotFoundWhenYarnDoesNotExist()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new YarnRepository(context);
            var service = new YarnService(repository);
            var controller = new YarnController(service);

            //Act
            var result = await controller.GetYarnId("NonExistentBrand", "Wool", 1, "Green");

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.NotNull(notFoundResult.Value); //Ensure that Value is not null

            //Extract the 'message' property from the anonymous object
            var messageProperty = notFoundResult.Value.GetType().GetProperty("message");
            Assert.NotNull(messageProperty); //Ensure the 'message' property exists
            var actualMessage = messageProperty.GetValue(notFoundResult.Value)?.ToString();

            //Assert that the message matches the expected value
            Assert.Equal("Yarn not found with the given specifications.", actualMessage);
        }

        [Fact]
        public async Task UpdateYarnQuantity_ShouldReturnOkWhenUpdated()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new YarnRepository(context);
            var service = new YarnService(repository);
            var controller = new YarnController(service);

            var mockYarn = new Yarn
            {
                Brand = "BrandA",
                FiberType = "Wool",
                FiberWeight = 2,
                Color = "Red",
                NumberOfSkeinsOwned = 5
            };

            //Act
            var result = await controller.UpdateYarnQuantity(mockYarn);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Yarn quantity updated successfully.", okResult.Value);
        }

                [Fact]
        public async Task GetAllYarnFiberWeights_ShouldReturnListOfFiberWeights()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new YarnRepository(context);
            var service = new YarnService(repository);
            var controller = new YarnController(service);

            //Act
            var result = await controller.GetAllYarnFiberWeights();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var fiberWeights = Assert.IsType<List<int>>(okResult.Value);
            Assert.Contains(2, fiberWeights);
            Assert.Contains(3, fiberWeights);
        }

        [Fact]
        public async Task GetAllYarnSortedByPrice_ShouldReturnYarnsSortedByPrice()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new YarnRepository(context);
            var service = new YarnService(repository);
            var controller = new YarnController(service);

            //Act
            var result = await controller.GetAllYarnSortedByPrice();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var yarns = Assert.IsType<List<Yarn>>(okResult.Value);
            Assert.Equal(2, yarns.Count); //There should be 2 yarn entries

            //Assert that yarns are sorted by price (manually assigned order here, if prices were defined)
            Assert.Equal("BrandA", yarns[0].Brand);
            Assert.Equal("BrandB", yarns[1].Brand);
        }

        [Fact]
        public async Task GetAllYarnSortedByYardagePerSkein_ShouldReturnYarnsSortedByYardage()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new YarnRepository(context);
            var service = new YarnService(repository);
            var controller = new YarnController(service);

            //Act
            var result = await controller.GetAllYarnSortedByYardagePerSkein();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var yarns = Assert.IsType<List<Yarn>>(okResult.Value);
            Assert.Equal(2, yarns.Count); //Verify there are 2 yarn entries
        }

        [Fact]
        public async Task GetAllYarnSortedByGramsPerSkein_ShouldReturnYarnsSortedByGrams()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new YarnRepository(context);
            var service = new YarnService(repository);
            var controller = new YarnController(service);

            //Act
            var result = await controller.GetAllYarnSortedByGramsPerSkein();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var yarns = Assert.IsType<List<Yarn>>(okResult.Value);
            Assert.Equal(2, yarns.Count); //Verify there are 2 yarn entries
        }

        [Fact]
        public async Task GetAllYarnSortedByNumberInStock_ShouldReturnYarnsSortedByStock()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new YarnRepository(context);
            var service = new YarnService(repository);
            var controller = new YarnController(service);

            //Act
            var result = await controller.GetAllYarnSortedByNumberInStock();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var yarns = Assert.IsType<List<Yarn>>(okResult.Value);
            Assert.Equal(2, yarns.Count); //Verify there are 2 yarn entries
        }

        [Fact]
        public async Task DeleteYarnByDetails_ShouldDeleteYarnAndReturnOk()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new YarnRepository(context);
            var service = new YarnService(repository);
            var controller = new YarnController(service);

            var yarnToDelete = new Yarn { Brand = "BrandA", FiberType = "Wool", FiberWeight = 2, Color = "Red" };

            //Act
            var result = await controller.DeleteYarnByDetails(yarnToDelete);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Yarn deleted successfully.", okResult.Value);
        }

        [Fact]
        public async Task DeleteYarnByDetails_ShouldReturnNotFoundWhenYarnDoesNotExist()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new YarnRepository(context);
            var service = new YarnService(repository);
            var controller = new YarnController(service);

            var nonExistentYarn = new Yarn { Brand = "NonExistentBrand", FiberType = "Silk", FiberWeight = 5, Color = "Purple" };

            //Act
            var result = await controller.DeleteYarnByDetails(nonExistentYarn);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Yarn not found.", notFoundResult.Value);
        }
    }
}