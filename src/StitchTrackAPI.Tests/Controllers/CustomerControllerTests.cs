using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrochetBusinessAPI.Data;
using CrochetBusinessAPI.Models;
using CrochetBusinessAPI.Repositories;
using CrochetBusinessAPI.Services;
using CrochetBusinessAPI.Controllers;

namespace CrochetBusinessAPI.Tests.Controllers
{
    public class CustomerControllerTests
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
            context.SaveChanges();
        }

        [Fact]
        public async Task GetAllCustomers_ShouldReturnAllCustomers()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);
            var service = new CustomerService(repository);
            var controller = new CustomerController(service);

            //Act
            var result = await controller.GetAllCustomers();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var customers = Assert.IsType<List<Customer>>(okResult.Value);
            Assert.Equal(2, customers.Count); //Should return 2 customers from the seeded data
        }

        [Fact]
        public async Task GetCustomerByName_ShouldReturnCustomer_WhenCustomerExists()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);
            var service = new CustomerService(repository);
            var controller = new CustomerController(service);

            //Act
            var result = await controller.GetCustomerByName("John Doe");

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var customer = Assert.IsType<Customer>(okResult.Value);
            Assert.Equal("John Doe", customer.Name);
            Assert.Equal("john@example.com", customer.EmailAddress);
        }

        [Fact]
        public async Task GetCustomerByName_ShouldReturnNotFound_WhenCustomerDoesNotExist()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);
            var service = new CustomerService(repository);
            var controller = new CustomerController(service);

            //Act
            var result = await controller.GetCustomerByName("NonExistent Customer");

            //Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task UpdateCustomerName_ShouldReturnUpdatedCustomer_WhenCustomerExists()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);
            var service = new CustomerService(repository);
            var controller = new CustomerController(service);

            //Act
            var result = await controller.UpdateCustomerName(1, "John Smith");

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var updatedCustomer = Assert.IsType<Customer>(okResult.Value);
            Assert.Equal(1, updatedCustomer.CustomerID);
            Assert.Equal("John Smith", updatedCustomer.Name);
        }

        [Fact]
        public async Task UpdateCustomerName_ShouldReturnNotFound_WhenCustomerDoesNotExist()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);
            var service = new CustomerService(repository);
            var controller = new CustomerController(service);

            //Act
            var result = await controller.UpdateCustomerName(999, "NonExistent Name");

            //Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task DeleteCustomerByName_ShouldReturnDeletedCustomer_WhenCustomerExists()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);
            var service = new CustomerService(repository);
            var controller = new CustomerController(service);

            //Act
            var result = await controller.DeleteCustomerByName("John Doe");

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var deletedCustomer = Assert.IsType<Customer>(okResult.Value);
            Assert.Equal("John Doe", deletedCustomer.Name);
            Assert.Equal("john@example.com", deletedCustomer.EmailAddress);

            var remainingCustomers = await context.Customers.ToListAsync();
            Assert.Single(remainingCustomers); //Only 1 customer should remain
        }

        [Fact]
        public async Task DeleteCustomerByName_ShouldReturnNotFound_WhenCustomerDoesNotExist()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);
            var service = new CustomerService(repository);
            var controller = new CustomerController(service);

            //Act
            var result = await controller.DeleteCustomerByName("NonExistent Customer");

            //Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task UpdateCustomerDetails_ShouldUpdateCustomerDetailsSuccessfully()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);
            var service = new CustomerService(repository);
            var controller = new CustomerController(service);
            var updatedCustomer = new Customer
            {
                Name = "John Doe",
                PhoneNumber = "1112223333",
                EmailAddress = "johnsmith@example.com"
            };

            //Act
            var result = await controller.UpdateCustomerDetails(updatedCustomer);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var updatedEntity = Assert.IsType<Customer>(okResult.Value);
            Assert.Equal("1112223333", updatedEntity.PhoneNumber);
            Assert.Equal("johnsmith@example.com", updatedEntity.EmailAddress);
        }


        [Fact]
        public async Task UpdateCustomerPhone_ShouldReturnUpdatedCustomer_WhenCustomerExists()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);
            var service = new CustomerService(repository);
            var controller = new CustomerController(service);

            //Act
            var result = await controller.UpdateCustomerPhone(1, "111222333");

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var updatedCustomer = Assert.IsType<Customer>(okResult.Value);
            Assert.Equal(1, updatedCustomer.CustomerID);
            Assert.Equal("111222333", updatedCustomer.PhoneNumber);
        }

        [Fact]
        public async Task UpdateCustomerPhone_ShouldReturnNotFound_WhenCustomerDoesNotExist()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);
            var service = new CustomerService(repository);
            var controller = new CustomerController(service);

            //Act
            var result = await controller.UpdateCustomerPhone(999, "111222333");

            //Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task UpdateCustomerEmail_ShouldReturnUpdatedCustomer_WhenCustomerExists()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);
            var service = new CustomerService(repository);
            var controller = new CustomerController(service);

            //Act
            var result = await controller.UpdateCustomerEmail(1, "newemail@example.com");

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var updatedCustomer = Assert.IsType<Customer>(okResult.Value);
            Assert.Equal(1, updatedCustomer.CustomerID);
            Assert.Equal("newemail@example.com", updatedCustomer.EmailAddress);
        }

        [Fact]
        public async Task UpdateCustomerEmail_ShouldReturnNotFound_WhenCustomerDoesNotExist()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);
            var service = new CustomerService(repository);
            var controller = new CustomerController(service);

            //Act
            var result = await controller.UpdateCustomerEmail(999, "nonexistent@example.com");

            //Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
    }
}
