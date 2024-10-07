using Microsoft.EntityFrameworkCore;        //Import EntityFrameworkCore for database context and DbSet operations
using CrochetBusinessAPI.Models;            //Import the models containing the Yarn entity
using CrochetBusinessAPI.Data;              //Import the namespace containing the application's DbContext

namespace CrochetBusinessAPI.Repositories
{
    //Repository class for managing Yarn-specific database operations
    public class YarnRepository : Repository<Yarn>
    {
        //Private readonly context for database operations specific to YarnRepository
        private readonly CrochetDbContext _context;

        //Constructor that initializes the base repository with the given context
        public YarnRepository(CrochetDbContext context) : base(context)
        {
            _context = context;
        }

        //Retrieve a distinct list of all yarn brands
        public async Task<List<string>> GetAllYarnBrandsAsync()
        {
            return await EntitySet.Select(y => y.Brand).Distinct().ToListAsync();
        }

        //Retrieve a distinct list of all yarn colors
        public async Task<List<string>> GetAllYarnColorsAsync()
        {
            return await EntitySet.Select(y => y.Color).Distinct().ToListAsync();
        }

        //Retrieve a distinct list of all yarn fiber types
        public async Task<List<string>> GetAllYarnFiberTypesAsync()
        {
            return await EntitySet.Select(y => y.FiberType).Distinct().ToListAsync();
        }

        //Retrieve a distinct list of all yarn fiber weights
        public async Task<List<int>> GetAllYarnFiberWeightsAsync()
        {
            return await EntitySet.Select(y => y.FiberWeight.GetValueOrDefault()).Distinct().ToListAsync();
        }

        //Retrieve a list of all yarns sorted by price
        public async Task<List<Yarn>> GetAllYarnSortedByPriceAsync()
        {
            return await EntitySet.OrderBy(y => y.Price).AsNoTracking().ToListAsync();
        }

        //Retrieve a list of all yarns sorted by yardage per skein
        public async Task<List<Yarn>> GetAllYarnSortedByYardagePerSkeinAsync()
        {
            return await EntitySet.OrderBy(y => y.YardagePerSkein).AsNoTracking().ToListAsync();
        }

        //Retrieve a list of all yarns sorted by grams per skein
        public async Task<List<Yarn>> GetAllYarnSortedByGramsPerSkeinAsync()
        {
            return await EntitySet.OrderBy(y => y.GramsPerSkein).AsNoTracking().ToListAsync();
        }

        //Retrieve a list of all yarns sorted by number in stock
        public async Task<List<Yarn>> GetAllYarnSortedByNumberInStockAsync()
        {
            return await EntitySet.OrderBy(y => y.NumberOfSkeinsOwned).AsNoTracking().ToListAsync();
        }

        //Insert a new yarn entity asynchronously
        public async Task AddAsync(Yarn yarn)
        {
            await EntitySet.AddAsync(yarn);
            await _context.SaveChangesAsync();
        }

        //Check if a yarn exists by its attributes (brand, fiber type, fiber weight, color)
        public async Task<bool> CheckYarnExistsAsync(string brand, string fiberType, int fiberWeight, string color)
        {
            return await EntitySet.AnyAsync(y =>
                y.Brand == brand &&
                y.FiberType == fiberType &&
                y.FiberWeight == fiberWeight &&
                y.Color == color
            );
        }

        //Update the quantity of a specific yarn by its attributes
        public async Task<bool> UpdateYarnQuantityAsync(string brand, string fiberType, int fiberWeight, string color, int newQuantity)
        {
            var yarn = await EntitySet.FirstOrDefaultAsync(y =>
                y.Brand == brand &&
                y.FiberType == fiberType &&
                y.FiberWeight == fiberWeight &&
                y.Color == color
            );

            if (yarn == null) return false;

            yarn.NumberOfSkeinsOwned = newQuantity;
            await SaveAsync();
            return true;
        }

        //Delete a yarn based on its details and return the deleted yarn entity
        public async Task<Yarn?> DeleteYarnByDetailsAsync(string brand, string? fiberType, int? fiberWeight, string color)
        {
            var yarnToDelete = await EntitySet
                .FirstOrDefaultAsync(y =>
                    y.Brand == brand &&
                    y.FiberType == fiberType &&
                    y.FiberWeight == fiberWeight &&
                    y.Color == color);

            if (yarnToDelete == null) return null;

            EntitySet.Remove(yarnToDelete);
            await _context.SaveChangesAsync(); //Save changes to the database using the context method
            return yarnToDelete;
        }

        //Retrieve a specific yarn based on its specifications
        public async Task<Yarn?> GetYarnBySpecificationsAsync(string brand, string fiberType, int fiberWeight, string color)
        {
            return await EntitySet
                .Where(y => y.Brand == brand && y.FiberType == fiberType && y.FiberWeight == fiberWeight && y.Color == color)
                .FirstOrDefaultAsync();
        }
    }
}