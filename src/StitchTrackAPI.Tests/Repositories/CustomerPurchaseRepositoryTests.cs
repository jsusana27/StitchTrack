using Microsoft.EntityFrameworkCore;
using CrochetBusinessAPI.Models;
using CrochetBusinessAPI.Data;
using CrochetBusinessAPI.Repositories;

namespace CrochetBusinessAPI.Tests.Repositories
{
    public class CustomerPurchaseRepositoryTests
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
            var customer1 = new Customer
            {
                CustomerID = 1,
                Name = "John Doe",
                PhoneNumber = "123-456-7890",
                EmailAddress = "john.doe@example.com"
            };

            var customer2 = new Customer
            {
                CustomerID = 2,
                Name = "Jane Smith",
                PhoneNumber = "987-654-3210",
                EmailAddress = "jane.smith@example.com"
            };

            var product1 = new FinishedProduct
            {
                FinishedProductsID = 1,
                Name = "Hat",
                TotalCostToMake = 8.99m,
                SalePrice = 15.00m,
                NumberInStock = 10
            };

            var product2 = new FinishedProduct
            {
                FinishedProductsID = 2,
                Name = "Scarf",
                TotalCostToMake = 5.99m,
                SalePrice = 10.00m,
                NumberInStock = 5
            };

            context.Customers.AddRange(customer1, customer2);
            context.FinishedProducts.AddRange(product1, product2);

            context.CustomerPurchases.AddRange(
                new CustomerPurchase
                {
                    CustomerID = 1,
                    FinishedProductsID = 1,
                    FinishedProduct = product1,
                    Customer = customer1
                },
                new CustomerPurchase
                {
                    CustomerID = 2,
                    FinishedProductsID = 2,
                    FinishedProduct = product2,
                    Customer = customer2
                }
            );

            context.SaveChanges();
        }

        [Fact]
        public async Task GetPurchasesByCustomerIdAsync_ShouldReturnCorrectPurchases()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerPurchaseRepository(context);

            //Act
            var purchases = await repository.GetPurchasesByCustomerIdAsync(1);

            //Assert
            Assert.NotNull(purchases);
            Assert.Single(purchases); //Only one purchase should be related to CustomerID 1
            Assert.Equal("Hat", purchases.First().FinishedProduct.Name); //Verify product name
        }

        [Fact]
        public async Task GetCustomersByFinishedProductIdAsync_ShouldReturnCorrectCustomers()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerPurchaseRepository(context);

            //Act
            var customers = await repository.GetCustomersByFinishedProductIdAsync(2);

            //Assert
            Assert.NotNull(customers);
            Assert.Single(customers); //Only one customer should be related to FinishedProductID 2
            Assert.Equal("Jane Smith", customers.First().Name); //Verify customer name
        }

        [Fact]
        public async Task CreateAsync_ShouldAddCustomerPurchaseToDatabase()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerPurchaseRepository(context);

            var newPurchase = new CustomerPurchase
            {
                CustomerID = 1,
                FinishedProductsID = 2,
            };

            //Act
            var createdPurchase = await repository.CreateAsync(newPurchase);

            //Assert
            var purchases = await context.CustomerPurchases.ToListAsync();
            Assert.Equal(3, purchases.Count); //2 initial purchases + 1 new purchase
            Assert.NotNull(createdPurchase);
        }

        [Fact]
        public async Task GetCustomerPurchaseAsync_ShouldReturnCorrectPurchaseIfExists()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerPurchaseRepository(context);

            //Act
            var purchase = await repository.GetCustomerPurchaseAsync(1, 1); //Check for CustomerID 1 and FinishedProductID 1

            //Assert
            Assert.NotNull(purchase);
            Assert.Equal(1, purchase.CustomerID);
            Assert.Equal(1, purchase.FinishedProductsID);
        }

        [Fact]
        public async Task GetCustomerPurchaseAsync_ShouldReturnNullIfPurchaseDoesNotExist()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerPurchaseRepository(context);

            //Act
            var purchase = await repository.GetCustomerPurchaseAsync(3, 1); //Non-existing CustomerID 3

            //Assert
            Assert.Null(purchase);
        }

        [Fact]
        public async Task DeleteCustomerPurchaseAsync_ShouldDeleteSpecificPurchase()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerPurchaseRepository(context);
            var purchase = await repository.GetCustomerPurchaseAsync(1, 1); //Get a valid CustomerPurchase

            //Act
            await repository.DeleteCustomerPurchaseAsync(purchase!);

            //Assert
            var deletedPurchase = await repository.GetCustomerPurchaseAsync(1, 1);
            Assert.Null(deletedPurchase); //Should no longer exist in the database
        }
    }
}