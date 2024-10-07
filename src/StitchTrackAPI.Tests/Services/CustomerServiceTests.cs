using CrochetBusinessAPI.Data; 
using CrochetBusinessAPI.Models;
using CrochetBusinessAPI.Repositories; 
using CrochetBusinessAPI.Services; 
using Microsoft.EntityFrameworkCore; 

namespace CrochetBusinessAPI.Tests.Services
{
    public class CustomerServiceTests
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
                    PhoneNumber = "123-456-7890",
                    EmailAddress = "john@example.com"
                },
                new Customer
                {
                    CustomerID = 2,
                    Name = "Jane Smith",
                    PhoneNumber = "987-654-3210",
                    EmailAddress = "jane@example.com"
                }
            );
            context.SaveChanges();
        }

        [Fact]
        public async Task GetOrCreateCustomerAsync_ShouldReturnExistingCustomer()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);
            var service = new CustomerService(repository);

            //Act
            var customer = await service.GetOrCreateCustomerAsync("John Doe");

            //Assert
            Assert.NotNull(customer);
            Assert.Equal(1, customer.CustomerID);
            Assert.Equal("John Doe", customer.Name);
        }

        [Fact]
        public async Task GetOrCreateCustomerAsync_ShouldCreateNewCustomerIfNotExists()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);
            var service = new CustomerService(repository);

            //Act
            var customer = await service.GetOrCreateCustomerAsync("New Customer");

            //Assert
            Assert.NotNull(customer);
            Assert.Equal("New Customer", customer.Name);
            Assert.Equal("N/A", customer.PhoneNumber); //Default values should be set
            Assert.Equal("N/A", customer.EmailAddress);
        }

        [Fact]
        public async Task UpdateCustomerNameAsync_ShouldUpdateCustomerName()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);
            var service = new CustomerService(repository);

            //Act
            var updatedCustomer = await service.UpdateCustomerNameAsync(1, "Johnathan Doe");

            //Assert
            Assert.NotNull(updatedCustomer);
            Assert.Equal("Johnathan Doe", updatedCustomer?.Name);
        }

        [Fact]
        public async Task UpdateCustomerPhoneNumberAsync_ShouldUpdateCustomerPhoneNumber()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);
            var service = new CustomerService(repository);

            //Act
            var updatedCustomer = await service.UpdateCustomerPhoneNumberAsync(2, "555-555-5555");

            //Assert
            Assert.NotNull(updatedCustomer);
            Assert.Equal("555-555-5555", updatedCustomer?.PhoneNumber);
        }

        [Fact]
        public async Task UpdateCustomerEmailAddressAsync_ShouldUpdateCustomerEmailAddress()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);
            var service = new CustomerService(repository);

            //Act
            var updatedCustomer = await service.UpdateCustomerEmailAddressAsync(1, "john.doe@example.com");

            //Assert
            Assert.NotNull(updatedCustomer);
            Assert.Equal("john.doe@example.com", updatedCustomer?.EmailAddress);
        }

        [Fact]
        public async Task GetCustomerByNameAsync_ShouldReturnCorrectCustomer()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);
            var service = new CustomerService(repository);

            //Act
            var customer = await service.GetCustomerByNameAsync("Jane Smith");

            //Assert
            Assert.NotNull(customer);
            Assert.Equal("Jane Smith", customer?.Name);
            Assert.Equal("987-654-3210", customer?.PhoneNumber);
            Assert.Equal("jane@example.com", customer?.EmailAddress);
        }

        [Fact]
        public async Task DeleteCustomerByNameAsync_ShouldDeleteCustomer()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);
            var service = new CustomerService(repository);

            //Act
            var deletedCustomer = await service.DeleteCustomerByNameAsync("Jane Smith");

            //Assert
            Assert.NotNull(deletedCustomer);
            Assert.Equal("Jane Smith", deletedCustomer?.Name);

            var customerInDb = await context.Customers.FindAsync(deletedCustomer?.CustomerID);
            Assert.Null(customerInDb); //Customer should be removed from the database
        }

        [Fact]
        public async Task DeleteCustomerByNameAsync_ShouldReturnNullIfCustomerDoesNotExist()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);
            var service = new CustomerService(repository);

            //Act
            var deletedCustomer = await service.DeleteCustomerByNameAsync("NonExistent Customer");

            //Assert
            Assert.Null(deletedCustomer); //No customer should be found and deleted
        }
    }
}
