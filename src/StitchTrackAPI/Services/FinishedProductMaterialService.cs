using CrochetBusinessAPI.Models;        //Import models used in the service layer
using CrochetBusinessAPI.Repositories;  //Import repositories for data operations

namespace CrochetBusinessAPI.Services
{
    //Service class for managing operations related to FinishedProductMaterial
    public class FinishedProductMaterialService : Service<FinishedProductMaterial>
    {
        //Repository specific to FinishedProductMaterial operations
        private readonly FinishedProductMaterialRepository _finishedProductMaterialRepository;

        //Constructor to initialize FinishedProductMaterialService with the repository
        public FinishedProductMaterialService(FinishedProductMaterialRepository finishedProductMaterialRepository) : base(finishedProductMaterialRepository)
        {
            _finishedProductMaterialRepository = finishedProductMaterialRepository;
        }

        //Get all finished products that use a specific material type
        public async Task<List<FinishedProduct>> GetFinishedProductsByMaterialAsync(string materialType, int materialId)
        {
            return await _finishedProductMaterialRepository.GetFinishedProductsByMaterialAsync(materialType, materialId);
        }

        //Get all materials used in a specific finished product
        public async Task<List<FinishedProductMaterial>> GetMaterialsByFinishedProductIdAsync(int finishedProductId)
        {
            return await _finishedProductMaterialRepository.GetMaterialsByFinishedProductIdAsync(finishedProductId);
        }

        //Check if there is enough material available to make a product
        public async Task<bool> CanMakeProductAsync(int materialId, decimal requiredQuantity)
        {
            return await _finishedProductMaterialRepository.CanMakeProductAsync(materialId, requiredQuantity);
        }

        //Get detailed materials for a specific product by its name
        public async Task<List<object>> GetDetailedMaterialsByProductNameAsync(string productName)
        {
            return await _finishedProductMaterialRepository.GetDetailedMaterialsByProductNameAsync(productName);
        }

        //Service method to update the yarn quantity
        public async Task<bool> UpdateYarnQuantityAsync(int finishedProductId, int yarnId, decimal newQuantity)
        {
            return await _finishedProductMaterialRepository.UpdateYarnQuantityAsync(finishedProductId, yarnId, newQuantity);
        }

        //Service method to update the quantity of safety eyes
        public async Task<bool> UpdateSafetyEyesQuantityAsync(int finishedProductId, int safetyEyesId, decimal newQuantity)
        {
            return await _finishedProductMaterialRepository.UpdateSafetyEyesQuantityAsync(finishedProductId, safetyEyesId, newQuantity);
        }

        //Delete yarn material by its ID and the finished product ID
        public async Task<bool> DeleteYarnMaterialAsync(int finishedProductId, int yarnId)
        {
            return await _finishedProductMaterialRepository.DeleteYarnMaterialAsync(finishedProductId, yarnId);
        }

        //Delete safety eyes material by its ID and the finished product ID
        public async Task<bool> DeleteSafetyEyesMaterialAsync(int finishedProductId, int safetyEyesId)
        {
            return await _finishedProductMaterialRepository.DeleteSafetyEyesMaterialAsync(finishedProductId, safetyEyesId);
        }

        //Delete stuffing material by its ID and the finished product ID
        public async Task<bool> DeleteStuffingFromProductAsync(int finishedProductId, int stuffingId)
        {
            return await _finishedProductMaterialRepository.DeleteStuffingFromProductAsync(finishedProductId, stuffingId);
        }
    }
}