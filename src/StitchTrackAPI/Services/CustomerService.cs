using CrochetBusinessAPI.Models;        //Import models used in the service layer
using CrochetBusinessAPI.Repositories;  //Import repositories for data operations

namespace CrochetBusinessAPI.Services
{
    //Service class for managing operations related to Customer
    public class CustomerService : Service<Customer>
    {
        //Repository specific to Customer operations
        private readonly CustomerRepository _customerRepository;

        //Constructor to initialize CustomerService with the repository
        public CustomerService(CustomerRepository customerRepository) : base(customerRepository)
        {
            _customerRepository = customerRepository;
        }

        //Update the name of a customer by ID
        public async Task<Customer?> UpdateCustomerNameAsync(int customerId, string newName)
        {
            return await _customerRepository.UpdateCustomerNameAsync(customerId, newName);
        }

        //Update the phone number of a customer by ID
        public async Task<Customer?> UpdateCustomerPhoneNumberAsync(int customerId, string newPhoneNumber)
        {
            return await _customerRepository.UpdateCustomerPhoneNumberAsync(customerId, newPhoneNumber);
        }

        //Update the email address of a customer by ID
        public async Task<Customer?> UpdateCustomerEmailAddressAsync(int customerId, string newEmailAddress)
        {
            return await _customerRepository.UpdateCustomerEmailAddressAsync(customerId, newEmailAddress);
        }

        //Retrieve a customer by name
        public async Task<Customer?> GetCustomerByNameAsync(string name)
        {
            return await _customerRepository.GetCustomerByNameAsync(name);
        }

        //Delete a customer by name
        public async Task<Customer?> DeleteCustomerByNameAsync(string name)
        {
            return await _customerRepository.DeleteCustomerByNameAsync(name);
        }

        //Get or create a customer by name
        public async Task<Customer> GetOrCreateCustomerAsync(string customerName)
        {
            //Step 1: Attempt to retrieve the customer by name
            var customer = await _customerRepository.GetCustomerByNameAsync(customerName);

            //Step 2: If the customer does not exist, create a new one
            if (customer == null)
            {
                customer = new Customer
                {
                    Name = customerName, //Set the customer's name
                    PhoneNumber = "N/A", //Default phone number value
                    EmailAddress = "N/A" //Default email address value
                };

                //Create the new customer using the repository method
                var createdCustomer = await _customerRepository.CreateCustomerAsync(customer);

                //Throw an exception if customer creation fails
                if (createdCustomer != null)
                {
                    return createdCustomer;
                }
                else
                {
                    throw new InvalidOperationException($"Failed to create customer '{customerName}'");
                }
            }
            //Return the existing or newly created customer
            return customer;
        }
    }
}