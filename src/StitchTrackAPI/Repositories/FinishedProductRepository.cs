using Microsoft.EntityFrameworkCore;        //Import EntityFrameworkCore for database context and DbSet operations
using CrochetBusinessAPI.Models;            //Import the models containing the FinishedProduct entity
using CrochetBusinessAPI.Data;              //Import the namespace containing the application's DbContext

namespace CrochetBusinessAPI.Repositories
{
    //Repository class for managing FinishedProduct-specific database operations
    public class FinishedProductRepository : Repository<FinishedProduct>
    {
        //Private readonly context for database operations specific to FinishedProductRepository
        private readonly CrochetDbContext _context;
        
        //Constructor that initializes the base repository with the given context
        public FinishedProductRepository(CrochetDbContext context) : base(context)
        {
            _context = context;
        }

        //Get all product names
        public async Task<List<string>> GetAllProductNamesAsync()
        {
            return await EntitySet.Select(fp => fp.Name).ToListAsync();
        }

        //Get all products sorted by time to make
        public async Task<List<FinishedProduct>> GetProductsSortedByTimeToMakeAsync()
        {
            return await EntitySet.OrderBy(fp => fp.TimeToMake).AsNoTracking().ToListAsync();
        }

        //Get all products sorted by cost to make
        public async Task<List<FinishedProduct>> GetProductsSortedByCostToMakeAsync()
        {
            return await EntitySet.OrderBy(fp => fp.TotalCostToMake).AsNoTracking().ToListAsync();
        }

        //Get all products sorted by sale price
        public async Task<List<FinishedProduct>> GetProductsSortedBySalePriceAsync()
        {
            return await EntitySet.OrderBy(fp => fp.SalePrice).AsNoTracking().ToListAsync();
        }

        //Get all products sorted by number in stock
        public async Task<List<FinishedProduct>> GetProductsSortedByNumberInStockAsync()
        {
            return await EntitySet.OrderBy(fp => fp.NumberInStock).AsNoTracking().ToListAsync();
        }

        //Method to search for a product by name
        public async Task<FinishedProduct?> GetProductByNameAsync(string productName)
        {
            return await EntitySet
                .Where(fp => fp.Name == productName)
                .FirstOrDefaultAsync(); //Get the first match or return null if not found
        }

        //Delete a product by its name
        public async Task<FinishedProduct?> DeleteProductByNameAsync(string name)
        {
            var productToDelete = await EntitySet.FirstOrDefaultAsync(p => p.Name == name);
            if (productToDelete == null) return null;

            EntitySet.Remove(productToDelete);
            await _context.SaveChangesAsync();
            return productToDelete;
        }

        //Check if a finished product exists by name
        public async Task<bool> CheckFinishedProductExistsAsync(string name)
        {
            var normalizedInputName = name.ToLower(); //Pre-normalize the input string to lowercase
            return await EntitySet.AnyAsync(f => f.Name.ToLower() == normalizedInputName);
        }

        //Update the quantity of a specific finished product
        public async Task<bool> UpdateFinishedProductQuantityAsync(string name, int newQuantity)
        {
            var normalizedInputName = name.ToLower(); //Pre-normalize the input string to lowercase

            var finishedProduct = await EntitySet
                .FirstOrDefaultAsync(f => f.Name.ToLower() == normalizedInputName);

            if (finishedProduct == null) return false;

            finishedProduct.NumberInStock = newQuantity;
            await SaveAsync();
            return true;
        }
    }
}