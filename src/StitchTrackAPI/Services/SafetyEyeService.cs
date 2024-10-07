using CrochetBusinessAPI.Models;        //Import models used by the service layer
using CrochetBusinessAPI.Repositories;  //Import repositories for data operations

namespace CrochetBusinessAPI.Services
{
    //Service class for SafetyEye-related operations extending the generic Service class
    public class SafetyEyeService : Service<SafetyEye>
    {
        //Private repository specific to SafetyEye entities
        private readonly SafetyEyeRepository _safetyEyeRepository;

        //Constructor to initialize SafetyEyeService with the specific SafetyEyeRepository
        public SafetyEyeService(SafetyEyeRepository safetyEyeRepository) : base(safetyEyeRepository)
        {
            _safetyEyeRepository = safetyEyeRepository;
        }

        //Method to retrieve all unique safety eye sizes
        public async Task<List<int>> GetAllSafetyEyeSizesAsync()
        {
            return await _safetyEyeRepository.GetAllSafetyEyeSizesAsync();
        }

        //Method to retrieve all unique safety eye colors
        public async Task<List<string>> GetAllSafetyEyeColorsAsync()
        {
            return await _safetyEyeRepository.GetAllSafetyEyeColorsAsync();
        }

        //Method to retrieve all unique safety eye shapes
        public async Task<List<string>> GetAllSafetyEyeShapesAsync()
        {
            return await _safetyEyeRepository.GetAllSafetyEyeShapesAsync();
        }

        //Method to retrieve all safety eyes sorted by price
        public async Task<List<SafetyEye>> GetSafetyEyesSortedByPriceAsync()
        {
            return await _safetyEyeRepository.GetSafetyEyesSortedByPriceAsync();
        }

        //Method to retrieve all safety eyes sorted by the number in stock
        public async Task<List<SafetyEye>> GetSafetyEyesSortedByStockAsync()
        {
            return await _safetyEyeRepository.GetSafetyEyesSortedByStockAsync();
        }

        //Delete safety eyes by size, color, and shape
        public async Task<SafetyEye?> DeleteSafetyEyesByDetailsAsync(double size, string color, string shape)
        {
            return await _safetyEyeRepository.DeleteSafetyEyesByDetailsAsync(size, color, shape);
        }

        //Check if a specific safety eye exists by its attributes
        public async Task<bool> SafetyEyeExistsAsync(int sizeInMM, string color, string shape)
        {
            return await _safetyEyeRepository.CheckSafetyEyeExistsAsync(sizeInMM, color, shape);
        }

        //Update the quantity of an existing SafetyEye entity
        public async Task<bool> UpdateSafetyEyeQuantityAsync(int sizeInMM, string color, string shape, int newQuantity)
        {
            return await _safetyEyeRepository.UpdateSafetyEyeQuantityAsync(sizeInMM, color, shape, newQuantity);
        }

        //Get a SafetyEye entity by its specifications
        public async Task<SafetyEye?> GetSafetyEyesIdBySpecificationsAsync(int size, string color, string shape)
        {
            return await _safetyEyeRepository.GetSafetyEyesBySpecificationsAsync(size, color, shape);
        }
    }
}