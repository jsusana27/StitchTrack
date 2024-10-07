using Microsoft.AspNetCore.Mvc;         //Import ASP.NET Core MVC for building API controllers
using CrochetBusinessAPI.Models;        //Import data models representing Stuffing entities
using CrochetBusinessAPI.Services;      //Import service layer for business logic

namespace CrochetBusinessAPI.Controllers
{
    //API Controller for Stuffing-related operations
    [Route("api/[controller]")]
    [ApiController]
    public class StuffingController : Controller<Stuffing>
    {
        //Service instance for stuffing-specific operations
        private readonly StuffingService _stuffingService;

        //Constructor to initialize the StuffingController with a StuffingService instance
        public StuffingController(StuffingService stuffingService) : base(stuffingService)
        {
            _stuffingService = stuffingService;
        }

        //Get all available stuffing brands
        //GET: api/Stuffing/brands
        [HttpGet("brands")]
        public async Task<IActionResult> GetAllStuffingBrands()
        {
            var brands = await _stuffingService.GetAllStuffingBrandsAsync(); //Retrieve distinct stuffing brands
            return Ok(brands); //Return 200 OK with the list of brands
        }

        //Get all available stuffing types
        //GET: api/Stuffing/types
        [HttpGet("types")]
        public async Task<IActionResult> GetAllStuffingTypes()
        {
            var types = await _stuffingService.GetAllStuffingTypesAsync(); //Retrieve distinct stuffing types
            return Ok(types); //Return 200 OK with the list of types
        }

        //Get stuffing sorted by price per 5 lbs
        //GET: api/Stuffing/sorted-by-price
        [HttpGet("sorted-by-price")]
        public async Task<IActionResult> GetStuffingSortedByPricePer5Lbs()
        {
            var stuffing = await _stuffingService.GetStuffingSortedByPricePer5LbsAsync(); //Retrieve stuffing sorted by price
            return Ok(stuffing); //Return 200 OK with the sorted list
        }

        //Delete stuffing by specifying brand and type
        //DELETE: api/Stuffing/delete
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteStuffingByDetails([FromQuery] string brand, [FromQuery] string type)
        {
            var deletedStuffing = await _stuffingService.DeleteStuffingByDetailsAsync(brand, type); //Delete stuffing by brand and type

            if (deletedStuffing == null)
            {
                return NotFound("Stuffing not found with the specified brand and type."); //Return 404 Not Found if the specified stuffing is not found
            }

            return Ok(deletedStuffing); //Return 200 OK with the deleted stuffing entity
        }
    }
}