using Microsoft.EntityFrameworkCore;
using CrochetBusinessAPI.Data;
using CrochetBusinessAPI.Models;
using CrochetBusinessAPI.Repositories;
using CrochetBusinessAPI.Services;

namespace CrochetBusinessAPI.Tests.Services
{
    public class ServiceTests
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

        //Seed the in-memory database with sample data for testing
        private static void SeedData(CrochetDbContext context)
        {
            context.Yarns.AddRange(
                new Yarn
                {
                    Brand = "BrandA",
                    FiberType = "Chenille Polyester",
                    FiberWeight = 3,
                    Color = "Red",
                    Price = 5.99m,
                    NumberOfSkeinsOwned = 10,
                    YardagePerSkein = 100,
                    GramsPerSkein = 50
                },
                new Yarn
                {
                    Brand = "BrandB",
                    FiberType = "Chenille Polyester",
                    FiberWeight = 4,
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
        public async Task GetAllAsync_ShouldReturnAllEntities()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new Repository<Yarn>(context);
            var service = new Service<Yarn>(repository);

            //Act
            var yarns = await service.GetAllAsync();

            //Assert
            Assert.NotNull(yarns);
            Assert.Equal(2, yarns.Count); //Should return the 2 yarn entities in the seeded data
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntityIfExists()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new Repository<Yarn>(context);
            var service = new Service<Yarn>(repository);
            var yarn = context.Yarns.First();

            if (yarn.YarnID.HasValue)
            {
                //Act
                var retrievedYarn = await service.GetByIdAsync(yarn.YarnID.Value);

                //Assert
                Assert.NotNull(retrievedYarn);
                Assert.Equal(yarn.Brand, retrievedYarn?.Brand);
            }
            else
            {
                Assert.Fail("YarnID is null, cannot retrieve entity by ID.");
            }
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNullIfEntityDoesNotExist()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new Repository<Yarn>(context);
            var service = new Service<Yarn>(repository);

            //Act
            var retrievedYarn = await service.GetByIdAsync(-1); //Use an invalid ID

            //Assert
            Assert.Null(retrievedYarn);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddEntityToDatabase()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new Repository<Yarn>(context);
            var service = new Service<Yarn>(repository);
            var newYarn = new Yarn
            {
                Brand = "BrandC",
                FiberType = "Acrylic",
                FiberWeight = 5,
                Color = "Yellow",
                Price = 8.0m,
                NumberOfSkeinsOwned = 30,
                YardagePerSkein = 250,
                GramsPerSkein = 125
            };

            //Act
            var insertedYarn = await service.CreateAsync(newYarn);

            //Assert
            var yarns = await context.Yarns.ToListAsync();
            Assert.Equal(3, yarns.Count); //2 initial yarns + 1 new yarn
            Assert.NotNull(insertedYarn);
            Assert.Equal("BrandC", insertedYarn?.Brand);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateExistingEntity()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new Repository<Yarn>(context);
            var service = new Service<Yarn>(repository);
            var existingYarn = context.Yarns.First();
            existingYarn.Color = "Purple"; //Modify a property

            //Act
            var updatedYarn = await service.UpdateAsync(existingYarn);

            //Assert
            Assert.NotNull(updatedYarn);
            var yarn = await context.Yarns.FindAsync(existingYarn.YarnID);
            Assert.Equal("Purple", yarn?.Color); //The color should be updated to "Purple"
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNullIfEntityDoesNotExist()
        {
            //Arrange
            using var context = GetInMemoryDbContext();
            var repository = new Repository<Yarn>(context);
            var service = new Service<Yarn>(repository);
            var nonExistentYarn = new Yarn
            {
                YarnID = -1, //Invalid ID
                Brand = "NonExistent",
                FiberType = "Unknown",
                Color = "Invisible",
                FiberWeight = 0
            };

            //Act
            var result = await service.UpdateAsync(nonExistentYarn);

            //Assert
            Assert.Null(result); //Should return null since the entity doesn't exist
        }
    }
}