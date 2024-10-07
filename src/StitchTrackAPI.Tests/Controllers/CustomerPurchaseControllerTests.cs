using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrochetBusinessAPI.Controllers;
using CrochetBusinessAPI.Data;
using CrochetBusinessAPI.Models;
using CrochetBusinessAPI.Repositories;
using CrochetBusinessAPI.Services;

namespace CrochetBusinessAPI.Tests.Controllers
{
    public class CustomerPurchaseControllerTests
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
            //Seed Customers
            context.Customers.AddRange(
                new Customer
                {
                    CustomerID = 1,
                    Name = "John Doe",
                    PhoneNumber = "0123456789",
                    EmailAddress = "john@example.com"
                },
                new Customer
                {
                    CustomerID = 2,
                    Name = "Jane Doe",
                    PhoneNumber = "9876543210",
                    EmailAddress = "jane@example.com"
                }
            );

            //Seed FinishedProducts
            context.FinishedProducts.AddRange(
                new FinishedProduct
                {
                    FinishedProductsID = 1,
                    Name = "Hat",
                    SalePrice = 20.0m,
                    NumberInStock = 15
                },
                new FinishedProduct
                {
                    FinishedProductsID = 2,
                    Name = "Scarf",
                    SalePrice = 25.0m,
                    NumberInStock = 10
                }
            );

            //Seed CustomerPurchases
            context.CustomerPurchases.AddRange(
                new CustomerPurchase
                {
                    CustomerID = 1,
                    FinishedProductsID = 1
                },
                new CustomerPurchase
                {
                    CustomerID = 1,
                    FinishedProductsID = 2
                },
                new CustomerPurchase
                {
                    CustomerID = 2,
                    FinishedProductsID = 1
                }
            );

            context.SaveChanges(); //Commit changes to the context
        }

        [Fact]
        public async Task GetPurchasesByCustomerId_ShouldReturnPurchasesForGivenCustomerId()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerPurchaseRepository(context);
            var service = new CustomerPurchaseService(repository);
            var controller = new CustomerPurchaseController(service);

            //Act
            var result = await controller.GetPurchasesByCustomerId(1); //Get purchases for CustomerID = 1

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result); //Verify that the result is of type OkObjectResult
            var purchases = Assert.IsType<List<CustomerPurchase>>(okResult.Value); //Extract the list of purchases
            Assert.Equal(2, purchases.Count); //CustomerID 1 should have 2 purchases
        }

        [Fact]
        public async Task GetCustomersByFinishedProductId_ShouldReturnCustomersForGivenProductId()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerPurchaseRepository(context);
            var service = new CustomerPurchaseService(repository);
            var controller = new CustomerPurchaseController(service);

            //Act
            var result = await controller.GetCustomersByFinishedProductId(1); //Get customers for FinishedProductID = 1

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result); //Verify that the result is of type OkObjectResult
            var customers = Assert.IsType<List<Customer>>(okResult.Value); //Extract the list of customers
            Assert.Equal(2, customers.Count); //FinishedProductID 1 should have 2 customers
        }

        [Fact]
        public async Task GetPurchasesByCustomerId_ShouldReturnEmptyListForNonExistentCustomer()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerPurchaseRepository(context);
            var service = new CustomerPurchaseService(repository);
            var controller = new CustomerPurchaseController(service);

            //Act
            var result = await controller.GetPurchasesByCustomerId(999); //Use a non-existent CustomerID

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result); //Verify that the result is of type OkObjectResult
            var purchases = Assert.IsType<List<CustomerPurchase>>(okResult.Value); //Extract the list of purchases
            Assert.Empty(purchases); //List should be empty since no purchases exist for this CustomerID
        }

        [Fact]
        public async Task GetCustomersByFinishedProductId_ShouldReturnEmptyListForNonExistentProduct()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerPurchaseRepository(context);
            var service = new CustomerPurchaseService(repository);
            var controller = new CustomerPurchaseController(service);

            //Act
            var result = await controller.GetCustomersByFinishedProductId(999); //Use a non-existent FinishedProductID

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result); //Verify that the result is of type OkObjectResult
            var customers = Assert.IsType<List<Customer>>(okResult.Value); //Extract the list of customers
            Assert.Empty(customers); //List should be empty since no customers exist for this FinishedProductID
        }
    }
}