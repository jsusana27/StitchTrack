using Microsoft.EntityFrameworkCore;        //Import EntityFrameworkCore for database context and DbSet operations
using CrochetBusinessAPI.Models;            //Import models containing the FinishedProductMaterial entity
using CrochetBusinessAPI.Data;              //Import the namespace containing the application's DbContext

namespace CrochetBusinessAPI.Repositories
{
    //Repository class for managing FinishedProductMaterial-specific database operations
    public class FinishedProductMaterialRepository : Repository<FinishedProductMaterial>
    {
        //Private readonly context for database operations specific to FinishedProductMaterialRepository
        private readonly CrochetDbContext _context;

        //Constructor that initializes the base repository with the given context
        public FinishedProductMaterialRepository(CrochetDbContext context) : base(context)
        {
            _context = context;
        }

        //Method to get all finished products that use a specific material type
        public async Task<List<FinishedProduct>> GetFinishedProductsByMaterialAsync(string materialType, int materialId)
        {
            return await EntitySet
                .Where(fpm => fpm.MaterialType == materialType && fpm.MaterialID == materialId) //Filter by MaterialType and MaterialID
                .Include(fpm => fpm.FinishedProduct) //Eager load the related FinishedProduct
                .Select(fpm => fpm.FinishedProduct) //Only return the FinishedProduct
                .AsNoTracking() //Read-only query for performance optimization
                .ToListAsync();
        }

        //Method to get all materials used in a specific finished product
        public async Task<List<FinishedProductMaterial>> GetMaterialsByFinishedProductIdAsync(int finishedProductId)
        {
            return await EntitySet
                .Where(fpm => fpm.FinishedProductsID == finishedProductId) //Filter by FinishedProductID
                .AsNoTracking() //Read-only query for performance optimization
                .ToListAsync();
        }

        //Method to check if there is enough material available to make a product
        public async Task<bool> CanMakeProductAsync(int materialId, decimal requiredQuantity)
        {
            var materialUsed = await EntitySet
                .Where(fpm => fpm.MaterialID == materialId) //Filter by MaterialID
                .SumAsync(fpm => fpm.QuantityUsed); //Sum the QuantityUsed values for the material
            return materialUsed >= requiredQuantity;
        }

        //Method to get detailed materials used in a product based on product name
        public async Task<List<object>> GetDetailedMaterialsByProductNameAsync(string productName)
        {
            var materials = await EntitySet
                .Where(fpm => fpm.FinishedProduct.Name == productName) //Filter by FinishedProduct name
                .Include(fpm => fpm.FinishedProduct) //Eager load the related FinishedProduct
                .ToListAsync();

            List<object> detailedMaterials = new List<object>();

            //Fetch more details based on material type
            foreach (var material in materials)
            {
                object detailedMaterial = material;

                //Add additional details based on material type
                if (material.MaterialType == "Yarn")
                {
                    var yarn = await _context.Yarns.FindAsync(material.MaterialID);
                    if (yarn != null)
                    {
                        detailedMaterial = new
                        {
                            material.MaterialType,
                            material.QuantityUsed,
                            yarn.Brand,
                            yarn.Color,
                            yarn.FiberType,
                            yarn.FiberWeight,
                            yarn.Price
                        };
                    }
                }
                else if (material.MaterialType == "SafetyEyes")
                {
                    var safetyEye = await _context.SafetyEyes.FindAsync(material.MaterialID);
                    if (safetyEye != null)
                    {
                        detailedMaterial = new
                        {
                            material.MaterialType,
                            material.QuantityUsed,
                            safetyEye.SizeInMM,
                            safetyEye.Color,
                            safetyEye.Price
                        };
                    }
                }
                else if (material.MaterialType == "Stuffing")
                {
                    var stuffing = await _context.Stuffings.FindAsync(material.MaterialID);
                    if (stuffing != null)
                    {
                        detailedMaterial = new
                        {
                            material.MaterialType,
                            material.QuantityUsed,
                            stuffing.Brand,
                            stuffing.Type,
                            stuffing.PricePerFivelbs
                        };
                    }
                }

                detailedMaterials.Add(detailedMaterial);
            }

            return detailedMaterials;
        }

        //Method to update the yarn quantity in a finished product
        public async Task<bool> UpdateYarnQuantityAsync(int finishedProductId, int yarnId, decimal newQuantity)
        {
            var material = await _context.FinishedProductMaterials
                .FirstOrDefaultAsync(m => m.FinishedProductsID == finishedProductId && m.MaterialID == yarnId && m.MaterialType == "Yarn");

            if (material != null)
            {
                material.QuantityUsed = newQuantity;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        //Method to update the quantity of safety eyes in a finished product
        public async Task<bool> UpdateSafetyEyesQuantityAsync(int finishedProductId, int safetyEyesId, decimal newQuantity)
        {
            var finishedProductMaterial = await _context.FinishedProductMaterials
                .FirstOrDefaultAsync(fpm => fpm.FinishedProductsID == finishedProductId && fpm.MaterialID == safetyEyesId && fpm.MaterialType == "SafetyEyes");

            if (finishedProductMaterial != null)
            {
                finishedProductMaterial.QuantityUsed = newQuantity;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        //Method to delete yarn material from a finished product
        public async Task<bool> DeleteYarnMaterialAsync(int finishedProductId, int yarnId)
        {
            var material = await _context.FinishedProductMaterials
                .FirstOrDefaultAsync(m => m.FinishedProductsID == finishedProductId && m.MaterialID == yarnId && m.MaterialType == "Yarn");

            if (material == null) return false;

            _context.FinishedProductMaterials.Remove(material);
            await _context.SaveChangesAsync();
            return true;
        }

        //Method to delete safety eyes material from a finished product
        public async Task<bool> DeleteSafetyEyesMaterialAsync(int finishedProductId, int safetyEyesId)
        {
            var material = await _context.FinishedProductMaterials
                .FirstOrDefaultAsync(m => m.FinishedProductsID == finishedProductId && m.MaterialID == safetyEyesId && m.MaterialType == "SafetyEyes");

            if (material == null) return false;

            _context.FinishedProductMaterials.Remove(material);
            await _context.SaveChangesAsync();
            return true;
        }

        //Method to delete stuffing material from a finished product
        public async Task<bool> DeleteStuffingFromProductAsync(int finishedProductId, int stuffingId)
        {
            var material = await _context.FinishedProductMaterials
                .FirstOrDefaultAsync(fpm => fpm.FinishedProductsID == finishedProductId && fpm.MaterialID == stuffingId && fpm.MaterialType == "Stuffing");

            if (material == null) return false;

            _context.FinishedProductMaterials.Remove(material);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}