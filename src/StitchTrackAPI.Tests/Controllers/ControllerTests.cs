using Microsoft.EntityFrameworkCore;                
using CrochetBusinessAPI.Data;                     
using CrochetBusinessAPI.Models;                   
using CrochetBusinessAPI.Repositories;             
using CrochetBusinessAPI.Services;                   
using CrochetBusinessAPI.Controllers;               
using Microsoft.AspNetCore.Mvc;                       

namespace CrochetBusinessAPI.Tests.Controllers
{
    public class ControllerTests
    {
        //Helper method to create a new in-memory database context for each test
        private static CrochetDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<CrochetDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) //Use a unique database name for each test
                .Options;

            var context = new CrochetDbContext(options);
            SeedData(context); //Seed initial data
            return context;
        }

        //Seed the in-memory database with sample Yarn data for testing
        private static void SeedData(CrochetDbContext context)
        {
            context.Yarns.AddRange(
                new Yarn 
                { 
                    YarnID = 1, 
                    Brand = "BrandA", 
                    FiberType = "Cotton", 
                    Color = "Red", 
                    Price = 5.99m, 
                    NumberOfSkeinsOwned = 10, 
                    YardagePerSkein = 100, 
                    GramsPerSkein = 50 
                },
                new Yarn 
                { 
                    YarnID = 2, 
                    Brand = "BrandB", 
                    FiberType = "Wool", 
                    Color = "Blue", 
                    Price = 7.99m, 
                    NumberOfSkeinsOwned = 20, 
                    YardagePerSkein = 200, 
                    GramsPerSkein = 100 
                }
            );
            context.SaveChanges();
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllYarns()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new Repository<Yarn>(context);
            var service = new Service<Yarn>(repository);
            var controller = new Controller<Yarn>(service);

            //Act
            var result = await controller.GetAll();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var returnedYarns = Assert.IsType<List<Yarn>>(okResult.Value);
            Assert.Equal(2, returnedYarns.Count); //Should return 2 Yarn entities in the seeded data
        }

        [Fact]
        public async Task GetById_ShouldReturnYarnIfExists()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new Repository<Yarn>(context);
            var service = new Service<Yarn>(repository);
            var controller = new Controller<Yarn>(service);
            var yarn = context.Yarns.First();

            //Act
            var result = await controller.GetById(yarn.YarnID.GetValueOrDefault());

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var returnedYarn = Assert.IsType<Yarn>(okResult.Value);
            Assert.Equal(yarn.Brand, returnedYarn?.Brand); //Check if the brand matches the seeded data
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFoundIfYarnDoesNotExist()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new Repository<Yarn>(context);
            var service = new Service<Yarn>(repository);
            var controller = new Controller<Yarn>(service);

            //Act
            var result = await controller.GetById(-1); //Use an invalid ID

            //Assert
            Assert.IsType<NotFoundResult>(result); //Should return 404 Not Found
        }

        [Fact]
        public async Task Create_ShouldAddYarnToDatabase()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new Repository<Yarn>(context);
            var service = new Service<Yarn>(repository);
            var controller = new Controller<Yarn>(service);
            var newYarn = new Yarn 
            { 
                YarnID = 3, 
                Brand = "BrandC", 
                FiberType = "Silk", 
                Color = "Green", 
                Price = 9.99m, 
                NumberOfSkeinsOwned = 15, 
                YardagePerSkein = 150, 
                GramsPerSkein = 75 
            };
            
            //Act
            var result = await controller.Create(newYarn);

            //Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
            var returnedYarn = Assert.IsType<Yarn>(createdResult.Value);
            Assert.Equal("BrandC", returnedYarn?.Brand);

            var yarns = await context.Yarns.ToListAsync();
            Assert.Equal(3, yarns.Count); //Should have 3 Yarn entities after addition
            Assert.Contains(yarns, e => e.Brand == "BrandC");
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenModelIsInvalid()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new Repository<Yarn>(context);
            var service = new Service<Yarn>(repository);
            var controller = new Controller<Yarn>(service);

            //Simulate invalid model state
            controller.ModelState.AddModelError("Brand", "Required");

            //Act
            var result = await controller.Create(new Yarn { YarnID = 3, FiberType = "Silk", Color = "Green" }); //Missing required Brand field

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.IsType<SerializableError>(badRequestResult.Value); //Check for model state error
        }
    }
}
