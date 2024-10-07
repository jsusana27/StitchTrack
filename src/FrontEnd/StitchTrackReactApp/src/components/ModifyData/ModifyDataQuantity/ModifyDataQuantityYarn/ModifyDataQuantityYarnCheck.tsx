import React, { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

const ModifyDataQuantityYarnCheck: React.FC = () => {
  const [brand, setBrand] = useState("");
  const [fiberType, setFiberType] = useState("");
  const [fiberWeight, setFiberWeight] = useState("");
  const [color, setColor] = useState("");
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleNext = async () => {
    try {
      //Make sure all fields are filled
      if (!brand || !fiberType || !fiberWeight || !color) {
        setError("All fields are required.");
        return;
      }

      //Check if the yarn exists in the backend
      const response = await axios.get(
        `${process.env.REACT_APP_API_URL}/Yarn/check-existence`,
        { params: { brand, fiberType, fiberWeight, color } }
      );

      if (response.data.exists) {
        //Alert the user that a matching yarn was found
        alert("Matching yarn found!");

        //Navigate to the update page if the yarn exists
        navigate("/modify-data/quantity-update/yarn-check/yarn-update", {
          state: { brand, fiberType, fiberWeight, color },
        });
      } else {
        setError("The specified yarn does not exist in the database.");
      }
    } catch (err) {
      console.error("Error checking yarn existence", err);
      setError("Failed to check yarn existence. Please try again.");
    }
  };

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">Modify Yarn Quantity</h1>
        <hr className="title-separator" />
      </header>

      <div className="form-group">
        <label htmlFor="brand">Brand:</label> {/* Updated label association */}
        <input
          id="brand" //Added id for the input field
          type="text"
          value={brand}
          onChange={(e) => setBrand(e.target.value)}
        />
        <label htmlFor="fiberType">Fiber Type:</label>{" "}
        {/* Updated label association */}
        <input
          id="fiberType" //Added id for the input field
          type="text"
          value={fiberType}
          onChange={(e) => setFiberType(e.target.value)}
        />
        <label htmlFor="fiberWeight">Fiber Weight:</label>{" "}
        {/* Updated label association */}
        <input
          id="fiberWeight" //Added id for the input field
          type="text"
          value={fiberWeight}
          onChange={(e) => setFiberWeight(e.target.value)}
        />
        <label htmlFor="color">Color:</label> {/* Updated label association */}
        <input
          id="color" //Added id for the input field
          type="text"
          value={color}
          onChange={(e) => setColor(e.target.value)}
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

export default ModifyDataQuantityYarnCheck;
