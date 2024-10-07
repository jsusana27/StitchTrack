import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const AddDataYarn: React.FC = () => {
  const [brand, setBrand] = useState("");
  const [price, setPrice] = useState("");
  const [fiberType, setFiberType] = useState("");
  const [fiberWeight, setFiberWeight] = useState("");
  const [color, setColor] = useState("");
  const [yardage, setYardage] = useState("");
  const [grams, setGrams] = useState("");
  const [numberOfSkeinsOwned, setNumberOfSkeinsOwned] = useState("");
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleSubmit = async () => {
    //Ensure all fields are filled before submitting
    if (
      !brand.trim() ||
      !price.trim() ||
      !fiberType.trim() ||
      !fiberWeight.trim() ||
      !color.trim() ||
      !yardage.trim() ||
      !grams.trim() ||
      !numberOfSkeinsOwned.trim()
    ) {
      setError("Please fill in all fields.");
      return;
    }

    try {
      //Parse inputs and validate numeric fields
      const parsedPrice = parseFloat(price);
      const parsedFiberWeight = parseInt(fiberWeight, 10);
      const parsedYardage = parseInt(yardage, 10);
      const parsedGrams = parseInt(grams, 10);
      const parsedNumberOfSkeins = parseInt(numberOfSkeinsOwned, 10);

      if (
        isNaN(parsedPrice) ||
        isNaN(parsedFiberWeight) ||
        isNaN(parsedYardage) ||
        isNaN(parsedGrams) ||
        isNaN(parsedNumberOfSkeins)
      ) {
        setError("Please provide valid numeric values where applicable.");
        return;
      }

      const newYarn = {
        brand: brand.trim(),
        price: parsedPrice,
        fiberType: fiberType.trim(),
        fiberWeight: parsedFiberWeight,
        color: color.trim(),
        yardagePerSkein: parsedYardage,
        gramsPerSkein: parsedGrams,
        numberOfSkeinsOwned: parsedNumberOfSkeins,
      };

      //Log data to verify correctness before sending
      console.log("Data being sent:", newYarn);

      //Send a POST request to the backend API to add the new yarn
      await axios.post(`${process.env.REACT_APP_API_URL}/Yarn`, newYarn);

      //Display a success message
      alert("Yarn added successfully!");
      navigate("/add-data");
    } catch (error) {
      console.error("Error adding yarn", error);
      setError("Failed to add yarn. Please try again.");
    }
  };

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">Enter New Yarn Details</h1>
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

        <label htmlFor="price">Price:</label>
        <input
          id="price"
          type="text"
          value={price}
          onChange={(e) => setPrice(e.target.value)}
        />

        <label htmlFor="fiberType">Fiber Type:</label>
        <input
          id="fiberType"
          type="text"
          value={fiberType}
          onChange={(e) => setFiberType(e.target.value)}
        />

        <label htmlFor="fiberWeight">Fiber Weight:</label>
        <input
          id="fiberWeight"
          type="text"
          value={fiberWeight}
          onChange={(e) => setFiberWeight(e.target.value)}
        />

        <label htmlFor="color">Color:</label>
        <input
          id="color"
          type="text"
          value={color}
          onChange={(e) => setColor(e.target.value)}
        />

        <label htmlFor="yardage">Yardage per Skein:</label>
        <input
          id="yardage"
          type="text"
          value={yardage}
          onChange={(e) => setYardage(e.target.value)}
        />

        <label htmlFor="grams">Grams per Skein:</label>
        <input
          id="grams"
          type="text"
          value={grams}
          onChange={(e) => setGrams(e.target.value)}
        />

        <label htmlFor="numberOfSkeinsOwned">Skeins in Stock:</label>
        <input
          id="numberOfSkeinsOwned"
          type="text"
          value={numberOfSkeinsOwned}
          onChange={(e) => setNumberOfSkeinsOwned(e.target.value)}
        />
      </div>

      {/* Error Message */}
      {error && <div className="error-message">{error}</div>}

      {/* Buttons */}
      <div className="button-group">
        <button className="button" onClick={handleSubmit}>
          Add Yarn
        </button>
        <button className="button" onClick={() => navigate("/add-data")}>
          Back
        </button>
      </div>
    </div>
  );
};

export default AddDataYarn;
