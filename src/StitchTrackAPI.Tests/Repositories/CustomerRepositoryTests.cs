using Microsoft.EntityFrameworkCore;
using CrochetBusinessAPI.Data;
using CrochetBusinessAPI.Models;
using CrochetBusinessAPI.Repositories;
using Xunit;

namespace CrochetBusinessAPI.Tests.Repositories
{
    public class CustomerRepositoryTests
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
                    EmailAddress = "john.doe@example.com"
                },
                new Customer
                {
                    CustomerID = 2,
                    Name = "Jane Smith",
                    PhoneNumber = "987-654-3210",
                    EmailAddress = "jane.smith@example.com"
                }
            );
            context.SaveChanges();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCustomers()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);

            //Act
            var customers = await repository.GetAllAsync();

            //Assert
            Assert.NotNull(customers);
            Assert.Equal(2, customers.Count); //Should return the 2 customers in the seeded data
        }

        [Fact]
        public async Task GetCustomerByNameAsync_ShouldReturnCorrectCustomer()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);

            //Act
            var customer = await repository.GetCustomerByNameAsync("John Doe");

            //Assert
            Assert.NotNull(customer);
            Assert.Equal("John Doe", customer?.Name);
        }

        [Fact]
        public async Task GetCustomerByNameAsync_ShouldReturnNullIfNotExists()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);

            //Act
            var customer = await repository.GetCustomerByNameAsync("NonExistent Name");

            //Assert
            Assert.Null(customer); //Should return null since the customer does not exist
        }

        [Fact]
        public async Task CreateCustomerAsync_ShouldAddCustomerToDatabase()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);
            var newCustomer = new Customer
            {
                Name = "New Customer",
                PhoneNumber = "555-555-5555",
                EmailAddress = "new.customer@example.com"
            };

            //Act
            var createdCustomer = await repository.CreateCustomerAsync(newCustomer);

            //Assert
            var customers = await context.Customers.ToListAsync();
            Assert.Equal(3, customers.Count); //Initial 2 customers + 1 new customer
            Assert.NotNull(createdCustomer);
            Assert.Equal("New Customer", createdCustomer.Name);
        }

        [Fact]
        public async Task UpdateCustomerNameAsync_ShouldUpdateCustomerName()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);

            //Act
            var updatedCustomer = await repository.UpdateCustomerNameAsync(1, "John Updated");

            //Assert
            Assert.NotNull(updatedCustomer);
            Assert.Equal("John Updated", updatedCustomer?.Name);
        }

        [Fact]
        public async Task UpdateCustomerPhoneNumberAsync_ShouldUpdatePhoneNumber()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);

            //Act
            var updatedCustomer = await repository.UpdateCustomerPhoneNumberAsync(2, "111-111-1111");

            //Assert
            Assert.NotNull(updatedCustomer);
            Assert.Equal("111-111-1111", updatedCustomer?.PhoneNumber);
        }

        [Fact]
        public async Task UpdateCustomerEmailAddressAsync_ShouldUpdateEmailAddress()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);

            //Act
            var updatedCustomer = await repository.UpdateCustomerEmailAddressAsync(1, "new.email@example.com");

            //Assert
            Assert.NotNull(updatedCustomer);
            Assert.Equal("new.email@example.com", updatedCustomer?.EmailAddress);
        }

        [Fact]
        public async Task DeleteCustomerByNameAsync_ShouldDeleteCustomer()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);

            //Act
            var deletedCustomer = await repository.DeleteCustomerByNameAsync("Jane Smith");

            //Assert
            Assert.NotNull(deletedCustomer);
            Assert.Equal("Jane Smith", deletedCustomer?.Name);

            var exists = await repository.GetCustomerByNameAsync("Jane Smith");
            Assert.Null(exists); //Should no longer exist in the database
        }

        [Fact]
        public async Task DeleteCustomerByNameAsync_ShouldReturnNullIfNotExists()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);

            //Act
            var result = await repository.DeleteCustomerByNameAsync("NonExistent Name");

            //Assert
            Assert.Null(result); //Should return null since the customer does not exist
        }
    }
}