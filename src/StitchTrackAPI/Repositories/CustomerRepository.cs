using Microsoft.EntityFrameworkCore;        //Import EntityFrameworkCore for database context and DbSet operations
using CrochetBusinessAPI.Models;            //Import the models containing the Customer entity
using CrochetBusinessAPI.Data;              //Import the namespace containing the application's DbContext

namespace CrochetBusinessAPI.Repositories
{
    //Repository class for managing Customer-specific database operations
    public class CustomerRepository : Repository<Customer>
    {
        //Private readonly context for database operations specific to CustomerRepository
        private readonly CrochetDbContext _context;
        
        //Constructor that initializes the base repository with the given context
        public CustomerRepository(CrochetDbContext context) : base(context)
        {
            _context = context;
        }

        //Method to get all customer information
        public override async Task<List<Customer>> GetAllAsync()
        {
            return await EntitySet
                .AsNoTracking()
                .ToListAsync();
        }

        //Method to update customer name
        public async Task<Customer?> UpdateCustomerNameAsync(int customerId, string newName)
        {
            var customer = await EntitySet.FindAsync(customerId);
            if (customer == null) return null;

            customer.Name = newName;
            await Context.SaveChangesAsync();
            return customer;
        }

        //Method to update customer phone number
        public async Task<Customer?> UpdateCustomerPhoneNumberAsync(int customerId, string newPhoneNumber)
        {
            var customer = await EntitySet.FindAsync(customerId);
            if (customer == null) return null;

            customer.PhoneNumber = newPhoneNumber;
            await Context.SaveChangesAsync();
            return customer;
        }

        //Method to update customer email address
        public async Task<Customer?> UpdateCustomerEmailAddressAsync(int customerId, string newEmailAddress)
        {
            var customer = await EntitySet.FindAsync(customerId);
            if (customer == null) return null;

            customer.EmailAddress = newEmailAddress;
            await Context.SaveChangesAsync();
            return customer;
        }

        //Method to get a customer by name
        public async Task<Customer?> GetCustomerByNameAsync(string name)
        {
            return await EntitySet.FirstOrDefaultAsync(c => c.Name == name);
        }

        //Method to delete a customer by name
        public async Task<Customer?> DeleteCustomerByNameAsync(string name)
        {
            var customerToDelete = await EntitySet.FirstOrDefaultAsync(c => c.Name == name);
            if (customerToDelete == null) return null;

            EntitySet.Remove(customerToDelete);
            await _context.SaveChangesAsync();
            return customerToDelete;
        }

        //Method to create a new customer
        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }
    }
}