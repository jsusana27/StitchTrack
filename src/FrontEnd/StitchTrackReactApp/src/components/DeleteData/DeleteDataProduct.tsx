import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const DeleteDataProduct: React.FC = () => {
  const [productName, setProductName] = useState("");
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleSubmit = async () => {
    try {
      //Make DELETE request to the backend API
      const response = await axios.delete(
        `${process.env.REACT_APP_API_URL}/FinishedProduct/delete`,
        {
          params: {
            name: productName,
          },
        }
      );

      if (response.status === 200) {
        alert("Product deleted successfully!");
        navigate("/"); //Navigate back to home page
      } else {
        throw new Error("Failed to delete product.");
      }
    } catch (error) {
      console.error("Error deleting product", error);
      setError("Failed to delete product. Please try again.");
    }
  };

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">Enter Product Name to Delete</h1>
        <hr className="title-separator" />
      </header>

      {/* Form Fields */}
      <div className="form-group">
        <label htmlFor="productName">Product Name:</label>{" "}
        {/* Added htmlFor attribute */}
        <input
          id="productName" //Matching id attribute for the input field
          type="text"
          value={productName}
          onChange={(e) => setProductName(e.target.value)}
        />
      </div>

      {/* Error Message */}
      {error && <div className="error-message">{error}</div>}

      {/* Buttons */}
      <div className="button-group">
        <button className="button" onClick={handleSubmit}>
          Delete Product
        </button>
        <button className="button" onClick={() => navigate("/delete-data")}>
          Back
        </button>
      </div>
    </div>
  );
};

export default DeleteDataProduct;
