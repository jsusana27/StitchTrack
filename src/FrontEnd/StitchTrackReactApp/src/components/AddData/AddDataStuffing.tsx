import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const AddDataStuffing: React.FC = () => {
  const [brand, setBrand] = useState("");
  const [type, setType] = useState("");
  const [pricePerFivePounds, setPricePerFivePounds] = useState("");
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleSubmit = async () => {
    //Ensure all fields are filled before submitting
    if (!brand.trim() || !type.trim() || !pricePerFivePounds.trim()) {
      setError("Please fill in all fields.");
      return;
    }

    try {
      //Parse the price input to a float and handle empty string cases
      const parsedPricePerFivePounds = parseFloat(pricePerFivePounds);
      if (isNaN(parsedPricePerFivePounds)) {
        setError("Please provide a valid price.");
        return;
      }

      const newStuffing = {
        brand: brand.trim(),
        type: type.trim(),
        pricePerFivelbs: parsedPricePerFivePounds,
      };

      //Log the data to check what's being sent
      console.log("Data being sent:", newStuffing);

      //Send a POST request to the backend API to add the new stuffing
      await axios.post(
        `${process.env.REACT_APP_API_URL}/Stuffing`,
        newStuffing
      );

      //Display a success message
      alert("Stuffing added successfully!");
      navigate("/add-data");
    } catch (error) {
      console.error("Error adding stuffing", error);
      setError("Failed to add stuffing. Please try again.");
    }
  };

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">Enter New Stuffing Details</h1>
        <hr className="title-separator" />
      </header>

      {/* Form Fields */}
      <div className="form-group">
        <label htmlFor="brand">Brand:</label>
        <input
          id="brand"
          type="text"
          value={brand}
          onChange={(e) => setBrand(e.target.value)}
        />

        <label htmlFor="type">Type:</label>
        <input
          id="type"
          type="text"
          value={type}
          onChange={(e) => setType(e.target.value)}
        />

        <label htmlFor="pricePerFivePounds">Price per 5 lbs:</label>
        <input
          id="pricePerFivePounds"
          type="text"
          value={pricePerFivePounds}
          onChange={(e) => setPricePerFivePounds(e.target.value)}
        />
      </div>

      {/* Error Message */}
      {error && <div className="error-message">{error}</div>}

      {/* Buttons */}
      <div className="button-group">
        <button className="button" onClick={handleSubmit}>
          Add Stuffing
        </button>
        <button className="button" onClick={() => navigate("/add-data")}>
          Back
        </button>
      </div>
    </div>
  );
};

export default AddDataStuffing;
