using Microsoft.EntityFrameworkCore;    //Import EntityFrameworkCore for database context and DbSet operations
using CrochetBusinessAPI.Data;          //Import the namespace containing the application's DbContext

namespace CrochetBusinessAPI.Repositories
{
    //Generic repository class for performing CRUD operations on a specified entity type TEntity
    public class Repository<TEntity> where TEntity : class
    {
        //Protected context for database access
        protected readonly CrochetDbContext Context;

        //Protected DbSet representing the table of TEntity in the database
        protected readonly DbSet<TEntity> EntitySet;

        //Constructor to initialize context and entity set
        public Repository(CrochetDbContext context)
        {
            Context = context; //Initialize the context
            EntitySet = context.Set<TEntity>(); //Get the DbSet<TEntity> for the specified entity
        }

        //Retrieve all entities of type TEntity asynchronously
        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await EntitySet.AsNoTracking().ToListAsync(); //Return a list of all entities without tracking
        }

        //Retrieve a specific entity by its ID asynchronously
        public virtual async Task<TEntity?> GetByIdAsync(int id)
        {
            return await EntitySet.FindAsync(id); //Use FindAsync to get the entity by its primary key
        }

        //Insert a new entity into the DbSet asynchronously
        public virtual async Task<TEntity?> InsertAsync(TEntity entity)
        {
            var item = await EntitySet.AddAsync(entity); //Add the entity to the DbSet
            await Context.SaveChangesAsync(); //Save changes to the database
            return item.Entity; //Return the added entity
        }

        //Update an existing entity in the DbSet asynchronously
        public virtual async Task<TEntity?> UpdateAsync(TEntity entity)
        {
            var item = EntitySet.Update(entity); //Mark the entity as updated in the DbSet

            try
            {
                await Context.SaveChangesAsync(); //Attempt to save changes to the database
            }
            catch (DbUpdateConcurrencyException) //Catch concurrency exceptions
            {
                if (!await EntitySet.AnyAsync(e => e.Equals(entity))) //Check if the entity exists in the DbSet
                {
                    return null; //If not found, return null
                }
                throw; //Rethrow the exception if the entity does exist
            }

            return item.Entity; //Return the updated entity
        }

        //Save changes to the database asynchronously
        public async Task SaveAsync()
        {
            await Context.SaveChangesAsync(); //Save all pending changes in the context
        }
    }
}