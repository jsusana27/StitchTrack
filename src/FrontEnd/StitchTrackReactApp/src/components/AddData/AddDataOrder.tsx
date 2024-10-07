import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import { v4 as uuidv4 } from "uuid"; //Import the uuid library

const AddDataOrder: React.FC = () => {
  const [customerName, setCustomerName] = useState<string>("");
  const [orderDate, setOrderDate] = useState<string>("");
  const [formOfPayment, setFormOfPayment] = useState<string>("");
  const [products, setProducts] = useState<
    { id: string; productName: string; quantity: number }[] //Include 'id' property for unique keys
  >([]);
  const [currentProductName, setCurrentProductName] = useState<string>("");
  const [currentQuantity, setCurrentQuantity] = useState<string>(""); //Change to string for controlled input
  const navigate = useNavigate();

  //Add a product to the order
  const addProduct = () => {
    const parsedQuantity = parseInt(currentQuantity, 10);
    if (currentProductName && parsedQuantity > 0) {
      setProducts([
        ...products,
        {
          id: uuidv4(),
          productName: currentProductName,
          quantity: parsedQuantity,
        }, //Add unique id to each product
      ]);
      setCurrentProductName("");
      setCurrentQuantity("");
    } else {
      alert("Please provide valid product name and quantity.");
    }
  };

  //Handle the form submission to create a new order
  const handleOrderSubmit = async () => {
    if (
      !customerName ||
      !orderDate ||
      !formOfPayment ||
      products.length === 0
    ) {
      alert("Please fill in all fields and add at least one product.");
      return;
    }

    //Prepare the query parameters for the GET request
    const productNames = products
      .map((product) => product.productName)
      .join(",");
    const quantities = products.map((product) => product.quantity).join(",");

    try {
      //Log the URL to see the full request being made
      const apiUrl = `${process.env.REACT_APP_API_URL}/Order/create`;
      const queryParams = new URLSearchParams({
        customerName: customerName.trim(),
        orderDate: orderDate.trim(),
        formOfPayment: formOfPayment.trim(),
        productNames,
        quantities,
      }).toString();

      console.log(`API Request URL: ${apiUrl}?${queryParams}`);

      //Make the API call using query parameters
      const response = await axios.post(`${apiUrl}?${queryParams}`);

      if (response.status === 200) {
        alert("Order created successfully!");
        setProducts([]); //Clear the products in the order
        setCustomerName("");
        setOrderDate("");
        setFormOfPayment("");
        navigate("/");
      } else {
        alert("Failed to create order. Please try again.");
      }
    } catch (error) {
      console.error("Error creating order:", error);
      alert("An error occurred while creating the order.");
    }
  };

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">Enter Order Details</h1>
        <hr className="title-separator" />
      </header>

      {/* Order Details Form */}
      <div className="form-group">
        <input
          type="text"
          value={customerName}
          onChange={(e) => setCustomerName(e.target.value)}
          placeholder="First and Last Name"
          className="input"
        />
        <input
          type="text"
          value={orderDate}
          onChange={(e) => setOrderDate(e.target.value)}
          placeholder="Order Date (YYYY-MM-DD format)"
          className="input"
        />
        <input
          type="text"
          value={formOfPayment}
          onChange={(e) => setFormOfPayment(e.target.value)}
          placeholder="Form of Payment"
          className="input"
        />
      </div>

      {/* Add Product Section */}
      <header className="header">
        <h1 className="title">Enter the Product Name and Quantity Purchased</h1>
        <hr className="title-separator" />
      </header>

      <div className="form-group">
        <input
          type="text"
          value={currentProductName}
          onChange={(e) => setCurrentProductName(e.target.value)}
          placeholder="Product Name"
          className="input"
        />
        <input
          type="text" //Change to text input for controlled input handling
          value={currentQuantity}
          onChange={(e) => {
            const value = e.target.value;
            //Check if the value is a number or empty string
            if (/^\d*$/.test(value)) {
              setCurrentQuantity(value); //Only update state with valid number strings
            }
          }}
          placeholder="Quantity"
          className="input"
        />
      </div>

      <div className="button-group">
        <button onClick={addProduct} className="button">
          Add Product
        </button>
      </div>

      {/* Display Added Products */}
      <div className="product-list">
        <h2>Products in Order</h2>
        <ul>
          {products.map((product) => (
            <li key={product.id}>
              {" "}
              {/* Use unique id for keys */}
              {product.productName} - {product.quantity}
            </li>
          ))}
        </ul>
      </div>

      {/* Submit Order Button */}
      <div className="button-group">
        <button onClick={handleOrderSubmit} className="button">
          Finish Order
        </button>
        <button
          onClick={() => navigate("/add-data")}
          className="button back-button"
        >
          Back
        </button>
      </div>
    </div>
  );
};

export default AddDataOrder;
