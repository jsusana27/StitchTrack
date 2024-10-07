using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrochetBusinessAPI.Controllers;
using CrochetBusinessAPI.Data;
using CrochetBusinessAPI.Models;
using CrochetBusinessAPI.Repositories;
using CrochetBusinessAPI.Services;

namespace CrochetBusinessAPI.Tests.Controllers
{
    public class FinishedProductMaterialControllerTests
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
            //Seed Finished Products
            context.FinishedProducts.AddRange(
                new FinishedProduct { FinishedProductsID = 1, Name = "Hat", SalePrice = 20.0m, NumberInStock = 15 },
                new FinishedProduct { FinishedProductsID = 2, Name = "Scarf", SalePrice = 30.0m, NumberInStock = 10 }
            );

            //Seed Yarn
            context.Yarns.AddRange(
                new Yarn { YarnID = 1, Brand = "BrandA", FiberType = "Wool", FiberWeight = 2, Color = "Red", NumberOfSkeinsOwned = 10 },
                new Yarn { YarnID = 2, Brand = "BrandB", FiberType = "Cotton", FiberWeight = 3, Color = "Blue", NumberOfSkeinsOwned = 20 }
            );

            //Seed Safety Eyes
            context.SafetyEyes.AddRange(
                new SafetyEye { SafetyEyesID = 1, SizeInMM = 6, Color = "Black", Shape = "Round", NumberOfEyesOwned = 50 },
                new SafetyEye { SafetyEyesID = 2, SizeInMM = 12, Color = "Green", Shape = "Oval", NumberOfEyesOwned = 30 }
            );

            //Seed Stuffing
            context.Stuffings.AddRange(
                new Stuffing { StuffingID = 1, Brand = "StuffBrand", Type = "Polyester", PricePerFivelbs = 5 }
            );

            //Seed Finished Product Materials
            context.FinishedProductMaterials.AddRange(
                new FinishedProductMaterial
                {
                    FinishedProductMaterialsID = 1,
                    FinishedProductsID = 1,
                    MaterialType = "Yarn",
                    MaterialID = 1,
                    QuantityUsed = 2.5m
                },
                new FinishedProductMaterial
                {
                    FinishedProductMaterialsID = 2,
                    FinishedProductsID = 2,
                    MaterialType = "SafetyEyes",
                    MaterialID = 1,
                    QuantityUsed = 1.0m
                },
                new FinishedProductMaterial
                {
                    FinishedProductMaterialsID = 3,
                    FinishedProductsID = 1,
                    MaterialType = "Stuffing",
                    MaterialID = 1,
                    QuantityUsed = 1.0m
                }
            );

