import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const AddDataSafetyEyes: React.FC = () => {
  const [size, setSize] = useState("");
  const [color, setColor] = useState("");
  const [shape, setShape] = useState("");
  const [price, setPrice] = useState("");
  const [inStock, setInStock] = useState("");
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleSubmit = async () => {
    try {
      const newSafetyEyes = {
        sizeInMM: parseFloat(size), //Ensure it's a float for mm
        color,
        shape,
        price: parseFloat(price), //Ensure it's a float
        numberOfEyesOwned: parseInt(inStock), //Ensure it's an integer
      };

      //Send a POST request to the backend API to add the new safety eyes
      await axios.post(
        `${process.env.REACT_APP_API_URL}/SafetyEye`,
        newSafetyEyes
      );

      //Display a success message
      alert("Safety Eyes added successfully!");

      //After successful submission, navigate back to the add data page
      navigate("/add-data");
    } catch (error) {
      console.error("Error adding safety eyes", error);
      setError("Failed to add safety eyes. Please try again.");
    }
  };

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">Enter New Safety Eyes Details</h1>
        <hr className="title-separator" />
      </header>

      {/* Form Fields */}
      <div className="form-group">
        <label htmlFor="size">Size (mm):</label>
        <input
          id="size"
          type="text"
          value={size}
          onChange={(e) => setSize(e.target.value)}
        />

        <label htmlFor="color">Color:</label>
        <input
          id="color"
          type="text"
          value={color}
          onChange={(e) => setColor(e.target.value)}
        />

        <label htmlFor="shape">Shape:</label>
        <input
          id="shape"
          type="text"
          value={shape}
          onChange={(e) => setShape(e.target.value)}
        />

        <label htmlFor="price">Price:</label>
        <input
          id="price"
          type="text"
          value={price}
          onChange={(e) => setPrice(e.target.value)}
        />

        <label htmlFor="inStock">In Stock:</label>
        <input
          id="inStock"
          type="text"
          value={inStock}
          onChange={(e) => setInStock(e.target.value)}
        />
      </div>

      {/* Error Message */}
      {error && <div className="error-message">{error}</div>}

      {/* Buttons */}
      <div className="button-group">
        <button className="button" onClick={handleSubmit}>
          Add Safety Eyes
        </button>
        <button className="button" onClick={() => navigate("/add-data")}>
          Back
        </button>
      </div>
    </div>
  );
};

export default AddDataSafetyEyes;
