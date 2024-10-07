import React, { useState } from "react";
import axios from "axios";
import { useNavigate, useLocation } from "react-router-dom";

const ModifyDataQuantityYarnUpdate: React.FC = () => {
  const [quantity, setQuantity] = useState<number | "">("");
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();
  const location = useLocation();

  //Extract yarn details from location state
  const { brand, fiberType, fiberWeight, color } = location.state || {};

  const handleUpdate = async () => {
    try {
      if (quantity === "" || quantity < 0) {
        setError("Please enter a valid quantity.");
        return;
      }

      //Update the yarn quantity in the backend
      await axios.put(`${process.env.REACT_APP_API_URL}/Yarn/update-quantity`, {
        brand,
        fiberType,
        fiberWeight,
        color,
        NumberOfSkeinsOwned: quantity,
      });

      //Display alert to confirm successful update
      alert("Yarn quantity updated successfully!");
      setError(null);

      //Navigate back to the home screen after user acknowledges the alert
      navigate("/");
    } catch (err) {
      console.error("Error updating yarn quantity", err);
      setError("Failed to update yarn quantity. Please try again.");
    }
  };

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">Update Yarn Quantity</h1>
        <hr className="title-separator" />
      </header>

      <div className="form-group">
        <label htmlFor="brand">Brand:</label> {/* Updated label association */}
        <input
          id="brand" //Added id for the input field
          type="text"
          value={brand}
          readOnly
        />
        <label htmlFor="fiberType">Fiber Type:</label>{" "}
        {/* Updated label association */}
        <input
          id="fiberType" //Added id for the input field
          type="text"
          value={fiberType}
          readOnly
        />
        <label htmlFor="fiberWeight">Fiber Weight:</label>{" "}
        {/* Updated label association */}
        <input
          id="fiberWeight" //Added id for the input field
          type="text"
          value={fiberWeight}
          readOnly
        />
        <label htmlFor="color">Color:</label> {/* Updated label association */}
        <input
          id="color" //Added id for the input field
          type="text"
          value={color}
          readOnly
        />
        <label htmlFor="newQuantity">New Quantity:</label>{" "}
        {/* Updated label association */}
        <input
          id="newQuantity" //Added id for the input field
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

export default ModifyDataQuantityYarnUpdate;
