using CrochetBusinessAPI.Models;        //Import the necessary models
using CrochetBusinessAPI.Repositories;  //Import the required repositories for data access

namespace CrochetBusinessAPI.Services
{
    //Service class for handling operations related to CustomerPurchases
    public class CustomerPurchaseService : Service<CustomerPurchase>
    {
        //Repository for CustomerPurchase-specific operations
        private readonly CustomerPurchaseRepository _customerPurchaseRepository;

        //Constructor to initialize CustomerPurchaseService with the required repository
        public CustomerPurchaseService(CustomerPurchaseRepository customerPurchaseRepository) : base(customerPurchaseRepository)
        {
            _customerPurchaseRepository = customerPurchaseRepository;
        }

        //Get all purchases made by a specific customer
        public async Task<List<CustomerPurchase>> GetPurchasesByCustomerIdAsync(int customerId)
        {
            return await _customerPurchaseRepository.GetPurchasesByCustomerIdAsync(customerId);
        }

        //Get all customers who purchased a specific product
        public async Task<List<Customer>> GetCustomersByFinishedProductIdAsync(int finishedProductId)
        {
            return await _customerPurchaseRepository.GetCustomersByFinishedProductIdAsync(finishedProductId);
        }
    }
}