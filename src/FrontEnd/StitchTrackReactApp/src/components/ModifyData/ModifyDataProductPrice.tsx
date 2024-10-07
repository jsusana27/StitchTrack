import React, { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

const ModifyDataProductPrice: React.FC = () => {
  const [productName, setProductName] = useState("");
  const [newPrice, setNewPrice] = useState("");
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleModify = async () => {
    try {
      //Input validation
      if (!productName.trim()) {
        setError("Product Name is required.");
        return;
      }
      if (
        !newPrice.trim() ||
        isNaN(Number(newPrice)) ||
        Number(newPrice) <= 0
      ) {
        setError("Please enter a valid new price greater than 0.");
        return;
      }

      //Prepare the request payload
      const updatedProduct = {
        name: productName,
        salePrice: parseFloat(newPrice),
      };

      //Send a PUT request to the backend API to update the product's sale price
      await axios.put(
        `${process.env.REACT_APP_API_URL}/FinishedProduct/update-sale-price`,
        updatedProduct
      );

      //Display success message and navigate back to the home screen after user clicks "OK"
      alert("Product sale price updated successfully!");
      navigate("/");
    } catch (error) {
      console.error("Error updating product sale price", error);
      setError("Failed to update product sale price. Please try again.");
    }
  };

  return (
    <div className="container">
      {/* Title Section */}
      <header className="header">
        <h1 className="title">Modify Product Price</h1>
        <hr className="title-separator" />
      </header>

      {/* Form Fields */}
      <div className="form-group">
        <label htmlFor="productName-input">Product Name:</label>{" "}
        {/* Added htmlFor attribute */}
        <input
          id="productName-input" //Added id attribute
          type="text"
          value={productName}
          onChange={(e) => setProductName(e.target.value)}
        />
        <label htmlFor="newPrice-input">New Price:</label>{" "}
        {/* Added htmlFor attribute */}
        <input
          id="newPrice-input" //Added id attribute
          type="text"
          value={newPrice}
          onChange={(e) => setNewPrice(e.target.value)}
        />
      </div>

      {/* Error Message */}
      {error && <div className="error-message">{error}</div>}

      {/* Button Group */}
      <div className="button-group">
        <button className="button" onClick={handleModify}>
          Modify
        </button>
        <button className="button" onClick={() => navigate("/modify-data")}>
          Back
        </button>
      </div>
    </div>
  );
};

export default ModifyDataProductPrice;
