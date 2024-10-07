using Microsoft.EntityFrameworkCore;
using CrochetBusinessAPI.Data;
using CrochetBusinessAPI.Models;
using CrochetBusinessAPI.Repositories;

namespace CrochetBusinessAPI.Tests.Repositories
{
    public class OrderRepositoryTests
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

        // Seed the in-memory database with sample data for testing
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

            var order1 = new Order
            {
                OrderID = 1,
                CustomerID = 1,
                Customer = customer1,
                OrderDate = DateTime.Now.AddDays(-1)
            };

            var order2 = new Order
            {
                OrderID = 2,
                CustomerID = 2,
                Customer = customer2,
                OrderDate = DateTime.Now
            };

            context.Customers.AddRange(customer1, customer2);
            context.FinishedProducts.AddRange(product1, product2);
            context.Orders.AddRange(order1, order2);

            context.OrderProducts.AddRange(
                new OrderProduct
                {
                    OrderID = 1,
                    FinishedProductsID = 1,
                    Quantity = 2,
                    FinishedProduct = product1
                },
                new OrderProduct
                {
                    OrderID = 2,
                    FinishedProductsID = 2,
                    Quantity = 3,
                    FinishedProduct = product2
                }
            );

            context.SaveChanges();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllOrdersWithDetails()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new OrderRepository(context);

            //Act
            var orders = await repository.GetAllAsync();

            //Assert
            Assert.NotNull(orders);
            Assert.Equal(2, orders.Count); //Should return the 2 orders in the seeded data
            Assert.Equal("John Doe", orders[0].Customer.Name); //Ensure Customer details are included
            Assert.Single(orders[0].OrderProducts); //Ensure OrderProducts are included
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldAddOrderToDatabase()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new OrderRepository(context);
            var newOrder = new Order
            {
                CustomerID = 1,
                OrderDate = DateTime.Now.AddDays(-3)
            };

            //Act
            var createdOrder = await repository.CreateOrderAsync(newOrder);

            //Assert
            var orders = await context.Orders.ToListAsync();
            Assert.Equal(3, orders.Count); //Initial 2 orders + 1 new order
            Assert.NotNull(createdOrder);
            Assert.Equal(1, createdOrder.CustomerID); //Ensure the CustomerID matches
        }

        [Fact]
        public async Task DeleteOrderByCustomerAndDateAsync_ShouldDeleteOrder()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new OrderRepository(context);

            //Act
            var deletedOrder = await repository.DeleteOrderByCustomerAndDateAsync(
                "Jane Smith", 
                context.Orders.First(o => o.Customer.Name == "Jane Smith").OrderDate!.Value
            );

            //Assert
            Assert.NotNull(deletedOrder);
            Assert.Equal("Jane Smith", deletedOrder.Customer.Name);

            //Safely handle the nullable OrderDate
            if (deletedOrder.OrderDate.HasValue)
            {
                var exists = await repository.GetOrderByCustomerAndDateAsync("Jane Smith", deletedOrder.OrderDate.Value);
                Assert.Null(exists); //Should no longer exist in the database
            }
            else
            {
                Assert.Fail("OrderDate should not be null after deleting the order.");
            }
        }


        [Fact]
        public async Task GetOrderByIdAsync_ShouldReturnCorrectOrderIfExists()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new OrderRepository(context);

            //Act
            var order = await repository.GetOrderByIdAsync(1);

            //Assert
            Assert.NotNull(order);
            Assert.Equal(1, order.OrderID);
            Assert.Equal("John Doe", order.Customer.Name); //Ensure the Customer is included
        }

        [Fact]
        public async Task GetOrderByIdAsync_ShouldThrowExceptionIfOrderNotFound()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new OrderRepository(context);

            //Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => repository.GetOrderByIdAsync(999)); //ID 999 does not exist
        }

        [Fact]
        public async Task DeleteOrderAsync_ShouldDeleteOrderIfExists()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new OrderRepository(context);

            //Act
            var result = await repository.DeleteOrderAsync(1);

            //Assert
            Assert.True(result);
            var exists = await context.Orders.FindAsync(1);
            Assert.Null(exists); //Order should be deleted
        }

        [Fact]
        public async Task DeleteOrderAsync_ShouldReturnFalseIfOrderNotFound()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new OrderRepository(context);

            //Act
            var result = await repository.DeleteOrderAsync(999); //ID 999 does not exist

            //Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteOrderProductsByOrderIdAsync_ShouldDeleteRelatedOrderProducts()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new OrderRepository(context);

            //Act
            await repository.DeleteOrderProductsByOrderIdAsync(1);

            //Assert
            var orderProducts = await context.OrderProducts.Where(op => op.OrderID == 1).ToListAsync();
            Assert.Empty(orderProducts); //All OrderProducts for OrderID 1 should be deleted
        }
    }
}