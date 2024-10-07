import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const DeleteDataOrder: React.FC = () => {
  const [customerName, setCustomerName] = useState("");
  const [orderDate, setOrderDate] = useState("");
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleSubmit = async () => {
    try {
      //Construct full URL
      const url = `${process.env.REACT_APP_API_URL}/Order/delete`;
      console.log("Delete URL: ", url);

      //Make DELETE request to the backend API
      const response = await axios.delete(url, {
        params: {
          customerName: customerName,
          orderDate: orderDate,
        },
      });

      if (response.status === 200) {
        alert("Order deleted successfully!");
        navigate("/"); //Navigate back to home page
      } else {
        throw new Error("Failed to delete order.");
      }
    } catch (error: any) {
      console.error(
        "Error deleting order",
        error.response ? error.response.data : error.message
      );
      setError("Failed to delete order. Please try again.");
    }
  };

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">
          Enter Customer Name and Order Date to Delete Order
        </h1>
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
        <label htmlFor="orderDate">Order Date (YYYY-MM-DD):</label>{" "}
        {/* Added htmlFor attribute */}
        <input
          id="orderDate" //Matching id attribute for the input field
          type="text"
          value={orderDate}
          onChange={(e) => setOrderDate(e.target.value)}
        />
      </div>

      {/* Error Message */}
      {error && <div className="error-message">{error}</div>}

      {/* Buttons */}
      <div className="button-group">
        <button className="button" onClick={handleSubmit}>
          Delete Order
        </button>
        <button className="button" onClick={() => navigate("/delete-data")}>
          Back
        </button>
      </div>
    </div>
  );
};

export default DeleteDataOrder;
