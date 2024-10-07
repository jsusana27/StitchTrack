import React, { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

const ModifyDataCustomer: React.FC = () => {
  const [customerName, setCustomerName] = useState("");
  const [newPhoneNumber, setNewPhoneNumber] = useState("");
  const [newEmailAddress, setNewEmailAddress] = useState("");
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleModify = async () => {
    try {
      //Validate inputs
      if (!customerName.trim()) {
        setError("Customer Name is required.");
        return;
      }

      //Prepare the request payload
      const updatedCustomer = {
        name: customerName,
        phoneNumber: newPhoneNumber || null, //Optional field
        emailAddress: newEmailAddress || null, //Optional field
      };

      //Send a PUT request to the backend API to update the customer
      await axios.put(
        `${process.env.REACT_APP_API_URL}/Customer/update-customer-details`,
        updatedCustomer
      );

      setError(null);

      //Show success alert and navigate back to home after the user clicks "OK"
      alert("Customer details updated successfully!");
      navigate("/"); //Navigate to the home page after the alert
    } catch (error) {
      console.error("Error updating customer details", error);
      setError("Failed to update customer details. Please try again.");
    }
  };

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">Modify Customer Details</h1>
        <hr className="title-separator" />
      </header>

      {/* Form Fields */}
      <div className="form-group">
        <label htmlFor="customerName-input">Customer Name:</label>{" "}
        {/* Added htmlFor attribute */}
        <input
          id="customerName-input" //Added id attribute
          type="text"
          value={customerName}
          onChange={(e) => setCustomerName(e.target.value)}
        />
        <label htmlFor="phoneNumber-input">New Phone Number:</label>{" "}
        {/* Added htmlFor attribute */}
        <input
          id="phoneNumber-input" //Added id attribute
          type="text"
          value={newPhoneNumber}
          onChange={(e) => setNewPhoneNumber(e.target.value)}
        />
        <label htmlFor="emailAddress-input">New Email Address:</label>{" "}
        {/* Added htmlFor attribute */}
        <input
          id="emailAddress-input" //Added id attribute
          type="text"
          value={newEmailAddress}
          onChange={(e) => setNewEmailAddress(e.target.value)}
        />
      </div>

      {/* Error Message */}
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

export default ModifyDataCustomer;
