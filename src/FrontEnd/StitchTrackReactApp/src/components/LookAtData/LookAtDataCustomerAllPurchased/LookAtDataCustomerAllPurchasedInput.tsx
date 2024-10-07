import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const LookAtDataCustomerAllPurchasedInput: React.FC = () => {
  const [customerName, setCustomerName] = useState("");
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleSubmit = async () => {
    try {
      //First, search for the customer by name
      const response = await axios.get(
        `${process.env.REACT_APP_API_URL}/customer/search-by-name?name=${customerName}`
      );

      //Use optional chaining for concise checking
      if (response.data?.customerID) {
        //If the customer is found, navigate to the purchases page using the customer ID
        navigate(
          `/look-at-data/customer-purchased/${response.data.customerID}`
        );
      } else {
        setError("Customer not found");
      }
    } catch (error) {
      console.error("Error fetching customer data", error);
      setError("Error fetching customer data");
    }
  };

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">What is the name of the customer?</h1>
        <hr className="title-separator" />
      </header>

      <div className="form-group">
        <input
          type="text"
          value={customerName}
          onChange={(e) => setCustomerName(e.target.value)}
          placeholder="Enter customer name"
        />
      </div>

      {error && <div className="error">{error}</div>}

      <div className="button-group">
        <button className="button" onClick={handleSubmit}>
          Next
        </button>
        <button className="button" onClick={() => navigate("/look-at-data")}>
          Back
        </button>
      </div>
    </div>
  );
};

export default LookAtDataCustomerAllPurchasedInput;
