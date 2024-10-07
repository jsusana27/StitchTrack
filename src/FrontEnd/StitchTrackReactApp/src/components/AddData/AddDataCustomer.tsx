import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const AddDataCustomer: React.FC = () => {
  const [fullName, setFullName] = useState("");
  const [phoneNumber, setPhoneNumber] = useState("");
  const [email, setEmail] = useState("");
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleSubmit = async () => {
    //Check if the fullName is empty
    if (!fullName.trim()) {
      setError("Full Name is required.");
      return; //Stop form submission
    }

    try {
      const newCustomer = {
        Name: fullName, //This should now match the backend model's property
        PhoneNumber: phoneNumber || null, //Optional field
        EmailAddress: email || null, //Optional field
      };

      //Log the request payload to see what is being sent
      console.log("Request Payload:", newCustomer);

      //Send a POST request to the backend API to add the new customer
      await axios.post(
        `${process.env.REACT_APP_API_URL}/Customer`,
        newCustomer
      );

      //Display a success message
      alert("Customer added successfully!");

      //After successful submission, navigate back to home
      navigate("/");
    } catch (error) {
      console.error("Error adding customer", error);
      setError("Failed to add customer. Please try again.");
    }
  };

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">Enter New Customer Details</h1>
        <hr className="title-separator" />
      </header>

      {/* Form Fields */}
      <div className="form-group">
        <label htmlFor="fullName">First and Last Name:</label>
        <input
          id="fullName"
          type="text"
          value={fullName}
          onChange={(e) => setFullName(e.target.value)}
        />

        <label htmlFor="phoneNumber">Phone Number (optional):</label>
        <input
          id="phoneNumber"
          type="text"
          value={phoneNumber}
          onChange={(e) => setPhoneNumber(e.target.value)}
        />

        <label htmlFor="email">Email Address (optional):</label>
        <input
          id="email"
          type="text"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        />
      </div>

      {/* Error Message */}
      {error && <div className="error-message">{error}</div>}

      {/* Buttons */}
      <div className="button-group">
        <button className="button" onClick={handleSubmit}>
          Add Customer
        </button>
        <button className="button" onClick={() => navigate("/add-data")}>
          Back
        </button>
      </div>
    </div>
  );
};

export default AddDataCustomer;
