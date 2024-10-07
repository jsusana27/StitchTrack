import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const AddDataProduct: React.FC = () => {
  const [name, setName] = useState("");
  const [timeToMake, setTimeToMake] = useState("");
  const [totalCostToMake, setTotalCostToMake] = useState("");
  const [salePrice, setSalePrice] = useState("");
  const [numberInStock, setNumberInStock] = useState("");
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleSubmit = async () => {
    try {
      //Remove the FinishedProductsID from the request payload
      const newProduct = {
        name,
        timeToMake, //Assuming this is in string format (like "01:30:00" for 1 hour and 30 minutes)
        totalCostToMake: parseFloat(totalCostToMake), //Ensure it's a float
        salePrice: parseFloat(salePrice), //Ensure it's a float
        numberInStock: parseInt(numberInStock), //Ensure it's an integer
      };

      //Send a POST request to the backend API to add the new product
      await axios.post(
        `${process.env.REACT_APP_API_URL}/FinishedProduct`,
        newProduct
      );

      //Display a success message
      alert("Product added successfully!");

      //After successful submission, navigate back to the main data page
      navigate("/add-data");
    } catch (error) {
      console.error("Error adding product", error);
      setError("Failed to add product. Please try again.");
    }
  };

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">Enter New Finished Product Details</h1>
        <hr className="title-separator" />
      </header>

      {/* Form Fields */}
      <div className="form-group">
        <label htmlFor="productName">Product Name:</label>
        <input
          id="productName"
          type="text"
          value={name}
          onChange={(e) => setName(e.target.value)}
        />

        <label htmlFor="timeToMake">Time to Make hh:mm:ss:</label>
        <input
          id="timeToMake"
          type="text"
          value={timeToMake}
          onChange={(e) => setTimeToMake(e.target.value)}
        />

        <label htmlFor="totalCostToMake">Total Cost to Make:</label>
        <input
          id="totalCostToMake"
          type="text"
          value={totalCostToMake}
          onChange={(e) => setTotalCostToMake(e.target.value)}
        />

        <label htmlFor="salePrice">Sale Price:</label>
        <input
          id="salePrice"
          type="text"
          value={salePrice}
          onChange={(e) => setSalePrice(e.target.value)}
        />

        <label htmlFor="numberInStock">Number in Stock:</label>
        <input
          id="numberInStock"
          type="text"
          value={numberInStock}
          onChange={(e) => setNumberInStock(e.target.value)}
        />
      </div>

      {/* Error Message */}
      {error && <div className="error-message">{error}</div>}

      {/* Buttons */}
      <div className="button-group">
        <button className="button" onClick={handleSubmit}>
          Add Product
        </button>
        <button className="button" onClick={() => navigate("/add-data")}>
          Back
        </button>
      </div>
    </div>
  );
};

export default AddDataProduct;
