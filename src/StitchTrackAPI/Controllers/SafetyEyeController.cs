using Microsoft.AspNetCore.Mvc;         //Import ASP.NET Core MVC for building API controllers
using CrochetBusinessAPI.Models;        //Import data models representing SafetyEye entities
using CrochetBusinessAPI.Services;      //Import service layer for business logic

namespace CrochetBusinessAPI.Controllers
{
    //API Controller for SafetyEye-related operations
    [Route("api/[controller]")]
    [ApiController]
    public class SafetyEyeController : Controller<SafetyEye>
    {
        //Service for safety-eye-specific operations
        private readonly SafetyEyeService _safetyEyeService;

        //Constructor to initialize the SafetyEyeController
        public SafetyEyeController(SafetyEyeService safetyEyeService) : base(safetyEyeService)
        {
            _safetyEyeService = safetyEyeService;
        }

        //Get all available sizes of Safety Eyes
        //GET: api/SafetyEye/sizes
        [HttpGet("sizes")]
        public async Task<IActionResult> GetAllSafetyEyeSizes()
        {
            var sizes = await _safetyEyeService.GetAllSafetyEyeSizesAsync(); //Retrieve distinct sizes
            return Ok(sizes); //Return 200 OK with the list of sizes
        }

        //Get all available colors of Safety Eyes
        //GET: api/SafetyEye/colors
        [HttpGet("colors")]
        public async Task<IActionResult> GetAllSafetyEyeColors()
        {
            var colors = await _safetyEyeService.GetAllSafetyEyeColorsAsync(); //Retrieve distinct colors
            return Ok(colors); //Return 200 OK with the list of colors
        }

        //Get all available shapes of Safety Eyes
        //GET: api/SafetyEye/shapes
        [HttpGet("shapes")]
        public async Task<IActionResult> GetAllSafetyEyeShapes()
        {
            var shapes = await _safetyEyeService.GetAllSafetyEyeShapesAsync(); //Retrieve distinct shapes
            return Ok(shapes); //Return 200 OK with the list of shapes
        }

        //Get Safety Eyes sorted by price
        //GET: api/SafetyEye/sorted-by-price
        [HttpGet("sorted-by-price")]
        public async Task<IActionResult> GetSafetyEyesSortedByPrice()
        {
            var safetyEyes = await _safetyEyeService.GetSafetyEyesSortedByPriceAsync(); //Retrieve Safety Eyes sorted by price
            return Ok(safetyEyes); //Return 200 OK with the sorted list
        }

        //Get Safety Eyes sorted by number in stock
        //GET: api/SafetyEye/sorted-by-stock
        [HttpGet("sorted-by-stock")]
        public async Task<IActionResult> GetSafetyEyesSortedByStock()
        {
            var safetyEyes = await _safetyEyeService.GetSafetyEyesSortedByStockAsync(); //Retrieve Safety Eyes sorted by stock
            return Ok(safetyEyes); //Return 200 OK with the sorted list
        }

        //Delete Safety Eyes based on details such as size, color, and shape
        //DELETE: api/SafetyEye/delete
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteSafetyEyesByDetails([FromBody] SafetyEye details)
        {
            var deletedSafetyEye = await _safetyEyeService.DeleteSafetyEyesByDetailsAsync(
                details.SizeInMM.GetValueOrDefault(), details.Color, details.Shape); //Delete Safety Eye by details

            if (deletedSafetyEye == null)
            {
                return NotFound("Safety Eyes not found with the specified details."); //Return 404 Not Found if not found
            }

            return Ok(deletedSafetyEye); //Return 200 OK with the deleted entity
        }

        //Check if a specific Safety Eye exists by size, color, and shape
        //GET: api/SafetyEye/check-existence
        [HttpGet("check-existence")]
        public async Task<IActionResult> CheckSafetyEyeExistence([FromQuery] int sizeInMM, [FromQuery] string color, [FromQuery] string shape)
        {
            var exists = await _safetyEyeService.SafetyEyeExistsAsync(sizeInMM, color, shape); //Check if Safety Eye exists
            return Ok(new { exists }); //Return 200 OK with existence status
        }

        //Update the quantity of a specific Safety Eye
        //PUT: api/SafetyEye/update-quantity
        [HttpPut("update-quantity")]
        public async Task<IActionResult> UpdateSafetyEyeQuantity([FromBody] SafetyEye safetyEyeDetails)
        {
            var updated = await _safetyEyeService.UpdateSafetyEyeQuantityAsync(
                safetyEyeDetails.SizeInMM.GetValueOrDefault(),
                safetyEyeDetails.Color,
                safetyEyeDetails.Shape,
                safetyEyeDetails.NumberOfEyesOwned.GetValueOrDefault()); //Update the quantity

            if (!updated)
            {
                return NotFound("Safety Eye not found or update failed."); //Return 404 Not Found if update failed
            }

            return Ok("Safety Eye quantity updated successfully."); //Return 200 OK if update is successful
        }
    }
}