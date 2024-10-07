using CrochetBusinessAPI.Models;        //Import models used by the service layer
using CrochetBusinessAPI.Repositories;  //Import repositories for data operations

namespace CrochetBusinessAPI.Services
{
    //Service class for Yarn-related operations extending the generic Service class
    public class YarnService : Service<Yarn>
    {
        //Private repository specific to Yarn entities
        private readonly YarnRepository _yarnRepository;

        //Constructor to initialize YarnService with the specific YarnRepository
        public YarnService(YarnRepository yarnRepository) : base(yarnRepository)
        {
            _yarnRepository = yarnRepository;
        }

        //Method to retrieve all unique yarn brands
        public async Task<List<string>> GetAllYarnBrandsAsync()
        {
            return await _yarnRepository.GetAllYarnBrandsAsync();
        }

        //Method to retrieve all unique yarn colors
        public async Task<List<string>> GetAllYarnColorsAsync()
        {
            return await _yarnRepository.GetAllYarnColorsAsync();
        }

        //Method to retrieve all unique yarn fiber types
        public async Task<List<string>> GetAllYarnFiberTypesAsync()
        {
            return await _yarnRepository.GetAllYarnFiberTypesAsync();
        }

        //Method to retrieve all unique yarn fiber weights
        public async Task<List<int>> GetAllYarnFiberWeightsAsync()
        {
            return await _yarnRepository.GetAllYarnFiberWeightsAsync();
        }

        //Method to retrieve all yarns sorted by price
        public async Task<List<Yarn>> GetAllYarnSortedByPriceAsync()
        {
            return await _yarnRepository.GetAllYarnSortedByPriceAsync();
        }

        //Method to retrieve all yarns sorted by yardage per skein
        public async Task<List<Yarn>> GetAllYarnSortedByYardagePerSkeinAsync()
        {
            return await _yarnRepository.GetAllYarnSortedByYardagePerSkeinAsync();
        }

        //Method to retrieve all yarns sorted by grams per skein
        public async Task<List<Yarn>> GetAllYarnSortedByGramsPerSkeinAsync()
        {
            return await _yarnRepository.GetAllYarnSortedByGramsPerSkeinAsync();
        }

        //Method to retrieve all yarns sorted by the number in stock
        public async Task<List<Yarn>> GetAllYarnSortedByNumberInStockAsync()
        {
            return await _yarnRepository.GetAllYarnSortedByNumberInStockAsync();
        }

        //Method to add a new Yarn entity
        public async Task AddAsync(Yarn yarn)
        {
            await _yarnRepository.AddAsync(yarn);
        }

        //Check if a specific yarn exists in the repository
        public async Task<bool> CheckYarnExistsAsync(string brand, string fiberType, int fiberWeight, string color)
        {
            return await _yarnRepository.CheckYarnExistsAsync(brand, fiberType, fiberWeight, color);
        }

        //Update the quantity of an existing Yarn entity
        public async Task<bool> UpdateYarnQuantityAsync(string brand, string fiberType, int fiberWeight, string color, int newQuantity)
        {
            return await _yarnRepository.UpdateYarnQuantityAsync(brand, fiberType, fiberWeight, color, newQuantity);
        }

        //Delete a Yarn entity by its detailed specifications
        public async Task<Yarn?> DeleteYarnByDetailsAsync(string brand, string? fiberType, int? fiberWeight, string color)
        {
            var yarnToDelete = await _yarnRepository.DeleteYarnByDetailsAsync(brand, fiberType, fiberWeight, color);
            return yarnToDelete;
        }

        //Retrieve a Yarn entity by its specifications
        public async Task<Yarn?> GetYarnIdBySpecificationsAsync(string brand, string fiberType, int fiberWeight, string color)
        {
            return await _yarnRepository.GetYarnBySpecificationsAsync(brand, fiberType, fiberWeight, color);
        }
    }
}