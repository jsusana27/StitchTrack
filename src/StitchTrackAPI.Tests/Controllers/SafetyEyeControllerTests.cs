using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrochetBusinessAPI.Controllers;
using CrochetBusinessAPI.Data;
using CrochetBusinessAPI.Models;
using CrochetBusinessAPI.Repositories;
using CrochetBusinessAPI.Services;

namespace CrochetBusinessAPI.Tests.Controllers
{
    public class SafetyEyeControllerTests
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
                new SafetyEye { SafetyEyesID = 1, SizeInMM = 6, Color = "Black", Shape = "Round", NumberOfEyesOwned = 50 },
                new SafetyEye { SafetyEyesID = 2, SizeInMM = 12, Color = "Blue", Shape = "Oval", NumberOfEyesOwned = 30 },
                new SafetyEye { SafetyEyesID = 3, SizeInMM = 6, Color = "Green", Shape = "Round", NumberOfEyesOwned = 20 }
            );
            context.SaveChanges();
        }

        [Fact]
        public async Task GetAllSafetyEyeSizes_ShouldReturnListOfSizes()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new SafetyEyeRepository(context);
            var service = new SafetyEyeService(repository);
            var controller = new SafetyEyeController(service);

            //Act
            var result = await controller.GetAllSafetyEyeSizes();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var sizes = Assert.IsType<List<int>>(okResult.Value);
            Assert.Contains(6, sizes);
            Assert.Contains(12, sizes);
        }

        [Fact]
        public async Task GetAllSafetyEyeColors_ShouldReturnListOfColors()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new SafetyEyeRepository(context);
            var service = new SafetyEyeService(repository);
            var controller = new SafetyEyeController(service);

            //Act
            var result = await controller.GetAllSafetyEyeColors();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var colors = Assert.IsType<List<string>>(okResult.Value);
            Assert.Contains("Black", colors);
            Assert.Contains("Blue", colors);
            Assert.Contains("Green", colors);
        }

        [Fact]
        public async Task GetAllSafetyEyeShapes_ShouldReturnListOfShapes()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new SafetyEyeRepository(context);
            var service = new SafetyEyeService(repository);
            var controller = new SafetyEyeController(service);

            //Act
            var result = await controller.GetAllSafetyEyeShapes();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var shapes = Assert.IsType<List<string>>(okResult.Value);
            Assert.Contains("Round", shapes);
            Assert.Contains("Oval", shapes);
        }

        [Fact]
        public async Task GetSafetyEyesSortedByPrice_ShouldReturnSortedList()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new SafetyEyeRepository(context);
            var service = new SafetyEyeService(repository);
            var controller = new SafetyEyeController(service);

            //Act
            var result = await controller.GetSafetyEyesSortedByPrice();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var safetyEyes = Assert.IsType<List<SafetyEye>>(okResult.Value);
            Assert.Equal(3, safetyEyes.Count);
        }

        [Fact]
        public async Task GetSafetyEyesSortedByStock_ShouldReturnSortedListByStock()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new SafetyEyeRepository(context);
            var service = new SafetyEyeService(repository);
            var controller = new SafetyEyeController(service);

            //Act
            var result = await controller.GetSafetyEyesSortedByStock();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var safetyEyes = Assert.IsType<List<SafetyEye>>(okResult.Value);
            Assert.Equal(3, safetyEyes.Count);
        }

        [Fact]
        public async Task DeleteSafetyEyesByDetails_ShouldDeleteAndReturnDeletedEntity()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new SafetyEyeRepository(context);
            var service = new SafetyEyeService(repository);
            var controller = new SafetyEyeController(service);

            var safetyEyeToDelete = new SafetyEye { SizeInMM = 6, Color = "Black", Shape = "Round" };

            //Act
            var result = await controller.DeleteSafetyEyesByDetails(safetyEyeToDelete);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var deletedSafetyEye = Assert.IsType<SafetyEye>(okResult.Value);
            Assert.Equal(6, deletedSafetyEye.SizeInMM);
            Assert.Equal("Black", deletedSafetyEye.Color);
            Assert.Equal("Round", deletedSafetyEye.Shape);
        }

        [Fact]
        public async Task DeleteSafetyEyesByDetails_ShouldReturnNotFoundWhenNotExists()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new SafetyEyeRepository(context);
            var service = new SafetyEyeService(repository);
            var controller = new SafetyEyeController(service);

            var nonExistentSafetyEye = new SafetyEye { SizeInMM = 20, Color = "Pink", Shape = "Round" };

            //Act
            var result = await controller.DeleteSafetyEyesByDetails(nonExistentSafetyEye);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Safety Eyes not found with the specified details.", notFoundResult.Value);
        }

        [Fact]
        public async Task CheckSafetyEyeExistence_ShouldReturnTrueIfExists()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new SafetyEyeRepository(context);
            var service = new SafetyEyeService(repository);
            var controller = new SafetyEyeController(service);

            //Act
            var result = await controller.CheckSafetyEyeExistence(6, "Black", "Round");

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value); //Ensure that okResult.Value is not null

            //Use reflection to access the 'exists' property
            var existsProperty = okResult.Value?.GetType().GetProperty("exists");
            Assert.NotNull(existsProperty); //Ensure the 'exists' property exists

            //Retrieve the value of the 'exists' property
            var existsValue = (bool?)existsProperty?.GetValue(okResult.Value);
            Assert.True(existsValue.GetValueOrDefault()); //Assert that 'exists' is true
        }

        [Fact]
        public async Task UpdateSafetyEyeQuantity_ShouldReturnOkWhenUpdated()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new SafetyEyeRepository(context);
            var service = new SafetyEyeService(repository);
            var controller = new SafetyEyeController(service);

            var safetyEyeToUpdate = new SafetyEye { SizeInMM = 6, Color = "Black", Shape = "Round", NumberOfEyesOwned = 60 };

            //Act
            var result = await controller.UpdateSafetyEyeQuantity(safetyEyeToUpdate);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Safety Eye quantity updated successfully.", okResult.Value);
        }
    }
}