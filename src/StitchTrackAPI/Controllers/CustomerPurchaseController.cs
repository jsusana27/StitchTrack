using Microsoft.AspNetCore.Mvc;         //ASP.NET Core MVC for building API controllers
using CrochetBusinessAPI.Models;        //Import CustomerPurchase model for use in the controller
using CrochetBusinessAPI.Services;      //Import service layer for business logic related to CustomerPurchases

namespace CrochetBusinessAPI.Controllers
{
    //API Controller for CustomerPurchase-related operations
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerPurchaseController : Controller<CustomerPurchase>
    {
        private readonly CustomerPurchaseService _customerPurchaseService; //Reference to CustomerPurchaseService for handling business logic

        //Constructor to initialize CustomerPurchaseController with a specific service
        public CustomerPurchaseController(CustomerPurchaseService customerPurchaseService) : base(customerPurchaseService)
        {
            _customerPurchaseService = customerPurchaseService;
        }

        //Get all purchases made by a specific customer
        //GET: api/CustomerPurchase/customer/{customerId}
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetPurchasesByCustomerId(int customerId)
        {
            var purchases = await _customerPurchaseService.GetPurchasesByCustomerIdAsync(customerId); //Fetch purchases for a given customer ID
            return Ok(purchases); //Return the list of purchases for the specified customer
        }

        //Get all customers who purchased a specific finished product
        //GET: api/CustomerPurchase/product/{finishedProductId}
        [HttpGet("product/{finishedProductId}")]
        public async Task<IActionResult> GetCustomersByFinishedProductId(int finishedProductId)
        {
            var customers = await _customerPurchaseService.GetCustomersByFinishedProductIdAsync(finishedProductId); //Fetch customers by FinishedProductID
            return Ok(customers); //Return the list of customers who purchased the specified product
        }
    }
}