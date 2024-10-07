using CrochetBusinessAPI.Models;        //Import models used in the service layer
using CrochetBusinessAPI.Repositories;  //Import repositories for data operations

namespace CrochetBusinessAPI.Services
{
    //Service class for managing operations related to Order
    public class OrderService : Service<Order>
    {
        //Repository specific to Order operations
        private readonly OrderRepository _orderRepository;

        //Repositories for handling related entities
        private readonly OrderProductRepository _orderProductRepository;
        private readonly CustomerPurchaseRepository _customerPurchaseRepository;

        //Services for managing dependencies and related operations
        private readonly CustomerService _customerService;
        private readonly FinishedProductService _finishedProductService;

        //Constructor to initialize OrderService with required repositories and services
        public OrderService(OrderRepository orderRepository,
                            OrderProductRepository orderProductRepository,
                            CustomerPurchaseRepository customerPurchaseRepository,
                            CustomerService customerService,
                            FinishedProductService finishedProductService)
            : base(orderRepository)  //Pass the OrderRepository to the base constructor
        {
            _orderRepository = orderRepository;
            _orderProductRepository = orderProductRepository;
            _customerPurchaseRepository = customerPurchaseRepository;
            _customerService = customerService;
            _finishedProductService = finishedProductService;
        }

        //Get all orders with related customer and order products
        public override async Task<List<Order>> GetAllAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        //Delete an order by customer name and order date
        public async Task<bool> DeleteOrderByCustomerAndDateAsync(string customerName, DateTime orderDate)
        {
            //Step 1: Retrieve the order by customer name and order date
            var order = await _orderRepository.GetOrderByCustomerAndDateAsync(customerName, orderDate);
            if (order == null) return false;

            //Step 2: Retrieve all products in the OrderProduct table associated with this order
            var orderProducts = await _orderProductRepository.GetOrderProductsByOrderIdAsync(order.OrderID);

            //Step 3: Delete all associated OrderProduct entries
            await _orderRepository.DeleteOrderProductsByOrderIdAsync(order.OrderID);

            //Step 4: Update CustomerPurchases table
            foreach (var orderProduct in orderProducts)
            {
                //Use the GetCustomerPurchaseAsync method to find the corresponding CustomerPurchase entry
                var customerPurchase = await _customerPurchaseRepository.GetCustomerPurchaseAsync(order.CustomerID, orderProduct.FinishedProductsID);

                //If the customer purchase entry exists, delete it
                if (customerPurchase != null)
                {
                    await _customerPurchaseRepository.DeleteCustomerPurchaseAsync(customerPurchase);
                }
            }

            //Step 5: Delete the order itself
            return await _orderRepository.DeleteOrderAsync(order.OrderID);
        }

        //Create a new order with customer, order date, and product details
        public async Task<bool> CreateOrderAsync(string customerName, string orderDate, string formOfPayment, List<string> productNames, List<int> quantities)
        {
            //Step 1: Retrieve or create the customer
            var customer = await _customerService.GetOrCreateCustomerAsync(customerName);
            if (customer == null) return false;

            //Step 2: Parse the order date
            if (!DateTime.TryParseExact(orderDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime parsedOrderDate))
            {
                return false;
            }

            //Step 3: Create the order with initial details
            var newOrder = new Order
            {
                CustomerID = customer.CustomerID.GetValueOrDefault(), //Set CustomerID from the created customer
                OrderDate = parsedOrderDate, //Set the parsed order date
                FormOfPayment = formOfPayment, //Set the form of payment
                TotalPrice = 0 //Initialize to 0, will update it after calculating total price
            };

            //Create the new order in the repository
            var createdOrder = await _orderRepository.CreateOrderAsync(newOrder);
            if (createdOrder == null) return false;

            //Step 4: Loop through each product and add to OrderProduct table
            decimal totalPrice = 0; //Variable to hold the total price
            for (int i = 0; i < productNames.Count; i++)
            {
                var productName = productNames[i];
                var quantity = quantities[i];

                //Get the finished product by name
                var finishedProduct = await _finishedProductService.GetProductByNameAsync(productName);
                if (finishedProduct == null) continue; //Skip if the product is not found

                //Create an OrderProduct record
                var orderProduct = new OrderProduct
                {
                    OrderID = createdOrder.OrderID, //Use the auto-generated ID
                    FinishedProductsID = finishedProduct.FinishedProductsID.GetValueOrDefault(),
                    Quantity = quantity //Set the quantity for the order
                };

                //Add the OrderProduct entry to the repository
                await _orderProductRepository.AddOrderProductAsync(orderProduct);

                //Calculate the cost of this product based on its sale price and quantity
                totalPrice += finishedProduct.SalePrice.GetValueOrDefault() * quantity;

                //Step 5: Insert into CustomerPurchases table
                var customerPurchase = new CustomerPurchase
                {
                    CustomerID = customer.CustomerID.GetValueOrDefault(), //Set the CustomerID
                    FinishedProductsID = finishedProduct.FinishedProductsID.GetValueOrDefault() //Set the FinishedProductsID
                };

                //Add the CustomerPurchase entry
                await _customerPurchaseRepository.CreateAsync(customerPurchase);
            }

            //Step 6: Update the total price in the order entity
            createdOrder.TotalPrice = totalPrice;

            //Step 7: Save the changes to the order with the updated total price using the repository's update method
            await _orderRepository.UpdateAsync(createdOrder);
            await _orderRepository.SaveAsync();

            return true;
        }
    }
}