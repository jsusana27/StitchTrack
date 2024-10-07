using Microsoft.AspNetCore.Mvc;         //Import ASP.NET Core MVC for API routing and controllers
using CrochetBusinessAPI.Models;        //Import models representing the data structures
using CrochetBusinessAPI.Services;      //Import service layer for business logic

namespace CrochetBusinessAPI.Controllers
{
    //API Controller for Yarn-related operations
    [Route("api/[controller]")]
    [ApiController]
    public class YarnController : Controller<Yarn>
    {
        //Service for yarn-specific operations
        private readonly YarnService _yarnService;

        //Constructor to initialize the yarn controller
        public YarnController(YarnService yarnService) : base(yarnService)
        {
            _yarnService = yarnService;
        }

        //Get all yarn brands
        [HttpGet("brands")]
        public async Task<IActionResult> GetYarnBrands()
        {
            var brands = await _yarnService.GetAllYarnBrandsAsync(); //Retrieve all distinct yarn brands
            return Ok(brands); //Return 200 OK with the list of brands
        }

        //Get all colors
        [HttpGet("colors")]
        public async Task<IActionResult> GetAllYarnColors()
        {
            var colors = await _yarnService.GetAllYarnColorsAsync(); //Retrieve all distinct yarn colors
            return Ok(colors); //Return 200 OK with the list of colors
        }

        //Get all fiber types
        [HttpGet("fiber-types")]
        public async Task<IActionResult> GetAllYarnFiberTypes()
        {
            var fiberTypes = await _yarnService.GetAllYarnFiberTypesAsync(); //Retrieve all distinct yarn fiber types
            return Ok(fiberTypes); //Return 200 OK with the list of fiber types
        }

        //Get all fiber weights
        [HttpGet("fiber-weights")]
        public async Task<IActionResult> GetAllYarnFiberWeights()
        {
            var fiberWeights = await _yarnService.GetAllYarnFiberWeightsAsync(); //Retrieve all distinct yarn fiber weights
            return Ok(fiberWeights); //Return 200 OK with the list of fiber weights
        }

        //Get yarn sorted by price
        [HttpGet("sorted-by-price")]
        public async Task<IActionResult> GetAllYarnSortedByPrice()
        {
            var yarns = await _yarnService.GetAllYarnSortedByPriceAsync(); //Retrieve all yarns sorted by price
            return Ok(yarns); //Return 200 OK with the sorted list of yarns
        }

        //Get yarn sorted by yardage per skein
        [HttpGet("sorted-by-yardage")]
        public async Task<IActionResult> GetAllYarnSortedByYardagePerSkein()
        {
            var yarns = await _yarnService.GetAllYarnSortedByYardagePerSkeinAsync(); //Retrieve all yarns sorted by yardage
            return Ok(yarns); //Return 200 OK with the sorted list of yarns
        }

        //Get yarn sorted by grams per skein
        [HttpGet("sorted-by-grams")]
        public async Task<IActionResult> GetAllYarnSortedByGramsPerSkein()
        {
            var yarns = await _yarnService.GetAllYarnSortedByGramsPerSkeinAsync(); //Retrieve all yarns sorted by grams per skein
            return Ok(yarns); //Return 200 OK with the sorted list of yarns
        }

        //Get yarn sorted by the number in stock
        [HttpGet("sorted-by-number-in-stock")]
        public async Task<IActionResult> GetAllYarnSortedByNumberInStock()
        {
            var yarns = await _yarnService.GetAllYarnSortedByNumberInStockAsync(); //Retrieve all yarns sorted by stock
            return Ok(yarns); //Return 200 OK with the sorted list of yarns
        }

        //Check if a specific yarn exists based on its properties
        [HttpGet("check-existence")]
        public async Task<IActionResult> CheckYarnExistence(
            [FromQuery] string brand, [FromQuery] string fiberType, [FromQuery] int fiberWeight, [FromQuery] string color)
        {
            var exists = await _yarnService.CheckYarnExistsAsync(brand, fiberType, fiberWeight, color); //Check if the yarn exists
            return Ok(new { exists }); //Return 200 OK with the existence status
        }

        //Get the ID of a yarn based on its properties
        [HttpGet("get-yarn-id")]
        public async Task<IActionResult> GetYarnId(
            [FromQuery] string brand, [FromQuery] string fiberType,
            [FromQuery] int fiberWeight, [FromQuery] string color)
        {
            var yarn = await _yarnService.GetYarnIdBySpecificationsAsync(brand, fiberType, fiberWeight, color); //Get yarn by specifications

            if (yarn == null)
            {
                return NotFound(new { message = "Yarn not found with the given specifications." }); //Return 404 Not Found if yarn is not found
            }

            return Ok(new { yarnId = yarn.YarnID }); //Return 200 OK with the yarn ID
        }

        //Update the quantity of a specific yarn
        //PUT: api/Yarn/update-yarn-quantity
        [HttpPut("update-quantity")]
        public async Task<IActionResult> UpdateYarnQuantity([FromBody] Yarn yarnDetails)
        {
            var updated = await _yarnService.UpdateYarnQuantityAsync(
                yarnDetails.Brand,
                yarnDetails.FiberType,
                yarnDetails.FiberWeight.GetValueOrDefault(),
                yarnDetails.Color,
                yarnDetails.NumberOfSkeinsOwned.GetValueOrDefault());

            if (!updated) return NotFound("Yarn not found or update failed."); //Return 404 Not Found if update failed
            return Ok("Yarn quantity updated successfully."); //Return 200 OK if update is successful
        }

        //Delete a yarn by its details
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteYarnByDetails([FromBody] Yarn yarn)
        {
            if (yarn == null || string.IsNullOrEmpty(yarn.Brand) || string.IsNullOrEmpty(yarn.Color))
            {
                return BadRequest("Invalid yarn details provided."); //Return 400 Bad Request if details are invalid
            }

            var deletedYarn = await _yarnService.DeleteYarnByDetailsAsync(yarn.Brand, yarn.FiberType, yarn.FiberWeight, yarn.Color);
            if (deletedYarn == null)
            {
                return NotFound("Yarn not found."); //Return 404 Not Found if the yarn does not exist
            }

            return Ok("Yarn deleted successfully."); //Return 200 OK if deletion is successful
        }
    }
}