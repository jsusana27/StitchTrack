using Microsoft.EntityFrameworkCore;        //Import EntityFrameworkCore for database context and DbSet operations
using CrochetBusinessAPI.Models;            //Import the models containing the OrderProduct entity
using CrochetBusinessAPI.Data;              //Import the namespace containing the application's DbContext

namespace CrochetBusinessAPI.Repositories
{
    //Repository class for managing OrderProduct-specific database operations
    public class OrderProductRepository : Repository<OrderProduct>
    {
        //Private readonly context for database operations specific to OrderProductRepository
        private readonly CrochetDbContext _context;

        //Constructor that initializes the base repository with the given context
        public OrderProductRepository(CrochetDbContext context) : base(context)
        {
            _context = context;
        }

        //Get all OrderProduct records by OrderID
        public async Task<List<OrderProduct>> GetOrderProductsByOrderIdAsync(int orderId)
        {
            return await EntitySet
                .Where(op => op.OrderID == orderId) //Filter by OrderID
                .Include(op => op.FinishedProduct) //Eager load FinishedProduct details
                .AsNoTracking() //Read-only query for performance optimization
                .ToListAsync();
        }

        //Get all OrderProduct records by FinishedProductID (for sale stats or product-related queries)
        public async Task<List<OrderProduct>> GetOrderProductsByFinishedProductIdAsync(int finishedProductId)
        {
            return await EntitySet
                .Where(op => op.FinishedProductsID == finishedProductId) // Filter by FinishedProductID
                .Include(op => op.FinishedProduct) // Include FinishedProduct details to avoid lazy loading issues
                .Include(op => op.Order) // Include Order details if needed
                .ToListAsync();
        }

        //Method to get total quantity sold for a finished product
        public async Task<int> GetTotalQuantitySoldAsync(int finishedProductId)
        {
            return await _context.OrderProducts
                .Where(op => op.FinishedProductsID == finishedProductId) //Filter by FinishedProductID
                .SumAsync(op => op.Quantity ?? 0); //Handle nullable Quantity values using the null-coalescing operator
        }

        //Method to get total revenue for a finished product
        public async Task<decimal> GetTotalRevenueForFinishedProductAsync(int finishedProductId)
        {
            return await EntitySet
                .Where(op => op.FinishedProductsID == finishedProductId) //Filter by FinishedProductID
                .SumAsync(op => (op.Quantity ?? 0) * (op.FinishedProduct.SalePrice ?? 0.0m));
        }

        //Method to add a new OrderProduct to the database
        public async Task<OrderProduct> AddOrderProductAsync(OrderProduct orderProduct)
        {
            _context.OrderProducts.Add(orderProduct); //Add the new OrderProduct entity
            await _context.SaveChangesAsync(); //Save changes to the database
            return orderProduct;
        }
    }
}