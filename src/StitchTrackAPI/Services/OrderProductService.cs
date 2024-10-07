using CrochetBusinessAPI.Models;        //Import the necessary models
using CrochetBusinessAPI.Repositories;  //Import the required repositories for data access

namespace CrochetBusinessAPI.Services
{
    //Service class for handling operations related to OrderProduct
    public class OrderProductService : Service<OrderProduct>
    {
        //Repository for OrderProduct-specific operations
        private readonly OrderProductRepository _orderProductRepository;

        //Repository for FinishedProduct-specific operations
        private readonly FinishedProductRepository _finishedProductRepository;

        //Constructor to initialize OrderProductService with the required repositories
        public OrderProductService(OrderProductRepository orderProductRepository, 
        FinishedProductRepository finishedProductRepository) : base(orderProductRepository)
        {
            _orderProductRepository = orderProductRepository;
            _finishedProductRepository = finishedProductRepository;
        }

        //Get all OrderProducts by OrderID
        public async Task<List<OrderProduct>> GetOrderProductsByOrderIdAsync(int orderId)
        {
            return await _orderProductRepository.GetOrderProductsByOrderIdAsync(orderId);
        }

        //Get all OrderProducts by FinishedProductID
        public async Task<List<OrderProduct>> GetOrderProductsByFinishedProductIdAsync(int finishedProductId)
        {
            return await _orderProductRepository.GetOrderProductsByFinishedProductIdAsync(finishedProductId);
        }

        //Get total quantity of a FinishedProduct across all orders
        public async Task<int> GetTotalQuantityForFinishedProductAsync(int finishedProductId)
        {
            return await _orderProductRepository.GetTotalQuantitySoldAsync(finishedProductId);
        }

        //Get the total revenue generated for a specific FinishedProduct
        public async Task<decimal> GetTotalRevenueForFinishedProductAsync(int finishedProductId)
        {
            return await _orderProductRepository.GetTotalRevenueForFinishedProductAsync(finishedProductId);
        }

        //Get sales statistics for a specific product, including quantity sold and revenue generated
        public async Task<object> GetSaleStatsForProductAsync(string productName)
        {
            //Get the Finished Product details by name
            var finishedProduct = await _finishedProductRepository.GetProductByNameAsync(productName);
            if (finishedProduct == null) 
            {
                throw new InvalidOperationException($"Product with name '{productName}' not found."); //Throw exception if product is not found
            }

            //Get total quantity sold for the product
            var totalQuantity = await _orderProductRepository.GetTotalQuantitySoldAsync(finishedProduct.FinishedProductsID.GetValueOrDefault());

            //Calculate total revenue using sale price and total quantity sold
            var totalRevenue = totalQuantity * finishedProduct.SalePrice;

            //Return an object containing total quantity and revenue information
            return new { totalQuantity, totalRevenue };
        }
    }
}