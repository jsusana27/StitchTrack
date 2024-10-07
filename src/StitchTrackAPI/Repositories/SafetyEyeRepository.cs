using Microsoft.EntityFrameworkCore;        //Import EntityFrameworkCore for database context and DbSet operations
using CrochetBusinessAPI.Models;            //Import the models containing the SafetyEye entity
using CrochetBusinessAPI.Data;              //Import the namespace containing the application's DbContext

namespace CrochetBusinessAPI.Repositories
{
    //Repository class for managing SafetyEye-specific database operations
    public class SafetyEyeRepository : Repository<SafetyEye>
    {
        //Private readonly context for database operations specific to SafetyEyeRepository
        private readonly CrochetDbContext _context;
        
        //Constructor that initializes the base repository with the given context
        public SafetyEyeRepository(CrochetDbContext context) : base(context)
        {
            _context = context;
        }

        //Method to get all available sizes
        public async Task<List<int>> GetAllSafetyEyeSizesAsync()
        {
            return await EntitySet.Select(se => se.SizeInMM.GetValueOrDefault()).Distinct().ToListAsync();
        }

        //Method to get all available colors
        public async Task<List<string>> GetAllSafetyEyeColorsAsync()
        {
            return await EntitySet.Select(se => se.Color).Distinct().ToListAsync();
        }
 
        //Method to get all available shapes
        public async Task<List<string>> GetAllSafetyEyeShapesAsync()
        {
            return await EntitySet.Select(se => se.Shape).Distinct().ToListAsync();
        }

        //Method to get safety eyes sorted by price
        public async Task<List<SafetyEye>> GetSafetyEyesSortedByPriceAsync()
        {
            return await EntitySet.OrderBy(se => se.Price).AsNoTracking().ToListAsync();
        }

        //Method to get safety eyes sorted by the number in stock
        public async Task<List<SafetyEye>> GetSafetyEyesSortedByStockAsync()
        {
            return await EntitySet.OrderBy(se => se.NumberOfEyesOwned).AsNoTracking().ToListAsync();
        }

        //Method to delete a specific safety eye based on its details
        public async Task<SafetyEye?> DeleteSafetyEyesByDetailsAsync(double size, string color, string shape)
        {
            const double Tolerance = 0.0001; //Define a small tolerance value for size comparison

            var safetyEyeToDelete = await EntitySet
                .FirstOrDefaultAsync(s =>
                    s.SizeInMM.HasValue && Math.Abs(s.SizeInMM.Value - size) < Tolerance && //Safely handle nullable double
                    s.Color == color &&
                    s.Shape == shape);

            if (safetyEyeToDelete == null) return null;

            EntitySet.Remove(safetyEyeToDelete);
            await _context.SaveChangesAsync(); //Save changes to the database

            return safetyEyeToDelete;
        }

        //Method to check if a Safety Eye exists in the database by its attributes
        public async Task<bool> CheckSafetyEyeExistsAsync(int sizeInMM, string color, string shape)
        {
            var safetyEye = await EntitySet.FirstOrDefaultAsync(se =>
                se.SizeInMM == sizeInMM &&
                se.Color == color &&
                se.Shape == shape);

            return safetyEye != null;
        }

        //Method to update the quantity of an existing Safety Eye by its attributes
        public async Task<bool> UpdateSafetyEyeQuantityAsync(int sizeInMM, string color, string shape, int newQuantity)
        {
            var safetyEye = await EntitySet.FirstOrDefaultAsync(se =>
                se.SizeInMM == sizeInMM &&
                se.Color == color &&
                se.Shape == shape);

            if (safetyEye == null) return false;

            safetyEye.NumberOfEyesOwned = newQuantity;
            await SaveAsync(); //Save changes using the base repository method
            return true;
        }

        //Method to get a Safety Eye by its specifications
        public async Task<SafetyEye?> GetSafetyEyesBySpecificationsAsync(int size, string color, string shape)
        {
            return await _context.SafetyEyes
                .Where(se => se.SizeInMM == size && se.Color == color && se.Shape == shape)
                .FirstOrDefaultAsync();
        }
    }
}