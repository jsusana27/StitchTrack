using CrochetBusinessAPI.Models;        //Import the necessary models
using CrochetBusinessAPI.Repositories;  //Import the required repositories for data access

namespace CrochetBusinessAPI.Services
{
    //Service class for handling operations related to Stuffing
    public class StuffingService : Service<Stuffing>
    {
        //Repository for Stuffing-specific operations
        private readonly StuffingRepository _stuffingRepository;

        //Constructor to initialize StuffingService with the required repository
        public StuffingService(StuffingRepository stuffingRepository) : base(stuffingRepository)
        {
            _stuffingRepository = stuffingRepository;
        }

        //Get all available stuffing brands
        public async Task<List<string>> GetAllStuffingBrandsAsync()
        {
            return await _stuffingRepository.GetAllStuffingBrandsAsync();
        }

        //Get all available stuffing types
        public async Task<List<string>> GetAllStuffingTypesAsync()
        {
            return await _stuffingRepository.GetAllStuffingTypesAsync();
        }

        //Get stuffing sorted by price per 5 lbs
        public async Task<List<Stuffing>> GetStuffingSortedByPricePer5LbsAsync()
        {
            return await _stuffingRepository.GetStuffingSortedByPricePer5LbsAsync();
        }

        //Delete a specific stuffing entry based on brand and type
        public async Task<Stuffing?> DeleteStuffingByDetailsAsync(string brand, string type)
        {
            return await _stuffingRepository.DeleteStuffingByDetailsAsync(brand, type);
        }

        //Get the stuffing entity by its brand and type
        public async Task<Stuffing?> GetStuffingIdBySpecificationsAsync(string brand, string type)
        {
            return await _stuffingRepository.GetStuffingBySpecificationsAsync(brand, type);
        }
    }
}