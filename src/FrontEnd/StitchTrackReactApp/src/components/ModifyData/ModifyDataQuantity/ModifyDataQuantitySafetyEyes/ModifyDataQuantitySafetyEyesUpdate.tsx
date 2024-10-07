import React, { useState } from "react";
import axios from "axios";
import { useNavigate, useLocation } from "react-router-dom";

const ModifyDataQuantitySafetyEyesUpdate: React.FC = () => {
  const [quantity, setQuantity] = useState<number | "">("");
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();
  const location = useLocation();

  //Extract safety eye details from location state
  const { sizeInMM, color, shape } = location.state || {};

  const handleUpdate = async () => {
    try {
      if (quantity === "" || quantity < 0) {
        setError("Please enter a valid quantity.");
        return;
      }

      //Update the safety eye quantity in the backend
      await axios.put(
        `${process.env.REACT_APP_API_URL}/SafetyEye/update-quantity`,
        {
          SizeInMM: sizeInMM, //Use `SizeInMM` to match the backend property name
          color,
          shape,
          NumberOfEyesOwned: quantity,
        }
      );

      //Display alert to confirm successful update
      alert("Safety eye quantity updated successfully!");
      setError(null);

      //Navigate back to the home screen after user acknowledges the alert
      navigate("/");
    } catch (err) {
      console.error("Error updating safety eye quantity", err);
      setError("Failed to update safety eye quantity. Please try again.");
    }
  };

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">Update Safety Eyes Quantity</h1>
        <hr className="title-separator" />
      </header>

      <div className="form-group">
        <label htmlFor="size">Size (mm):</label>{" "}
        {/* Updated label association */}
        <input
          id="size" //Added id for the input field
          type="text"
          value={sizeInMM}
          readOnly
        />
        <label htmlFor="color">Color:</label> {/* Updated label association */}
        <input
          id="color" //Added id for the input field
          type="text"
          value={color}
          readOnly
        />
        <label htmlFor="shape">Shape:</label> {/* Updated label association */}
        <input
          id="shape" //Added id for the input field
          type="text"
          value={shape}
          readOnly
        />
        <label htmlFor="quantity">New Quantity:</label>{" "}
        {/* Updated label association */}
        <input
          id="quantity" //Added id for the input field
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

export default ModifyDataQuantitySafetyEyesUpdate;
