using Microsoft.AspNetCore.Mvc;         //ASP.NET Core MVC for building API controllers
using CrochetBusinessAPI.Models;        //Import Customer model
using CrochetBusinessAPI.Services;      //Import service layer for business logic

namespace CrochetBusinessAPI.Controllers
{
    //API Controller for Customer-related operations
    [Route("api/[controller]")]
    public class CustomerController : Controller<Customer>
    {
        private readonly CustomerService _customerService; //Reference to CustomerService for handling business logic

        //Constructor to initialize the CustomerController with a specific service
        public CustomerController(CustomerService customerService) : base(customerService)
        {
            _customerService = customerService;
        }

        //Update a customer's name by ID
        //PUT: api/Customer/{id}/name
        [HttpPut("{id}/name")]
        public async Task<IActionResult> UpdateCustomerName(int id, [FromBody] string newName)
        {
            var result = await _customerService.UpdateCustomerNameAsync(id, newName); //Update the name using the service method
            if (result == null) return NotFound(); //Return 404 if the customer is not found
            return Ok(result); //Return the updated customer object
        }

        //Update a customer's phone number by ID
        //PUT: api/Customer/{id}/phone
        [HttpPut("{id}/phone")]
        public async Task<IActionResult> UpdateCustomerPhone(int id, [FromBody] string newPhoneNumber)
        {
            var result = await _customerService.UpdateCustomerPhoneNumberAsync(id, newPhoneNumber); //Update the phone number using the service method
            if (result == null) return NotFound(); //Return 404 if the customer is not found
            return Ok(result); //Return the updated customer object
        }

        //Update a customer's email address by ID
        //PUT: api/Customer/{id}/email
        [HttpPut("{id}/email")]
        public async Task<IActionResult> UpdateCustomerEmail(int id, [FromBody] string newEmailAddress)
        {
            var result = await _customerService.UpdateCustomerEmailAddressAsync(id, newEmailAddress); //Update the email address using the service method
            if (result == null) return NotFound(); //Return 404 if the customer is not found
            return Ok(result); //Return the updated customer object
        }

        //Retrieve all customers in the database
        //GET: api/Customer/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _customerService.GetAllAsync(); //Get all customers using the service method
            if (customers == null || customers.Count == 0)
            {
                return NotFound("No customers found"); //Return 404 if no customers are found
            }
            return Ok(customers); //Return the list of customers
        }

        //Search for a customer by name
        //GET: api/Customer/search-by-name?name={name}
        [HttpGet("search-by-name")]
        public async Task<IActionResult> GetCustomerByName(string name)
        {
            var customer = await _customerService.GetCustomerByNameAsync(name); //Get customer by name using the service method
            if (customer == null)
            {
                return NotFound(); //Return 404 if the customer is not found
            }
            return Ok(customer); //Return the found customer object
        }

        //Delete a customer by name
        //DELETE: api/Customer/delete?name={name}
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteCustomerByName(string name)
        {
            var deletedCustomer = await _customerService.DeleteCustomerByNameAsync(name); //Delete customer by name using the service method
            if (deletedCustomer == null) return NotFound(); //Return 404 if the customer is not found
            return Ok(deletedCustomer); //Return the deleted customer object
        }

        //Update a customer's details (phone number, email, etc.)
        //PUT: api/Customer/update-customer-details
        [HttpPut("update-customer-details")]
        public async Task<IActionResult> UpdateCustomerDetails([FromBody] Customer updatedCustomer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); //Return 400 if model state is invalid

            var existingCustomer = await _customerService.GetCustomerByNameAsync(updatedCustomer.Name); //Get customer by name
            if (existingCustomer == null)
                return NotFound(); //Return 404 if customer is not found

            //Update phone number and email address if they are provided
            if (updatedCustomer.PhoneNumber != null)
                existingCustomer.PhoneNumber = updatedCustomer.PhoneNumber;

            if (updatedCustomer.EmailAddress != null)
                existingCustomer.EmailAddress = updatedCustomer.EmailAddress;

            var updatedEntity = await _service.UpdateAsync(existingCustomer); //Update the customer using the base service
            return Ok(updatedEntity); //Return the updated customer object
        }
    }
}