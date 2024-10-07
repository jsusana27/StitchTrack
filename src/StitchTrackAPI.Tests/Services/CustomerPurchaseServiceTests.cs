using CrochetBusinessAPI.Data;       
using CrochetBusinessAPI.Models;      
using CrochetBusinessAPI.Repositories;
using CrochetBusinessAPI.Services;   
using Microsoft.EntityFrameworkCore;  

namespace CrochetBusinessAPI.Tests.Services
{
    public class CustomerPurchaseServiceTests
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

            //Seed FinishedProducts
            context.FinishedProducts.AddRange(
                new FinishedProduct
                {
                    FinishedProductsID = 1,
                    Name = "Hat",
                    SalePrice = 20.0m,
                    NumberInStock = 10
                },
                new FinishedProduct
                {
                    FinishedProductsID = 2,
                    Name = "Scarf",
                    SalePrice = 25.0m,
                    NumberInStock = 5
                }
            );

            //Seed CustomerPurchases
            context.CustomerPurchases.AddRange(
                new CustomerPurchase
                {
                    CustomerID = 1,
                    FinishedProductsID = 1 //John Doe purchased a Hat
                },
                new CustomerPurchase
                {
                    CustomerID = 1,
                    FinishedProductsID = 2 //John Doe purchased a Scarf
                },
                new CustomerPurchase
                {
                    CustomerID = 2,
                    FinishedProductsID = 1 //Jane Smith purchased a Hat
                }
            );

            context.SaveChanges();
        }

        [Fact]
        public async Task GetPurchasesByCustomerIdAsync_ShouldReturnAllPurchasesForCustomer()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerPurchaseRepository(context);
            var service = new CustomerPurchaseService(repository);

            //Act
            var purchases = await service.GetPurchasesByCustomerIdAsync(1); //Get purchases for CustomerID = 1 (John Doe)

            //Assert
            Assert.NotNull(purchases);
            Assert.Equal(2, purchases.Count); //John Doe has 2 purchases
            Assert.All(purchases, p => Assert.Equal(1, p.CustomerID)); //All purchases should belong to CustomerID = 1
        }

        [Fact]
        public async Task GetPurchasesByCustomerIdAsync_ShouldReturnEmptyListForNonExistentCustomer()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerPurchaseRepository(context);
            var service = new CustomerPurchaseService(repository);

            //Act
            var purchases = await service.GetPurchasesByCustomerIdAsync(999); //Get purchases for a non-existent CustomerID

            //Assert
            Assert.NotNull(purchases);
            Assert.Empty(purchases); //No purchases should be found
        }

        [Fact]
        public async Task GetCustomersByFinishedProductIdAsync_ShouldReturnAllCustomersWhoPurchasedProduct()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerPurchaseRepository(context);
            var service = new CustomerPurchaseService(repository);

            //Act
            var customers = await service.GetCustomersByFinishedProductIdAsync(1); //Get customers for FinishedProductID = 1 (Hat)

            //Assert
            Assert.NotNull(customers);
            Assert.Equal(2, customers.Count); //2 customers purchased a Hat
            Assert.Contains(customers, c => c.CustomerID == 1); //John Doe
            Assert.Contains(customers, c => c.CustomerID == 2); //Jane Smith
        }

        [Fact]
        public async Task GetCustomersByFinishedProductIdAsync_ShouldReturnEmptyListForNonExistentProduct()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerPurchaseRepository(context);
            var service = new CustomerPurchaseService(repository);

            //Act
            var customers = await service.GetCustomersByFinishedProductIdAsync(999); //Get customers for a non-existent FinishedProductID

            //Assert
            Assert.NotNull(customers);
            Assert.Empty(customers); //No customers should be found
        }
    }
}
