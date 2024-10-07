using Microsoft.AspNetCore.Mvc;         //Import necessary ASP.NET Core MVC namespaces
using CrochetBusinessAPI.Services;      //Import services for handling business logic

namespace CrochetBusinessAPI.Controllers
{
    //Base controller class for handling CRUD operations for a given entity type
    [ApiController]
    [Route("api/[controller]")]
    public class Controller<TEntity> : ControllerBase where TEntity : class
    {
        //Service layer for handling entity operations
        protected readonly Service<TEntity> _service;

        //Constructor to initialize the controller with the service
        public Controller(Service<TEntity> service)
        {
            _service = service;
        }

        //Get all entities
        //GET: api/[controller]
        [HttpGet]
        public virtual async Task<IActionResult> GetAll()
        {
            var entities = await _service.GetAllAsync(); //Retrieve all entities using the service
            return Ok(entities); //Return 200 OK with the list of entities
        }

        //Get an entity by its ID
        //GET: api/[controller]/{id}
        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(int id)
        {
            var entity = await _service.GetByIdAsync(id); //Retrieve entity by ID using the service
            if (entity == null) 
            {
                return NotFound(); //Return 404 Not Found if entity does not exist
            }
            return Ok(entity); //Return 200 OK with the retrieved entity
        }

        //Create a new entity
        //POST: api/[controller]
        [HttpPost]
        public virtual async Task<IActionResult> Create([FromBody] TEntity entity)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState); //Return 400 Bad Request if model state is invalid

            var createdEntity = await _service.CreateAsync(entity); //Create the entity using the service
            return CreatedAtAction(nameof(GetById), new { id = createdEntity }, createdEntity); //Return 201 Created with the created entity
        }
    }
}