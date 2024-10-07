using Microsoft.EntityFrameworkCore;        //Import EntityFrameworkCore for database context and DbSet operations
using CrochetBusinessAPI.Models;            //Import the models containing the Order entity
using CrochetBusinessAPI.Data;              //Import the namespace containing the application's DbContext

namespace CrochetBusinessAPI.Repositories
{
    //Repository class for managing Order-specific database operations
    public class OrderRepository : Repository<Order>
    {
        //Private readonly context for database operations specific to OrderRepository
        private readonly CrochetDbContext _context;

        //Constructor that initializes the base repository with the given context
        public OrderRepository(CrochetDbContext context) : base(context)
        {
            _context = context;
        }

        //Method to get all orders with related Customer and OrderProducts
        public override async Task<List<Order>> GetAllAsync()
        {
            return await EntitySet
                .Include(o => o.Customer) //Eager load Customer details
                .Include(o => o.OrderProducts) //Eager load related OrderProducts
                .ThenInclude(op => op.FinishedProduct) //Load FinishedProduct details for each OrderProduct
                .AsNoTracking() //Read-only query
                .ToListAsync();
        }

        //Method to delete an order by customer name and date
        public async Task<Order?> DeleteOrderByCustomerAndDateAsync(string customerName, DateTime orderDate)
        {
            var orderToDelete = await EntitySet
                .Include(o => o.Customer) //Include Customer entity for filtering
                .FirstOrDefaultAsync(o => o.Customer.Name == customerName && o.OrderDate == orderDate);
            
            if (orderToDelete == null) return null;

            EntitySet.Remove(orderToDelete);
            await _context.SaveChangesAsync();
            return orderToDelete;
        }

        //Method to create a new order
        public async Task<Order> CreateOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        //Method to retrieve an order by its ID
        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order is not null)
            {
                return order;
            }
            else
            {
                throw new KeyNotFoundException($"Order with ID {orderId} not found."); //Exception if ID not found
            }
        }

        //Method to find an order by customer name and order date
        public async Task<Order?> GetOrderByCustomerAndDateAsync(string customerName, DateTime orderDate)
        {
            return await _context.Orders
                .Include(o => o.Customer) //Include Customer entity for filtering
                .FirstOrDefaultAsync(o => o.Customer.Name == customerName && o.OrderDate == orderDate);
        }

        //Method to delete OrderProducts by Order ID
        public async Task DeleteOrderProductsByOrderIdAsync(int orderId)
        {
            var orderProducts = _context.OrderProducts.Where(op => op.OrderID == orderId); //Find OrderProducts by OrderID
            _context.OrderProducts.RemoveRange(orderProducts); //Remove all related OrderProducts
            await _context.SaveChangesAsync(); //Save changes
        }

        //Method to delete an order by its ID
        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId); //Find Order by ID
            if (order != null)
            {
                _context.Orders.Remove(order); //Remove Order if found
                await _context.SaveChangesAsync(); //Save changes
                return true;
            }
            return false;
        }
    }
}