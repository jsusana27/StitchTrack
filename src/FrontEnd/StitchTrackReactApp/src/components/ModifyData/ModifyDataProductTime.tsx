import React, { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

const ModifyDataProductTime: React.FC = () => {
  const [productName, setProductName] = useState("");
  const [estimatedTime, setEstimatedTime] = useState("");
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleModify = async () => {
    try {
      //Validate inputs
      if (!productName.trim()) {
        setError("Product Name is required.");
        return;
      }
      if (!estimatedTime.trim()) {
        setError("Estimated Time is required.");
        return;
      }

      //Prepare the request payload
      const updatedProduct = {
        name: productName,
        timeToMake: estimatedTime, //Expected format for time can be 'hh:mm:ss'
      };

      //Send a PUT request to the backend API to update the product time
      await axios.put(
        `${process.env.REACT_APP_API_URL}/FinishedProduct/update-product-time`,
        updatedProduct
      );

      setError(null);

      //Show success alert and navigate back to home after the user clicks "OK"
      alert("Product time updated successfully!");
      navigate("/"); //Navigate to the home page after the alert
    } catch (error) {
      console.error("Error updating product time", error);
      setError("Failed to update product time. Please try again.");
    }
  };

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">Modify Product Time</h1>
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
        <label htmlFor="estimatedTime-input">
          Estimated Time to Make (hh:mm:ss):
        </label>{" "}
        {/* Added htmlFor attribute */}
        <input
          id="estimatedTime-input" //Added id attribute
          type="text"
          value={estimatedTime}
          onChange={(e) => setEstimatedTime(e.target.value)}
        />
      </div>

      {/* Error and Success Messages */}
      {error && <div className="error-message">{error}</div>}

      {/* Buttons */}
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

export default ModifyDataProductTime;
