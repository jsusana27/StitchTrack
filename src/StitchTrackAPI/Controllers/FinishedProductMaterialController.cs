using Microsoft.AspNetCore.Mvc;         //ASP.NET Core library for building controllers
using CrochetBusinessAPI.Models;        //Import model classes for database entities
using CrochetBusinessAPI.Services;      //Import service classes for business logic

namespace CrochetBusinessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinishedProductMaterialController : Controller<FinishedProductMaterial>
    {
        private readonly FinishedProductMaterialService _finishedProductMaterialService;
        private readonly FinishedProductService _finishedProductService;
        private readonly YarnService _yarnService;
        private readonly SafetyEyeService _safetyEyesService;
        private readonly StuffingService _stuffingService;

        public FinishedProductMaterialController(
            FinishedProductMaterialService finishedProductMaterialService,
            FinishedProductService finishedProductService,
            YarnService yarnService,
            SafetyEyeService safetyEyesService, StuffingService stuffingService)
            : base(finishedProductMaterialService)
        {
            _finishedProductMaterialService = finishedProductMaterialService;
            _finishedProductService = finishedProductService;
            _yarnService = yarnService;
            _safetyEyesService = safetyEyesService;
            _stuffingService = stuffingService;
        }

        //New endpoint to add a Yarn material to a specific Finished Product
        [HttpPost("add-material-yarn")]
        public async Task<IActionResult> AddYarnMaterialToFinishedProduct(
            [FromQuery] string productName,
            [FromQuery] string brand,
            [FromQuery] string fiberType,
            [FromQuery] int fiberWeight,
            [FromQuery] string color,
            [FromQuery] decimal quantityUsed)
        {
            //Step 1: Retrieve the FinishedProduct ID based on the product name
            var finishedProduct = await _finishedProductService.GetProductByNameAsync(productName);
            if (finishedProduct == null)
            {
                return NotFound($"Finished product '{productName}' not found.");
            }

            //Step 2: Retrieve the Yarn ID based on the provided yarn specifications
            var yarn = await _yarnService.GetYarnIdBySpecificationsAsync(brand, fiberType, fiberWeight, color);
            if (yarn == null)
            {
                return NotFound("Matching Yarn not found based on the provided specifications.");
            }

            //Step 3: Create a new FinishedProductMaterial instance
            var newMaterial = new FinishedProductMaterial
            {
                FinishedProductsID = finishedProduct.FinishedProductsID.GetValueOrDefault(),
                MaterialType = "Yarn",
                MaterialID = yarn.YarnID.GetValueOrDefault(),
                QuantityUsed = quantityUsed
            };

            //Step 4: Insert into the FinishedProductMaterials table
            var createdMaterial = await _finishedProductMaterialService.CreateAsync(newMaterial);
            if(createdMaterial is not null)
            {
                return CreatedAtAction(nameof(GetById), new { id = createdMaterial.FinishedProductMaterialsID }, createdMaterial);
            }
            else
            {
                //Return a BadRequest or Problem result indicating an error in material creation
                return BadRequest("Failed to create material. The created material was null.");
            }
        }

        //New endpoint to add Safety Eyes material to a specific Finished Product
        [HttpPost("add-material-safety-eyes")]
        public async Task<IActionResult> AddSafetyEyesMaterialToFinishedProduct(
            [FromQuery] string productName,
            [FromQuery] int size,
            [FromQuery] string color,
            [FromQuery] string shape,
            [FromQuery] decimal quantityUsed)
        {
            //Step 1: Retrieve the FinishedProduct ID based on the product name
            var finishedProduct = await _finishedProductService.GetProductByNameAsync(productName);
            if (finishedProduct == null)
            {
                return NotFound($"Finished product '{productName}' not found.");
            }

            //Step 2: Retrieve the Safety Eyes ID based on the provided specifications
            var safetyEyes = await _safetyEyesService.GetSafetyEyesIdBySpecificationsAsync(size, color, shape);
            if (safetyEyes == null)
            {
                return NotFound("Matching Safety Eyes not found based on the provided specifications.");
            }

            //Step 3: Create a new FinishedProductMaterial instance
            var newMaterial = new FinishedProductMaterial
            {
                FinishedProductsID = finishedProduct.FinishedProductsID.GetValueOrDefault(),
                MaterialType = "SafetyEyes",
                MaterialID = safetyEyes.SafetyEyesID.GetValueOrDefault(), //Use the SafetyEyes ID
                QuantityUsed = quantityUsed
            };

            //Step 4: Insert into the FinishedProductMaterials table
            var createdMaterial = await _finishedProductMaterialService.CreateAsync(newMaterial);
            if(createdMaterial is not null)
            {
                return CreatedAtAction(nameof(GetById), new { id = createdMaterial.FinishedProductMaterialsID }, createdMaterial);
            }
            else
            {
                //Return a BadRequest or Problem result indicating an error in material creation
                return BadRequest("Failed to create material. The created material was null.");
            }
        }

        //Get all finished products that use a specific material type
        [HttpGet("material/{materialType}/{materialId}")]
        public async Task<IActionResult> GetFinishedProductsByMaterial(string materialType, int materialId)
        {
            var products = await _finishedProductMaterialService.GetFinishedProductsByMaterialAsync(materialType, materialId);
            return Ok(products);
        }

        //Get all materials used in a specific finished product by ID or Name
        [HttpGet("get-materials-by-name")]
        public async Task<IActionResult> GetMaterialsByFinishedProduct([FromQuery] string? finishedProductName = null, [FromQuery] int? finishedProductId = null)
        {
            //Step 1: Check if the FinishedProductID is provided
            if (finishedProductId == null)
            {
                //If the FinishedProductID is null, ensure that we have a valid product name to work with
                if (string.IsNullOrEmpty(finishedProductName))
                {
                    return BadRequest("Please provide either a FinishedProductName or a FinishedProductID.");
                }

                //Step 2: Retrieve the FinishedProductID based on the product name
                var finishedProduct = await _finishedProductService.GetProductByNameAsync(finishedProductName);
                if (finishedProduct == null)
                {
                    return NotFound($"Finished product '{finishedProductName}' not found.");
                }

                //Assign the finished product's ID to finishedProductId
                finishedProductId = finishedProduct.FinishedProductsID;
            }

            //Step 3: Use the FinishedProductID to retrieve materials (finishedProductId is guaranteed to be non-null here)
            var materials = await _finishedProductMaterialService.GetMaterialsByFinishedProductIdAsync(finishedProductId.GetValueOrDefault());
            if (materials == null || materials.Count == 0)
            {
                return NotFound($"No materials found for product ID {finishedProductId}.");
            }

            return Ok(materials);
        }

        //Check if there is enough material available to make a product
        [HttpGet("can-make/{materialId}/{requiredQuantity}")]
        public async Task<IActionResult> CanMakeProduct(int materialId, decimal requiredQuantity)
        {
            var canMake = await _finishedProductMaterialService.CanMakeProductAsync(materialId, requiredQuantity);
            return Ok(canMake);
        }

        [HttpGet("detailed-materials-by-product/{productName}")]
        public async Task<IActionResult> GetDetailedMaterialsByProductName(string productName)
        {
            var materials = await _finishedProductMaterialService.GetDetailedMaterialsByProductNameAsync(productName);
            if (materials == null || materials.Count == 0)
            {
                return NotFound($"No materials found for product {productName}");
            }
            return Ok(materials);
        }

        //Endpoint to add a Stuffing material to a specific Finished Product
        [HttpPost("add-material-stuffing")]
        public async Task<IActionResult> AddStuffingMaterialToFinishedProduct(
            [FromQuery] string productName,
            [FromQuery] string brand,
            [FromQuery] string type,
            [FromQuery] decimal quantityUsed)
        {
            //Step 1: Retrieve the FinishedProduct ID based on the product name
            var finishedProduct = await _finishedProductService.GetProductByNameAsync(productName);
            if (finishedProduct == null)
            {
                return NotFound($"Finished product '{productName}' not found.");
            }

            //Step 2: Retrieve the Stuffing ID based on the provided specifications
            var stuffing = await _stuffingService.GetStuffingIdBySpecificationsAsync(brand, type);
            if (stuffing == null)
            {
                return NotFound("Matching Stuffing not found based on the provided specifications.");
            }

            //Step 3: Create a new FinishedProductMaterial instance
            var newMaterial = new FinishedProductMaterial
            {
                FinishedProductsID = finishedProduct.FinishedProductsID.GetValueOrDefault(),
                MaterialType = "Stuffing",
                MaterialID = stuffing.StuffingID, //Use the Stuffing ID
                QuantityUsed = quantityUsed
            };

            //Step 4: Insert into the FinishedProductMaterials table
            var createdMaterial = await _finishedProductMaterialService.CreateAsync(newMaterial);
            if(createdMaterial is not null)
            {
                return CreatedAtAction(nameof(GetById), new { id = createdMaterial.FinishedProductMaterialsID }, createdMaterial);
            }
            else
            {
                //Return a BadRequest or Problem result indicating an error in material creation
                return BadRequest("Failed to create material. The created material was null.");
            }
        }

        //Endpoint to update the quantity of yarn for a specific product material using query parameters
        [HttpPut("update-quantity-yarn")]
        public async Task<IActionResult> UpdateYarnQuantity(
            [FromQuery] string productName,
            [FromQuery] string brand,
            [FromQuery] string fiberType,
            [FromQuery] int fiberWeight,
            [FromQuery] string color,
            [FromQuery] decimal newQuantity)
        {
            //Step 1: Get the FinishedProductID using the product name
            var finishedProduct = await _finishedProductService.GetProductByNameAsync(productName);
            if (finishedProduct == null)
            {
                return NotFound($"Product '{productName}' not found.");
            }

            //Step 2: Get the Yarn ID using the yarn details provided
            var yarn = await _yarnService.GetYarnIdBySpecificationsAsync(brand, fiberType, fiberWeight, color);
            if (yarn == null)
            {
                return NotFound("Matching Yarn not found based on the provided specifications.");
            }

            //Step 3: Update the QuantityUsed for the specific FinishedProductMaterial
            var success = await _finishedProductMaterialService.UpdateYarnQuantityAsync(finishedProduct.FinishedProductsID.GetValueOrDefault(), yarn.YarnID.GetValueOrDefault(), newQuantity);
            if (!success)
            {
                return StatusCode(500, "Failed to update the quantity. Please try again.");
            }

            return Ok("Yarn quantity updated successfully.");
        }

        //Endpoint to update the quantity of safety eyes for a specific product material using query parameters
        [HttpPut("update-quantity-safety-eyes")]
        public async Task<IActionResult> UpdateSafetyEyesQuantity(
            [FromQuery] string productName,
            [FromQuery] int size,
            [FromQuery] string color,
            [FromQuery] string shape,
            [FromQuery] decimal newQuantity)
        {
            //Step 1: Get the FinishedProductID using the product name
            var finishedProduct = await _finishedProductService.GetProductByNameAsync(productName);
            if (finishedProduct == null)
            {
                return NotFound($"Product '{productName}' not found.");
            }

            //Step 2: Get the Safety Eyes ID using the safety eyes details provided
            var safetyEyes = await _safetyEyesService.GetSafetyEyesIdBySpecificationsAsync(size, color, shape);
            if (safetyEyes == null)
            {
                return NotFound("Matching Safety Eyes not found based on the provided specifications.");
            }

            //Step 3: Update the QuantityUsed for the specific FinishedProductMaterial
            var success = await _finishedProductMaterialService.UpdateSafetyEyesQuantityAsync(finishedProduct.FinishedProductsID.GetValueOrDefault(), safetyEyes.SafetyEyesID.GetValueOrDefault(), newQuantity);
            if (!success)
            {
                return StatusCode(500, "Failed to update the quantity. Please try again.");
            }

            return Ok("Safety Eyes quantity updated successfully.");
        }

        [HttpDelete("delete-material-yarn")]
        public async Task<IActionResult> DeleteYarnMaterial(
            [FromQuery] string productName,
            [FromQuery] string brand,
            [FromQuery] string fiberType,
            [FromQuery] int fiberWeight,
            [FromQuery] string color)
        {
            //Step 1: Retrieve the FinishedProduct ID based on product name
            var finishedProduct = await _finishedProductService.GetProductByNameAsync(productName);
            if (finishedProduct == null)
            {
                return NotFound($"Product '{productName}' not found.");
            }

            //Step 2: Retrieve the Yarn ID based on the provided yarn details
            var yarn = await _yarnService.GetYarnIdBySpecificationsAsync(brand, fiberType, fiberWeight, color);
            if (yarn == null)
            {
                return NotFound("Matching Yarn not found based on the provided specifications.");
            }

            //Step 3: Call the service method to delete the yarn material from the product
            var success = await _finishedProductMaterialService.DeleteYarnMaterialAsync(finishedProduct.FinishedProductsID.GetValueOrDefault(), yarn.YarnID.GetValueOrDefault());
            if (!success)
            {
                return StatusCode(500, "Failed to delete the yarn material. Please try again.");
            }

            return Ok("Yarn material deleted successfully.");
        }

        [HttpDelete("delete-material-safety-eyes")]
        public async Task<IActionResult> DeleteSafetyEyesMaterial(
            [FromQuery] string productName,
            [FromQuery] int size,
            [FromQuery] string color,
            [FromQuery] string shape)
        {
            //Step 1: Retrieve the FinishedProduct ID based on product name
            var finishedProduct = await _finishedProductService.GetProductByNameAsync(productName);
            if (finishedProduct == null)
            {
                return NotFound($"Product '{productName}' not found.");
            }

            //Step 2: Retrieve the Safety Eyes ID based on the provided details
            var safetyEyes = await _safetyEyesService.GetSafetyEyesIdBySpecificationsAsync(size, color, shape);
            if (safetyEyes == null)
            {
                return NotFound("Matching Safety Eyes not found based on the provided specifications.");
            }

            //Step 3: Call the service method to delete the Safety Eyes from the product
            var success = await _finishedProductMaterialService.DeleteSafetyEyesMaterialAsync(finishedProduct.FinishedProductsID.GetValueOrDefault(), safetyEyes.SafetyEyesID.GetValueOrDefault());
            if (!success)
            {
                return StatusCode(500, "Failed to delete the Safety Eyes. Please try again.");
            }

            return Ok("Safety Eyes deleted successfully.");
        }

        //Endpoint to delete Stuffing material from a specific Finished Product
        [HttpDelete("delete-material-stuffing")]
        public async Task<IActionResult> DeleteStuffingFromProduct(
            [FromQuery] string productName,
            [FromQuery] string brand,
            [FromQuery] string type)
        {
            //Step 1: Retrieve the FinishedProduct ID based on the product name
            var finishedProduct = await _finishedProductService.GetProductByNameAsync(productName);
            if (finishedProduct == null)
            {
                return NotFound($"Finished product '{productName}' not found.");
            }

            //Step 2: Retrieve the Stuffing ID based on the provided specifications
            var stuffing = await _stuffingService.GetStuffingIdBySpecificationsAsync(brand, type);
            if (stuffing == null)
            {
                return NotFound("Matching Stuffing not found based on the provided specifications.");
            }

            //Step 3: Delete the specific FinishedProductMaterial entry
            var success = await _finishedProductMaterialService.DeleteStuffingFromProductAsync(finishedProduct.FinishedProductsID.GetValueOrDefault(), stuffing.StuffingID);
            if (!success)
            {
                return StatusCode(500, "Failed to delete the stuffing. Please try again.");
            }

            return Ok("Stuffing deleted successfully from the product.");
        }
    }
}