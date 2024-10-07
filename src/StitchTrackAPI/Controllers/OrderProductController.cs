using Microsoft.AspNetCore.Mvc;         //ASP.NET Core MVC for building API controllers
using CrochetBusinessAPI.Models;        //Import OrderProduct model
using CrochetBusinessAPI.Services;      //Import OrderProduct service layer for business logic

namespace CrochetBusinessAPI.Controllers
{
    //API Controller for OrderProduct-related operations
    [Route("api/[controller]")]
    [ApiController]
    public class OrderProductController : Controller<OrderProduct>
    {
        private readonly OrderProductService _orderProductService; //Reference to OrderProductService for handling business logic

        //Constructor to initialize the OrderProductController with a specific service
        public OrderProductController(OrderProductService orderProductService) : base(orderProductService)
        {
            _orderProductService = orderProductService;
        }

        //Get all OrderProducts by OrderID
        //GET: api/OrderProduct/order/{orderId}
        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetOrderProductsByOrderId(int orderId)
        {
            var orderProducts = await _orderProductService.GetOrderProductsByOrderIdAsync(orderId); //Fetch OrderProducts by OrderID
            return Ok(orderProducts); //Return the list of OrderProducts
        }

        //Get all OrderProducts by FinishedProductID
        //GET: api/OrderProduct/product/{finishedProductId}
        [HttpGet("product/{finishedProductId}")]
        public async Task<IActionResult> GetOrderProductsByFinishedProductId(int finishedProductId)
        {
            var orderProducts = await _orderProductService.GetOrderProductsByFinishedProductIdAsync(finishedProductId); //Fetch OrderProducts by FinishedProductID
            return Ok(orderProducts); //Return the list of OrderProducts
        }

        //Get total quantity of a FinishedProduct across all orders
        //GET: api/OrderProduct/product/{finishedProductId}/total
        [HttpGet("product/{finishedProductId}/total")]
        public async Task<IActionResult> GetTotalQuantityForFinishedProduct(int finishedProductId)
        {
            var totalQuantity = await _orderProductService.GetTotalQuantityForFinishedProductAsync(finishedProductId); //Calculate the total quantity of the finished product sold
            return Ok(totalQuantity); //Return the total quantity sold
        }

        //Get sales statistics for a specific product
        //GET: api/OrderProduct/sale-stats?productName={productName}
        [HttpGet("sale-stats")]
        public async Task<IActionResult> GetSaleStatsForProduct([FromQuery] string productName)
        {
            if (string.IsNullOrEmpty(productName))
            {
                return BadRequest("Product name cannot be empty."); // Return 400 if the product name is not provided
            }

            try
            {
                // Get the total quantity sold and total revenue for the given product name
                var saleStats = await _orderProductService.GetSaleStatsForProductAsync(productName);
                return Ok(saleStats); // Return the sales statistics
            }
            catch (InvalidOperationException ex)
            {
                // If the exception indicates the product was not found, return 404 NotFound
                return NotFound(ex.Message);
            }
        }
    }
}