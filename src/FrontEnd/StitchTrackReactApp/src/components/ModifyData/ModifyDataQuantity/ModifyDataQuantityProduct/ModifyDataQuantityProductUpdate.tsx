import React, { useState } from "react";
import axios from "axios";
import { useNavigate, useLocation } from "react-router-dom";

const ModifyDataQuantityProductUpdate: React.FC = () => {
  const [quantity, setQuantity] = useState<number | "">("");
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();
  const location = useLocation();

  //Extract finished product name from location state
  const { name } = location.state || {};

  const handleUpdate = async () => {
    try {
      if (quantity === "" || quantity < 0) {
        setError("Please enter a valid quantity.");
        return;
      }

      //Update the finished product quantity in the backend
      await axios.put(
        `${process.env.REACT_APP_API_URL}/FinishedProduct/update-quantity`,
        {
          name,
          NumberInStock: quantity,
        }
      );

      //Display alert to confirm successful update
      alert("Finished product quantity updated successfully!");
      setError(null);

      //Navigate back to the home screen after user acknowledges the alert
      navigate("/");
    } catch (err) {
      console.error("Error updating finished product quantity", err);
      setError("Failed to update finished product quantity. Please try again.");
    }
  };

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">Update Finished Product Quantity</h1>
        <hr className="title-separator" />
      </header>

      <div className="form-group">
        <label htmlFor="product-name">Product Name:</label>{" "}
        {/* Updated label association */}
        <input id="product-name" type="text" value={name} readOnly />
        <label htmlFor="new-quantity">New Quantity:</label>{" "}
        {/* Updated label association */}
        <input
          id="new-quantity"
          type="number"
          value={quantity}
          onChange={(e) => setQuantity(Number(e.target.value))}
        />
      </div>

      {error && <div className="error-message">{error}</div>}

      <div className="button-group">
        <button className="button" onClick={handleUpdate}>
          Update
        </button>
        <button className="button" onClick={() => navigate("/modify-data")}>
          Back
        </button>
      </div>
    </div>
  );
};

export default ModifyDataQuantityProductUpdate;
