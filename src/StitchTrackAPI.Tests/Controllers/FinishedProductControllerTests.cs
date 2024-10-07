using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrochetBusinessAPI.Controllers;
using CrochetBusinessAPI.Data;
using CrochetBusinessAPI.Models;
using CrochetBusinessAPI.Repositories;
using CrochetBusinessAPI.Services;

namespace CrochetBusinessAPI.Tests.Controllers
{
    public class FinishedProductControllerTests
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
            context.FinishedProducts.AddRange(
                new FinishedProduct
                {
                    FinishedProductsID = 1,
                    Name = "Hat",
                    SalePrice = 20.0m,
                    NumberInStock = 15,
                    TimeToMake = TimeSpan.FromMinutes(60),
                    TotalCostToMake = 5.0m
                },
                new FinishedProduct
                {
                    FinishedProductsID = 2,
                    Name = "Scarf",
                    SalePrice = 30.0m,
                    NumberInStock = 10,
                    TimeToMake = TimeSpan.FromMinutes(120),
                    TotalCostToMake = 8.0m
                }
            );
            context.SaveChanges(); //Save changes to the context
        }

        [Fact]
        public async Task GetAllProductNames_ShouldReturnListOfProductNames()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductRepository(context);
            var service = new FinishedProductService(repository);
            var controller = new FinishedProductController(service);

            //Act
            var result = await controller.GetAllProductNames();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result); //Assert that the result is of type OkObjectResult
            var names = Assert.IsType<List<string>>(okResult.Value); //Assert that the value is a list of strings
            Assert.Equal(2, names.Count); //Assert that there are 2 product names
            Assert.Contains("Hat", names); //Assert that "Hat" is in the list
            Assert.Contains("Scarf", names); //Assert that "Scarf" is in the list
        }

        [Fact]
        public async Task GetProductsSortedByTimeToMake_ShouldReturnProductsSortedByTime()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductRepository(context);
            var service = new FinishedProductService(repository);
            var controller = new FinishedProductController(service);

            //Act
            var result = await controller.GetProductsSortedByTimeToMake();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result); //Assert that the result is of type OkObjectResult
            var products = Assert.IsType<List<FinishedProduct>>(okResult.Value); //Assert that the value is a list of FinishedProducts
            Assert.Equal(2, products.Count); //Assert that there are 2 products
            Assert.Equal("Hat", products[0].Name); //"Hat" should come before "Scarf"
            Assert.Equal("Scarf", products[1].Name); //"Scarf" should come after "Hat"
        }

        [Fact]
        public async Task GetProductByName_ShouldReturnProduct_WhenProductExists()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductRepository(context);
            var service = new FinishedProductService(repository);
            var controller = new FinishedProductController(service);

            //Act
            var result = await controller.GetProductByName("Hat");

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result); //Assert that the result is of type OkObjectResult
            var product = Assert.IsType<FinishedProduct>(okResult.Value); //Assert that the value is a FinishedProduct
            Assert.Equal("Hat", product.Name); //Assert that the product name is "Hat"
        }

        [Fact]
        public async Task GetProductByName_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductRepository(context);
            var service = new FinishedProductService(repository);
            var controller = new FinishedProductController(service);

            //Act
            var result = await controller.GetProductByName("NonExistentProduct");

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var responseValue = notFoundResult.Value;
            Assert.NotNull(responseValue);

            //Check if the anonymous type contains the expected properties
            Assert.Equal("Product not found", responseValue.GetType().GetProperty("message")?.GetValue(responseValue));
        }

        [Fact]
        public async Task DeleteProductByName_ShouldReturnDeletedProduct_WhenProductExists()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductRepository(context);
            var service = new FinishedProductService(repository);
            var controller = new FinishedProductController(service);

            //Act
            var result = await controller.DeleteProductByName("Hat");

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result); //Assert that the result is of type OkObjectResult
            var deletedProduct = Assert.IsType<FinishedProduct>(okResult.Value); //Assert that the value is a FinishedProduct
            Assert.Equal("Hat", deletedProduct.Name); //Assert that the deleted product is "Hat"
        }

        [Fact]
        public async Task DeleteProductByName_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductRepository(context);
            var service = new FinishedProductService(repository);
            var controller = new FinishedProductController(service);

            //Act
            var result = await controller.DeleteProductByName("NonExistentProduct");

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result); //Assert that the result is of type NotFoundObjectResult
            Assert.Equal("Product not found.", notFoundResult.Value); //Assert that the error message is "Product not found."
        }

        [Fact]
        public async Task GetProductsSortedBySalePrice_ShouldReturnSortedProducts()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var service = new FinishedProductService(new FinishedProductRepository(context));
            var controller = new FinishedProductController(service);

            //Act
            var result = await controller.GetProductsSortedBySalePrice();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var products = Assert.IsType<List<FinishedProduct>>(okResult.Value);
            Assert.Equal(2, products.Count);
            Assert.Equal("Hat", products[0].Name); //Hat ($20) should come before Scarf ($30)
        }

        [Fact]
        public async Task GetProductsSortedByNumberInStock_ShouldReturnSortedProducts()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var service = new FinishedProductService(new FinishedProductRepository(context));
            var controller = new FinishedProductController(service);

            //Act
            var result = await controller.GetProductsSortedByNumberInStock();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var products = Assert.IsType<List<FinishedProduct>>(okResult.Value);
            Assert.Equal(2, products.Count);
            Assert.Equal("Scarf", products[0].Name); //Hat (15 in stock) should come after Scarf (10 in stock)
        }

        [Fact]
        public async Task GetProductIdByName_ShouldReturnProductIdIfExists()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var service = new FinishedProductService(new FinishedProductRepository(context));
            var controller = new FinishedProductController(service);

            //Act
            var result = await controller.GetProductIdByName("Hat");

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result); //Check if the result is of type OkObjectResult
            Assert.NotNull(okResult.Value); //Ensure that OkObjectResult has a value

            var productIdObject = okResult.Value?.GetType()?.GetProperty("productId")?.GetValue(okResult.Value, null); //Safely access the property
            Assert.NotNull(productIdObject); //Ensure the value is not null
            Assert.Equal(1, (int)productIdObject!); //Check if the ID matches the expected value (use '!' to suppress the warning)
        }

        [Fact]
        public async Task UpdateSalePrice_ShouldUpdateProductSalePrice()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var service = new FinishedProductService(new FinishedProductRepository(context));
            var controller = new FinishedProductController(service);
            var updatedProduct = new FinishedProduct { Name = "Hat", SalePrice = 25.0m };

            //Act
            var result = await controller.UpdateSalePrice(updatedProduct);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var product = Assert.IsType<FinishedProduct>(okResult.Value);
            Assert.Equal(25.0m, product.SalePrice); //Sale price should be updated to 25.0
        }

        [Fact]
        public async Task UpdateProductTime_ShouldUpdateTimeToMake()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var service = new FinishedProductService(new FinishedProductRepository(context));
            var controller = new FinishedProductController(service);
            var updatedProduct = new FinishedProduct { Name = "Hat", TimeToMake = TimeSpan.FromMinutes(90) };

            //Act
            var result = await controller.UpdateProductTime(updatedProduct);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var product = Assert.IsType<FinishedProduct>(okResult.Value);
            Assert.Equal(TimeSpan.FromMinutes(90), product.TimeToMake); //TimeToMake should be updated to 90 minutes
        }

        [Fact]
        public async Task CheckFinishedProductExistence_ShouldReturnExistenceStatus()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var service = new FinishedProductService(new FinishedProductRepository(context));
            var controller = new FinishedProductController(service);

            //Act
            var result = await controller.CheckFinishedProductExistence("Hat");

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value); //Ensure that the value is not null before accessing it

            var existsProperty = okResult.Value?.GetType().GetProperty("exists");
            Assert.NotNull(existsProperty); //Ensure the property exists

            var exists = existsProperty?.GetValue(okResult.Value);
            Assert.NotNull(exists); //Ensure the "exists" property is not null

            Assert.IsType<bool>(exists);
            Assert.True((bool)exists);
        }

        [Fact]
        public async Task UpdateFinishedProductQuantity_ShouldUpdateQuantity()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var service = new FinishedProductService(new FinishedProductRepository(context));
            var controller = new FinishedProductController(service);
            var updatedProductDetails = new FinishedProduct { Name = "Hat", NumberInStock = 20 };

            //Act
            var result = await controller.UpdateFinishedProductQuantity(updatedProductDetails);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Finished Product quantity updated successfully.", okResult.Value);
            var product = await context.FinishedProducts.FindAsync(1);
            Assert.Equal(20, product?.NumberInStock); //Stock quantity should be updated to 20
        }

        [Fact]
        public async Task GetProductsSortedByCostToMake_ShouldReturnProductsSortedByCost()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new FinishedProductRepository(context);
            var service = new FinishedProductService(repository);
            var controller = new FinishedProductController(service);

            //Act
            var result = await controller.GetProductsSortedByCostToMake();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result); //Assert that the result is of type OkObjectResult
            Assert.Equal(200, okResult.StatusCode); //Assert that the status code is 200 OK

            var returnedProducts = Assert.IsType<List<FinishedProduct>>(okResult.Value); //Assert that the returned value is a list of FinishedProduct
            Assert.Equal(2, returnedProducts.Count); //Ensure there are 2 products in the response

            //Verify that the products are sorted by `TotalCostToMake` in ascending order
            Assert.Equal("Hat", returnedProducts[0].Name); //Hat should be first as its TotalCostToMake is 8.0
            Assert.Equal("Scarf", returnedProducts[1].Name);   //Scarf should be second as its TotalCostToMake is 15.0
        }
    }
}