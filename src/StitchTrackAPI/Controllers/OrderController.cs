using Microsoft.AspNetCore.Mvc;         //ASP.NET Core MVC for building API controllers
using CrochetBusinessAPI.Models;        //Import Order model
using CrochetBusinessAPI.Services;      //Import Order service layer for business logic

namespace CrochetBusinessAPI.Controllers
{
    //API Controller for Order-related operations
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller<Order>
    {
        private readonly OrderService _orderService; //Reference to OrderService for handling business logic

        //Constructor to initialize the OrderController with a specific service
        public OrderController(OrderService orderService) : base(orderService)
        {
            _orderService = orderService;
        }

        //Override GET to get all orders with related customer and order products
        //GET: api/Order
        [HttpGet]
        public override async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAllAsync(); //Get all orders using the overridden service method
            return Ok(orders); //Return the list of orders
        }

        //Delete an order by customer name and date
        //DELETE: api/Order/delete?customerName={customerName}&orderDate={orderDate}
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteOrderByCustomerAndDate([FromQuery] string customerName, [FromQuery] string orderDate)
        {
            //Parse the order date using the exact format
            if (!DateTime.TryParseExact(orderDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime parsedOrderDate))
            {
                return BadRequest("Invalid date format. Please use YYYY-MM-DD format."); //Return 400 if the date format is invalid
            }

            //Delete the order by customer name and parsed order date
            var deleted = await _orderService.DeleteOrderByCustomerAndDateAsync(customerName, parsedOrderDate);
            if (!deleted) return NotFound($"No order found for customer '{customerName}' on date '{orderDate}'."); //Return 404 if the order is not found

            return Ok($"Order for customer '{customerName}' on date '{orderDate}' deleted successfully."); //Return a success message
        }

        //Endpoint to create a new order with customer name, order date, and products
        //POST: api/Order/create?customerName={customerName}&orderDate={orderDate}&formOfPayment={formOfPayment}&productNames={productNames}&quantities={quantities}
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder(
            [FromQuery] string customerName, //Name of the customer placing the order
            [FromQuery] string orderDate, //Order date in YYYY-MM-DD format
            [FromQuery] string formOfPayment, //Payment method for the order
            [FromQuery] List<string> productNames, //List of product names to be purchased
            [FromQuery] List<int> quantities) //Corresponding quantities for each product
        {
            //Validate that both productNames and quantities are provided and have the same count
            if (productNames == null || quantities == null || productNames.Count != quantities.Count)
            {
                return BadRequest("Please ensure that both 'productNames' and 'quantities' are provided with matching counts."); //Return 400 if validation fails
            }

            //Use the CreateOrderAsync method from the service with the required parameters
            var result = await _orderService.CreateOrderAsync(customerName, orderDate, formOfPayment, productNames, quantities);
            if (result)
            {
                return Ok("Order created successfully!"); //Return success message if the order was created successfully
            }

            return StatusCode(500, "Failed to create order."); //Return 500 if the order creation failed
        }
    }
}