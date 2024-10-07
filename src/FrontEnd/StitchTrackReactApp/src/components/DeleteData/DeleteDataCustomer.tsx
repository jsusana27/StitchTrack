import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const DeleteDataCustomer: React.FC = () => {
  const [customerName, setCustomerName] = useState("");
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleSubmit = async () => {
    try {
      //Make DELETE request to the backend API
      const response = await axios.delete(
        `${process.env.REACT_APP_API_URL}/Customer/delete`,
        {
          params: {
            name: customerName,
          },
        }
      );

      if (response.status === 200) {
        alert("Customer deleted successfully!");
        navigate("/"); //Navigate back to home page
      } else {
        throw new Error("Failed to delete customer.");
      }
    } catch (error) {
      console.error("Error deleting customer", error);
      setError("Failed to delete customer. Please try again.");
    }
  };

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">Enter Customer Name to Delete</h1>
        <hr className="title-separator" />
      </header>

      {/* Form Fields */}
      <div className="form-group">
        <label htmlFor="customerName">Customer Name:</label>{" "}
        {/* Added htmlFor attribute */}
        <input
          id="customerName" //Matching id attribute for the input field
          type="text"
          value={customerName}
          onChange={(e) => setCustomerName(e.target.value)}
        />
      </div>

      {/* Error Message */}
      {error && <div className="error-message">{error}</div>}

      {/* Buttons */}
      <div className="button-group">
        <button className="button" onClick={handleSubmit}>
          Delete Customer
        </button>
        <button className="button" onClick={() => navigate("/delete-data")}>
          Back
        </button>
      </div>
    </div>
  );
};

export default DeleteDataCustomer;