            context.SaveChanges(); //Commit changes to the context
        }

        [Fact]
        public async Task AddYarnMaterialToFinishedProduct_ShouldAddMaterialSuccessfully()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductMaterialRepository(context);
            var finishedProductService = new FinishedProductService(new FinishedProductRepository(context));
            var yarnService = new YarnService(new YarnRepository(context));
            
            //Create FinishedProductMaterialService
            var finishedProductMaterialService = new FinishedProductMaterialService(repository);
            
            var controller = new FinishedProductMaterialController(
                finishedProductMaterialService, finishedProductService, yarnService, null!, null!);

            //Act
            var result = await controller.AddYarnMaterialToFinishedProduct("Hat", "BrandA", "Wool", 2, "Red", 3.0m);

            //Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var material = Assert.IsType<FinishedProductMaterial>(createdResult.Value);
            Assert.Equal("Yarn", material.MaterialType);
            Assert.Equal(3.0m, material.QuantityUsed);
        }

        [Fact]
        public async Task AddSafetyEyesMaterialToFinishedProduct_ShouldAddMaterialSuccessfully()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            
            var repository = new FinishedProductMaterialRepository(context);
            var finishedProductService = new FinishedProductService(new FinishedProductRepository(context));
            var yarnService = new YarnService(new YarnRepository(context));
            var safetyEyeService = new SafetyEyeService(new SafetyEyeRepository(context)); //Initialize SafetyEyeService

            //Create FinishedProductMaterialService
            var finishedProductMaterialService = new FinishedProductMaterialService(repository);

            var controller = new FinishedProductMaterialController(
                finishedProductMaterialService, finishedProductService, yarnService, safetyEyeService, null!);

            //Act
            var result = await controller.AddSafetyEyesMaterialToFinishedProduct("Scarf", 6, "Black", "Round", 1.5m);

            //Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var material = Assert.IsType<FinishedProductMaterial>(createdResult.Value);
            Assert.Equal("SafetyEyes", material.MaterialType);
            Assert.Equal(1.5m, material.QuantityUsed);
        }

        [Fact]
        public async Task GetFinishedProductsByMaterial_ShouldReturnCorrectProducts()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductMaterialRepository(context);
            var finishedProductService = new FinishedProductService(new FinishedProductRepository(context));
            var yarnService = new YarnService(new YarnRepository(context));
            
            //Create FinishedProductMaterialService
            var finishedProductMaterialService = new FinishedProductMaterialService(repository);
            
            var controller = new FinishedProductMaterialController(
                finishedProductMaterialService, finishedProductService, yarnService, null!, null!);

            //Act
            var result = await controller.GetFinishedProductsByMaterial("Yarn", 1);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var products = Assert.IsType<List<FinishedProduct>>(okResult.Value);
            Assert.Single(products);
            Assert.Equal("Hat", products[0].Name);
        }

        [Fact]
        public async Task DeleteYarnMaterial_ShouldDeleteMaterialSuccessfully()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductMaterialRepository(context);
            var finishedProductService = new FinishedProductService(new FinishedProductRepository(context));
            var yarnService = new YarnService(new YarnRepository(context));
            
            //Create FinishedProductMaterialService
            var finishedProductMaterialService = new FinishedProductMaterialService(repository);
            
            var controller = new FinishedProductMaterialController(
                finishedProductMaterialService, finishedProductService, yarnService, null!, null!);

            //Act
            var result = await controller.DeleteYarnMaterial("Hat", "BrandA", "Wool", 2, "Red");

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Yarn material deleted successfully.", okResult.Value);
        }

        [Fact]
        public async Task UpdateYarnQuantity_ShouldUpdateSuccessfully()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductMaterialRepository(context);
            var finishedProductService = new FinishedProductService(new FinishedProductRepository(context));
            var yarnService = new YarnService(new YarnRepository(context));
            
            //Create FinishedProductMaterialService
            var finishedProductMaterialService = new FinishedProductMaterialService(repository);
            
            var controller = new FinishedProductMaterialController(
                finishedProductMaterialService, finishedProductService, yarnService, null!, null!);

            //Act
            var result = await controller.UpdateYarnQuantity("Hat", "BrandA", "Wool", 2, "Red", 5.0m);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Yarn quantity updated successfully.", okResult.Value);
        }

        [Fact]
        public async Task DeleteSafetyEyesMaterial_ShouldReturnOkWhenDeleted()
        {
            //Arrange
            using var context = GetInMemoryDbContext();

            var repository = new FinishedProductMaterialRepository(context);
            var finishedProductService = new FinishedProductService(new FinishedProductRepository(context));
            var yarnService = new YarnService(new YarnRepository(context));
            var safetyEyeService = new SafetyEyeService(new SafetyEyeRepository(context));
            var finishedProductMaterialService = new FinishedProductMaterialService(repository);

            var controller = new FinishedProductMaterialController(
                finishedProductMaterialService, finishedProductService, yarnService, safetyEyeService, null!);

            //Act
            var result = await controller.DeleteSafetyEyesMaterial("Scarf", 6, "Black", "Round");

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Safety Eyes deleted successfully.", okResult.Value);
        }

        [Fact]
        public async Task DeleteSafetyEyesMaterial_ShouldReturnNotFoundWhenProductNotFound()
        {
            //Arrange
            using var context = GetInMemoryDbContext();

            var repository = new FinishedProductMaterialRepository(context);
            var finishedProductService = new FinishedProductService(new FinishedProductRepository(context));
            var yarnService = new YarnService(new YarnRepository(context));
            var safetyEyeService = new SafetyEyeService(new SafetyEyeRepository(context));
            var finishedProductMaterialService = new FinishedProductMaterialService(repository);

            var controller = new FinishedProductMaterialController(
                finishedProductMaterialService, finishedProductService, yarnService, safetyEyeService, null!);

            //Act
            var result = await controller.DeleteSafetyEyesMaterial("NonExistentProduct", 10, "Black", "Round");

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Product 'NonExistentProduct' not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task DeleteSafetyEyesMaterial_ShouldReturnNotFoundWhenSafetyEyesNotFound()
        {
            //Arrange
            using var context = GetInMemoryDbContext();

            var repository = new FinishedProductMaterialRepository(context);
            var finishedProductService = new FinishedProductService(new FinishedProductRepository(context));
            var yarnService = new YarnService(new YarnRepository(context));
            var safetyEyeService = new SafetyEyeService(new SafetyEyeRepository(context));
            var finishedProductMaterialService = new FinishedProductMaterialService(repository);

            var controller = new FinishedProductMaterialController(
                finishedProductMaterialService, finishedProductService, yarnService, safetyEyeService, null!);

            //Act
            var result = await controller.DeleteSafetyEyesMaterial("Hat", 15, "Purple", "Square"); //Non-existent Safety Eye

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Matching Safety Eyes not found based on the provided specifications.", notFoundResult.Value);
        }

        [Fact]
        public async Task DeleteStuffingFromProduct_ShouldReturnOkWhenDeleted()
        {
            //Arrange
            using var context = GetInMemoryDbContext();

            var repository = new FinishedProductMaterialRepository(context);
            var finishedProductService = new FinishedProductService(new FinishedProductRepository(context));
            var yarnService = new YarnService(new YarnRepository(context));
            var stuffingService = new StuffingService(new StuffingRepository(context));
            var finishedProductMaterialService = new FinishedProductMaterialService(repository);

            var controller = new FinishedProductMaterialController(
                finishedProductMaterialService, finishedProductService, yarnService, null!, stuffingService);

            //Act
            var result = await controller.DeleteStuffingFromProduct("Hat", "StuffBrand", "Polyester");

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Stuffing deleted successfully from the product.", okResult.Value);
        }

        [Fact]
        public async Task DeleteStuffingFromProduct_ShouldReturnNotFoundWhenProductNotFound()
        {
            //Arrange
            using var context = GetInMemoryDbContext();

            var repository = new FinishedProductMaterialRepository(context);
            var finishedProductService = new FinishedProductService(new FinishedProductRepository(context));
            var yarnService = new YarnService(new YarnRepository(context));
            var stuffingService = new StuffingService(new StuffingRepository(context));
            var finishedProductMaterialService = new FinishedProductMaterialService(repository);

            var controller = new FinishedProductMaterialController(
                finishedProductMaterialService, finishedProductService, yarnService, null!, stuffingService);

            //Act
            var result = await controller.DeleteStuffingFromProduct("NonExistentProduct", "BrandA", "Polyester");

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Finished product 'NonExistentProduct' not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task DeleteStuffingFromProduct_ShouldReturnNotFoundWhenStuffingNotFound()
        {
            //Arrange
            using var context = GetInMemoryDbContext();

            var repository = new FinishedProductMaterialRepository(context);
            var finishedProductService = new FinishedProductService(new FinishedProductRepository(context));
            var yarnService = new YarnService(new YarnRepository(context));
            var stuffingService = new StuffingService(new StuffingRepository(context));
            var finishedProductMaterialService = new FinishedProductMaterialService(repository);

            var controller = new FinishedProductMaterialController(
                finishedProductMaterialService, finishedProductService, yarnService, null!, stuffingService);

            //Act
            var result = await controller.DeleteStuffingFromProduct("Hat", "BrandZ", "Foam"); //Non-existent Stuffing

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Matching Stuffing not found based on the provided specifications.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetMaterialsByFinishedProduct_ShouldReturnMaterialsByName()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductMaterialRepository(context);
            var finishedProductService = new FinishedProductService(new FinishedProductRepository(context));
            var finishedProductMaterialService = new FinishedProductMaterialService(repository);
            var controller = new FinishedProductMaterialController(finishedProductMaterialService, finishedProductService, null!, null!, null!);

            //Act
            var result = await controller.GetMaterialsByFinishedProduct("Hat", null);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var materials = Assert.IsType<List<FinishedProductMaterial>>(okResult.Value);
            Assert.Equal(2, materials.Count);

        }

        [Fact]
        public async Task GetMaterialsByFinishedProduct_ShouldReturnBadRequestIfNoParametersProvided()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductMaterialRepository(context);
            var finishedProductService = new FinishedProductService(new FinishedProductRepository(context));
            var finishedProductMaterialService = new FinishedProductMaterialService(repository);
            var controller = new FinishedProductMaterialController(finishedProductMaterialService, finishedProductService, null!, null!, null!);

            //Act
            var result = await controller.GetMaterialsByFinishedProduct(null, null);

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Please provide either a FinishedProductName or a FinishedProductID.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetMaterialsByFinishedProduct_ShouldReturnNotFoundIfProductDoesNotExist()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductMaterialRepository(context);
            var finishedProductService = new FinishedProductService(new FinishedProductRepository(context));
            var finishedProductMaterialService = new FinishedProductMaterialService(repository);
            var controller = new FinishedProductMaterialController(finishedProductMaterialService, finishedProductService, null!, null!, null!);

            //Act
            var result = await controller.GetMaterialsByFinishedProduct("NonExistentProduct", null);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Finished product 'NonExistentProduct' not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task CanMakeProduct_ShouldReturnTrueIfEnoughMaterial()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductMaterialRepository(context);
            var finishedProductService = new FinishedProductService(new FinishedProductRepository(context));
            var finishedProductMaterialService = new FinishedProductMaterialService(repository);
            var controller = new FinishedProductMaterialController(finishedProductMaterialService, finishedProductService, null!, null!, null!);

            //Act
            var result = await controller.CanMakeProduct(1, 2.0m); //Check if 2 units of Yarn (MaterialID = 1) can be used

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value); //Ensure value is not null before unboxing
            Assert.True((bool)okResult.Value); //Safely cast and check
        }

        [Fact]
        public async Task GetDetailedMaterialsByProductName_ShouldReturnMaterials()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductMaterialRepository(context);
            var finishedProductService = new FinishedProductService(new FinishedProductRepository(context));
            var finishedProductMaterialService = new FinishedProductMaterialService(repository);
            var controller = new FinishedProductMaterialController(finishedProductMaterialService, finishedProductService, null!, null!, null!);

            //Act
            var result = await controller.GetDetailedMaterialsByProductName("Hat");

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var materials = Assert.IsType<List<object>>(okResult.Value);
            Assert.Equal(2, materials.Count);
        }

        [Fact]
        public async Task GetDetailedMaterialsByProductName_ShouldReturnNotFoundIfNoMaterials()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductMaterialRepository(context);
            var finishedProductService = new FinishedProductService(new FinishedProductRepository(context));
            var finishedProductMaterialService = new FinishedProductMaterialService(repository);
            var controller = new FinishedProductMaterialController(finishedProductMaterialService, finishedProductService, null!, null!, null!);

            //Act
            var result = await controller.GetDetailedMaterialsByProductName("NonExistentProduct");

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No materials found for product NonExistentProduct", notFoundResult.Value);
        }

                [Fact]
        public async Task UpdateSafetyEyesQuantity_ShouldReturnOkWhenUpdatedSuccessfully()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductMaterialRepository(context);
            var finishedProductService = new FinishedProductService(new FinishedProductRepository(context));
            var safetyEyeService = new SafetyEyeService(new SafetyEyeRepository(context));
            var finishedProductMaterialService = new FinishedProductMaterialService(repository);

            var controller = new FinishedProductMaterialController(finishedProductMaterialService, finishedProductService, null!, safetyEyeService, null!);

            //Act
            var result = await controller.UpdateSafetyEyesQuantity("Scarf", 6, "Black", "Round", 5.0m);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Safety Eyes quantity updated successfully.", okResult.Value);
        }

        [Fact]
        public async Task UpdateSafetyEyesQuantity_ShouldReturnNotFoundIfFinishedProductDoesNotExist()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductMaterialRepository(context);
            var finishedProductService = new FinishedProductService(new FinishedProductRepository(context));
            var safetyEyeService = new SafetyEyeService(new SafetyEyeRepository(context));
            var finishedProductMaterialService = new FinishedProductMaterialService(repository);

            var controller = new FinishedProductMaterialController(finishedProductMaterialService, finishedProductService, null!, safetyEyeService, null!);

            //Act
            var result = await controller.UpdateSafetyEyesQuantity("NonExistentProduct", 6, "Black", "Round", 5.0m);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Product 'NonExistentProduct' not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task UpdateSafetyEyesQuantity_ShouldReturnNotFoundIfSafetyEyesDoesNotExist()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductMaterialRepository(context);
            var finishedProductService = new FinishedProductService(new FinishedProductRepository(context));
            var safetyEyeService = new SafetyEyeService(new SafetyEyeRepository(context));
            var finishedProductMaterialService = new FinishedProductMaterialService(repository);

            var controller = new FinishedProductMaterialController(finishedProductMaterialService, finishedProductService, null!, safetyEyeService, null!);

            //Act
            var result = await controller.UpdateSafetyEyesQuantity("Hat", 10, "Red", "Square", 3.0m); //Use non-existent Safety Eyes specifications

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Matching Safety Eyes not found based on the provided specifications.", notFoundResult.Value);
        }

        [Fact]
        public async Task UpdateSafetyEyesQuantity_ShouldReturnInternalServerErrorIfUpdateFails()
        {
            //Arrange
            using var context = GetInMemoryDbContext();

            var repository = new FinishedProductMaterialRepository(context);
            var finishedProductService = new FinishedProductService(new FinishedProductRepository(context));
            var safetyEyeService = new SafetyEyeService(new SafetyEyeRepository(context));
            var finishedProductMaterialService = new FinishedProductMaterialService(repository);

            var controller = new FinishedProductMaterialController(finishedProductMaterialService, finishedProductService, null!, safetyEyeService, null!);

            //Act
            var result = await controller.UpdateSafetyEyesQuantity("Hat", 6, "Black", "Round", 5.0m);

            //Assert
            var errorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, errorResult.StatusCode);
            Assert.Equal("Failed to update the quantity. Please try again.", errorResult.Value);
        }

                [Fact]
        public async Task AddStuffingMaterialToFinishedProduct_ShouldAddMaterialSuccessfully()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductMaterialRepository(context);
            var finishedProductService = new FinishedProductService(new FinishedProductRepository(context));
            var stuffingService = new StuffingService(new StuffingRepository(context));
            var finishedProductMaterialService = new FinishedProductMaterialService(repository);

            var controller = new FinishedProductMaterialController(
                finishedProductMaterialService, finishedProductService, null!, null!, stuffingService);

            //Act
            var result = await controller.AddStuffingMaterialToFinishedProduct("Hat", "StuffBrand", "Polyester", 2.0m);

            //Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var material = Assert.IsType<FinishedProductMaterial>(createdResult.Value);
            Assert.Equal("Stuffing", material.MaterialType);
            Assert.Equal(2.0m, material.QuantityUsed);
        }

        [Fact]
        public async Task AddStuffingMaterialToFinishedProduct_ShouldReturnNotFoundIfFinishedProductDoesNotExist()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductMaterialRepository(context);
            var finishedProductService = new FinishedProductService(new FinishedProductRepository(context));
            var stuffingService = new StuffingService(new StuffingRepository(context));
            var finishedProductMaterialService = new FinishedProductMaterialService(repository);

            var controller = new FinishedProductMaterialController(
                finishedProductMaterialService, finishedProductService, null!, null!, stuffingService);

            //Act
            var result = await controller.AddStuffingMaterialToFinishedProduct("NonExistentProduct", "StuffBrand", "Polyester", 2.0m);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Finished product 'NonExistentProduct' not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task AddStuffingMaterialToFinishedProduct_ShouldReturnNotFoundIfStuffingDoesNotExist()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductMaterialRepository(context);
            var finishedProductService = new FinishedProductService(new FinishedProductRepository(context));
            var stuffingService = new StuffingService(new StuffingRepository(context));
            var finishedProductMaterialService = new FinishedProductMaterialService(repository);

            var controller = new FinishedProductMaterialController(
                finishedProductMaterialService, finishedProductService, null!, null!, stuffingService);

            //Act
            var result = await controller.AddStuffingMaterialToFinishedProduct("Hat", "NonExistentBrand", "Polyester", 2.0m);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Matching Stuffing not found based on the provided specifications.", notFoundResult.Value);
        }
    }
}
