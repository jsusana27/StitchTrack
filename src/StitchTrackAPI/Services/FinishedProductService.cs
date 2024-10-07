using CrochetBusinessAPI.Models;        //Import the models used in the service layer
using CrochetBusinessAPI.Repositories;  //Import repositories for data access and operations

namespace CrochetBusinessAPI.Services
{
    //Service class for handling business logic related to finished products
    public class FinishedProductService : Service<FinishedProduct>
    {
        //Repository specific to FinishedProduct operations
        private readonly FinishedProductRepository _finishedProductRepository;

        //Constructor to initialize FinishedProductService with the FinishedProductRepository
        public FinishedProductService(FinishedProductRepository finishedProductRepository) : base(finishedProductRepository)
        {
            _finishedProductRepository = finishedProductRepository;
        }

        //Retrieve a list of all finished product names
        public async Task<List<string>> GetAllProductNamesAsync()
        {
            return await _finishedProductRepository.GetAllProductNamesAsync();
        }

        //Retrieve a list of finished products sorted by the time to make
        public async Task<List<FinishedProduct>> GetProductsSortedByTimeToMakeAsync()
        {
            return await _finishedProductRepository.GetProductsSortedByTimeToMakeAsync();
        }

        //Retrieve a list of finished products sorted by the cost to make
        public async Task<List<FinishedProduct>> GetProductsSortedByCostToMakeAsync()
        {
            return await _finishedProductRepository.GetProductsSortedByCostToMakeAsync();
        }

        //Retrieve a list of finished products sorted by sale price
        public async Task<List<FinishedProduct>> GetProductsSortedBySalePriceAsync()
        {
            return await _finishedProductRepository.GetProductsSortedBySalePriceAsync();
        }

        //Retrieve a list of finished products sorted by the number in stock
        public async Task<List<FinishedProduct>> GetProductsSortedByNumberInStockAsync()
        {
            return await _finishedProductRepository.GetProductsSortedByNumberInStockAsync();
        }

        //Search for a specific finished product by its name
        public async Task<FinishedProduct?> GetProductByNameAsync(string productName)
        {
            return await _finishedProductRepository.GetProductByNameAsync(productName);
        }

        //Delete a finished product by its name
        public async Task<FinishedProduct?> DeleteProductByNameAsync(string name)
        {
            return await _finishedProductRepository.DeleteProductByNameAsync(name);
        }

        //Check if a finished product exists based on its name
        public async Task<bool> CheckFinishedProductExistsAsync(string name)
        {
            return await _finishedProductRepository.CheckFinishedProductExistsAsync(name);
        }

        //Update the quantity of a specific finished product
        public async Task<bool> UpdateFinishedProductQuantityAsync(string name, int newQuantity)
        {
            return await _finishedProductRepository.UpdateFinishedProductQuantityAsync(name, newQuantity);
        }
    }
}