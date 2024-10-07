using Microsoft.AspNetCore.Mvc;         //Import ASP.NET Core MVC for building API controllers
using CrochetBusinessAPI.Models;        //Import data models representing FinishedProduct entities
using CrochetBusinessAPI.Services;      //Import service layer for business logic

namespace CrochetBusinessAPI.Controllers
{
    //API Controller for FinishedProduct-related operations
    [Route("api/[controller]")]
    [ApiController]
    public class FinishedProductController : Controller<FinishedProduct>
    {
        //Service instance for finished product-specific operations
        private readonly FinishedProductService _finishedProductService;

        //Constructor to initialize the FinishedProductController with a FinishedProductService instance
        public FinishedProductController(FinishedProductService finishedProductService) : base(finishedProductService)
        {
            _finishedProductService = finishedProductService;
        }

        //Get all finished product names
        //GET: api/FinishedProduct/names
        [HttpGet("names")]
        public async Task<IActionResult> GetAllProductNames()
        {
            var names = await _finishedProductService.GetAllProductNamesAsync(); //Retrieve distinct product names
            return Ok(names); //Return 200 OK with the list of product names
        }

        //Get finished products sorted by time to make
        //GET: api/FinishedProduct/sorted-by-time
        [HttpGet("sorted-by-time")]
        public async Task<IActionResult> GetProductsSortedByTimeToMake()
        {
            var products = await _finishedProductService.GetProductsSortedByTimeToMakeAsync(); //Retrieve products sorted by time
            return Ok(products); //Return 200 OK with the sorted list
        }

        //Get finished products sorted by cost to make
        //GET: api/FinishedProduct/sorted-by-cost
        [HttpGet("sorted-by-cost")]
        public async Task<IActionResult> GetProductsSortedByCostToMake()
        {
            var products = await _finishedProductService.GetProductsSortedByCostToMakeAsync(); //Retrieve products sorted by cost
            return Ok(products); //Return 200 OK with the sorted list
        }

        //Get finished products sorted by sale price
        //GET: api/FinishedProduct/sorted-by-price
        [HttpGet("sorted-by-price")]
        public async Task<IActionResult> GetProductsSortedBySalePrice()
        {
            var products = await _finishedProductService.GetProductsSortedBySalePriceAsync(); //Retrieve products sorted by price
            return Ok(products); //Return 200 OK with the sorted list
        }

        //Get finished products sorted by number in stock
        //GET: api/FinishedProduct/sorted-by-stock
        [HttpGet("sorted-by-stock")]
        public async Task<IActionResult> GetProductsSortedByNumberInStock()
        {
            var products = await _finishedProductService.GetProductsSortedByNumberInStockAsync(); //Retrieve products sorted by stock
            return Ok(products); //Return 200 OK with the sorted list
        }

        //Endpoint to search for a product by name
        //GET: api/FinishedProduct/search-by-name
        [HttpGet("search-by-name")]
        public async Task<IActionResult> GetProductByName([FromQuery] string name)
        {
            var product = await _finishedProductService.GetProductByNameAsync(name); //Retrieve product by name

            if (product == null)
            {
                return NotFound(new { message = "Product not found" }); //Return 404 if product is not found
            }

            return Ok(product); //Return 200 OK with the product details
        }

        //Get the product ID by name
        //GET: api/FinishedProduct/get-id-by-name
        [HttpGet("get-id-by-name")]
        public async Task<IActionResult> GetProductIdByName([FromQuery] string name)
        {
            var product = await _finishedProductService.GetProductByNameAsync(name); //Retrieve product by name

            if (product == null)
            {
                return NotFound(new { message = "Product not found" }); //Return 404 if product is not found
            }

            return Ok(new { productId = product.FinishedProductsID }); //Return 200 OK with the product ID
        }

        //Delete a product by its name
        //DELETE: api/FinishedProduct/delete
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteProductByName([FromQuery] string name)
        {
            var deletedProduct = await _finishedProductService.DeleteProductByNameAsync(name); //Delete product by name
            if (deletedProduct == null)
            {
                return NotFound("Product not found."); //Return 404 if product not found
            }

            return Ok(deletedProduct); //Return 200 OK with the deleted product entity
        }

        //Update sale price of a finished product
        //PUT: api/FinishedProduct/update-sale-price
        [HttpPut("update-sale-price")]
        public async Task<IActionResult> UpdateSalePrice([FromBody] FinishedProduct product)
        {
            if (string.IsNullOrEmpty(product.Name) || product.SalePrice <= 0)
            {
                return BadRequest("Invalid product details."); //Return 400 Bad Request for invalid input
            }

            var existingProduct = await _finishedProductService.GetProductByNameAsync(product.Name); //Check if product exists
            if (existingProduct == null) 
            {
                return NotFound("Product not found."); //Return 404 if not found
            }

            existingProduct.SalePrice = product.SalePrice; //Update sale price
            var result = await _service.UpdateAsync(existingProduct); //Save updated product

            return Ok(result); //Return 200 OK with updated product entity
        }

        //Update the time to make for a finished product
        //PUT: api/FinishedProduct/update-product-time
        [HttpPut("update-product-time")]
        public async Task<IActionResult> UpdateProductTime([FromBody] FinishedProduct request)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState); //Return 400 if model state is invalid
            }

            var product = await _finishedProductService.GetProductByNameAsync(request.Name); //Retrieve product by name
            if (product == null) 
            {
                return NotFound("Product not found."); //Return 404 if not found
            }

            product.TimeToMake = request.TimeToMake; //Update time to make
            var updatedProduct = await _service.UpdateAsync(product); //Save updated product

            return Ok(updatedProduct); //Return 200 OK with updated product entity
        }

        //Check if a finished product exists by name
        //GET: api/FinishedProduct/check-existence
        [HttpGet("check-existence")]
        public async Task<IActionResult> CheckFinishedProductExistence([FromQuery] string name)
        {
            var exists = await _finishedProductService.CheckFinishedProductExistsAsync(name); //Check product existence
            return Ok(new { exists }); //Return 200 OK with the existence status
        }

        //Update the quantity of a finished product
        //PUT: api/FinishedProduct/update-quantity
        [HttpPut("update-quantity")]
        public async Task<IActionResult> UpdateFinishedProductQuantity([FromBody] FinishedProduct finishedProductDetails)
        {
            var updated = await _finishedProductService.UpdateFinishedProductQuantityAsync(
                finishedProductDetails.Name, //Name of the product
                finishedProductDetails.NumberInStock.GetValueOrDefault()); //New stock quantity

            if (!updated) 
            {
                return NotFound("Finished Product not found or update failed."); //Return 404 if update failed
            }
            return Ok("Finished Product quantity updated successfully."); //Return 200 OK if successful
        }
    }
}