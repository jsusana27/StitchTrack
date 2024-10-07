using Microsoft.EntityFrameworkCore;        //Import EntityFrameworkCore for database context and DbSet operations
using CrochetBusinessAPI.Models;            //Import the models containing the Stuffing entity
using CrochetBusinessAPI.Data;              //Import the namespace containing the application's DbContext

namespace CrochetBusinessAPI.Repositories
{
    //Repository class for managing Stuffing-specific database operations
    public class StuffingRepository : Repository<Stuffing>
    {
        //Private readonly context for database operations specific to StuffingRepository
        private readonly CrochetDbContext _context;

        //Constructor that initializes the base repository with the given context
        public StuffingRepository(CrochetDbContext context) : base(context)
        {
            _context = context;
        }

        //Get all available stuffing brands
        public async Task<List<string>> GetAllStuffingBrandsAsync()
        {
            return await EntitySet.Select(s => s.Brand).Distinct().ToListAsync();
        }

        //Get all available stuffing types
        public async Task<List<string>> GetAllStuffingTypesAsync()
        {
            return await EntitySet.Select(s => s.Type).Distinct().ToListAsync();
        }

        //Get stuffing sorted by price per 5 lbs
        public async Task<List<Stuffing>> GetStuffingSortedByPricePer5LbsAsync()
        {
            return await EntitySet.OrderBy(s => s.PricePerFivelbs).AsNoTracking().ToListAsync();
        }

        //Delete stuffing by brand and type details
        public async Task<Stuffing?> DeleteStuffingByDetailsAsync(string brand, string type)
        {
            var stuffingToDelete = await EntitySet
                .FirstOrDefaultAsync(s => s.Brand == brand && s.Type == type);

            if (stuffingToDelete == null) return null;

            EntitySet.Remove(stuffingToDelete);
            await _context.SaveChangesAsync();

            return stuffingToDelete;
        }

        //Method to get Stuffing by brand and type
        public async Task<Stuffing?> GetStuffingBySpecificationsAsync(string brand, string type)
        {
            return await _context.Stuffings
                .Where(st => st.Brand == brand && st.Type == type)
                .FirstOrDefaultAsync();
        }
    }
}