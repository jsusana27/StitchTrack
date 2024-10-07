import React, { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

const ModifyDataQuantitySafetyEyesCheck: React.FC = () => {
  const [size, setSize] = useState("");
  const [color, setColor] = useState("");
  const [shape, setShape] = useState("");
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleNext = async () => {
    try {
      //Validate inputs
      if (!size || !color || !shape) {
        setError("All fields are required.");
        return;
      }

      //Convert `size` to an integer to match the expected backend type for `SizeInMM`
      const sizeInMM = parseInt(size);

      if (isNaN(sizeInMM)) {
        setError("Please enter a valid size in mm.");
        return;
      }

      //Check if the safety eye exists in the backend using `SizeInMM`
      const response = await axios.get(
        `${process.env.REACT_APP_API_URL}/SafetyEye/check-existence`,
        { params: { sizeInMM, color, shape } } //Use `sizeInMM` to match the backend property name
      );

      if (response.data.exists) {
        //Alert the user that a matching safety eye was found
        alert("Matching safety eye found!");

        //Navigate to the update page if the safety eye exists
        navigate(
          "/modify-data/quantity-update/safety-eyes-check/safety-eyes-update",
          {
            state: { sizeInMM, color, shape }, //Pass `sizeInMM` instead of `size`
          }
        );
      } else {
        setError("The specified safety eye does not exist in the database.");
      }
    } catch (err) {
      console.error("Error checking safety eye existence", err);
      setError("Failed to check safety eye existence. Please try again.");
    }
  };

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">Modify Safety Eyes Quantity</h1>
        <hr className="title-separator" />
      </header>

      <div className="form-group">
        <label htmlFor="size">Size (mm):</label>{" "}
        {/* Updated label association */}
        <input
          id="size" //Added id for the input field
          type="text"
          value={size}
          onChange={(e) => setSize(e.target.value)}
        />
        <label htmlFor="color">Color:</label> {/* Updated label association */}
        <input
          id="color" //Added id for the input field
          type="text"
          value={color}
          onChange={(e) => setColor(e.target.value)}
        />
        <label htmlFor="shape">Shape:</label> {/* Updated label association */}
        <input
          id="shape" //Added id for the input field
          type="text"
          value={shape}
          onChange={(e) => setShape(e.target.value)}
        />
      </div>

      {error && <div className="error-message">{error}</div>}

      <div className="button-group">
        <button className="button" onClick={handleNext}>
          Next
        </button>
        <button className="button" onClick={() => navigate("/modify-data")}>
          Back
        </button>
      </div>
    </div>
  );
};

export default ModifyDataQuantitySafetyEyesCheck;
