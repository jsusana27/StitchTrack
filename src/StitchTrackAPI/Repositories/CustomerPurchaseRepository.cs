using Microsoft.EntityFrameworkCore;        //Import EntityFrameworkCore for database context and DbSet operations
using CrochetBusinessAPI.Models;            //Import models containing the CustomerPurchase entity
using CrochetBusinessAPI.Data;              //Import the namespace containing the application's DbContext

namespace CrochetBusinessAPI.Repositories
{
    //Repository class for managing CustomerPurchase-specific database operations
    public class CustomerPurchaseRepository : Repository<CustomerPurchase>
    {
        //Private readonly context for database operations specific to CustomerPurchaseRepository
        private readonly CrochetDbContext _context;

        //Constructor that initializes the base repository with the given context
        public CustomerPurchaseRepository(CrochetDbContext context) : base(context)
        {
            _context = context;
        }

        //Method to get all purchases made by a specific customer
        public async Task<List<CustomerPurchase>> GetPurchasesByCustomerIdAsync(int customerId)
        {
            return await EntitySet
                .Where(cp => cp.CustomerID == customerId) //Filter by CustomerID
                .Include(cp => cp.FinishedProduct) //Eager load the purchased product details
                .AsNoTracking() //Read-only query for performance optimization
                .ToListAsync();
        }

        //Method to get all customers who purchased a specific product
        public async Task<List<Customer>> GetCustomersByFinishedProductIdAsync(int finishedProductId)
        {
            return await EntitySet
                .Where(cp => cp.FinishedProductsID == finishedProductId) //Filter by FinishedProductID
                .Include(cp => cp.Customer) //Eager load the customer details
                .Select(cp => cp.Customer) //Return only the customers from the query
                .AsNoTracking() //Read-only query for performance optimization
                .ToListAsync();
        }

        //Method to create a new CustomerPurchase entry
        public async Task<CustomerPurchase> CreateAsync(CustomerPurchase customerPurchase)
        {
            _context.CustomerPurchases.Add(customerPurchase); //Add the new customer purchase record to the context
            await _context.SaveChangesAsync(); //Save changes asynchronously to update the database
            return customerPurchase; //Return the created customer purchase object
        }

        //Method to get a specific CustomerPurchase by CustomerID and FinishedProductsID
        public async Task<CustomerPurchase?> GetCustomerPurchaseAsync(int customerId, int finishedProductId)
        {
            //Filter by CustomerID and FinishedProductID
            return await EntitySet
                .FirstOrDefaultAsync(cp => cp.CustomerID == customerId && cp.FinishedProductsID == finishedProductId); 
        }

        //Method to delete a specific CustomerPurchase
        public async Task DeleteCustomerPurchaseAsync(CustomerPurchase customerPurchase)
        {
            EntitySet.Remove(customerPurchase); //Remove the CustomerPurchase record from the DbSet
            await _context.SaveChangesAsync(); //Save changes to the database
        }
    }
}