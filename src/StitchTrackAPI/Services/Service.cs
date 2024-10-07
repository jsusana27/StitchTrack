using CrochetBusinessAPI.Repositories;      //Import the Repository layer for data access operations

namespace CrochetBusinessAPI.Services
{
    //Generic service class for CRUD operations on entities
    public class Service<TEntity> where TEntity : class
    {
        //Protected repository field to interact with the database
        protected readonly Repository<TEntity> _repository;

        //Constructor to initialize the service with a specific repository
        public Service(Repository<TEntity> repository)
        {
            _repository = repository;
        }

        //Retrieve all entities from the repository
        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        //Retrieve a single entity by its ID from the repository
        public virtual async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        //Insert a new entity into the repository
        public virtual async Task<TEntity?> CreateAsync(TEntity entity)
        {
            return await _repository.InsertAsync(entity);
        }

        //Update an existing entity in the repository
        public virtual async Task<TEntity?> UpdateAsync(TEntity entity)
        {
            return await _repository.UpdateAsync(entity);
        }
    }
}