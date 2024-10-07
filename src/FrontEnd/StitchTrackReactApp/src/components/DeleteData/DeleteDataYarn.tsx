import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const DeleteDataYarn: React.FC = () => {
  const [brand, setBrand] = useState("");
  const [fiberType, setFiberType] = useState("");
  const [fiberWeight, setFiberWeight] = useState("");
  const [color, setColor] = useState("");
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleDelete = async () => {
    try {
      //Send a DELETE request to the backend API to delete the yarn based on provided details
      await axios.delete(`${process.env.REACT_APP_API_URL}/Yarn/delete`, {
        data: {
          brand,
          fiberType,
          fiberWeight: parseInt(fiberWeight),
          color,
        },
      });

      //Display a success message
      alert("Yarn deleted successfully!");

      //Navigate back to the start or previous page
      navigate("/");
    } catch (error) {
      console.error("Error deleting yarn", error);
      setError("Failed to delete yarn. Please try again.");
    }
  };

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">Enter Yarn Details to Delete</h1>
        <hr className="title-separator" />
      </header>

      {/* Form Fields */}
      <div className="form-group">
        <label htmlFor="brand-input">Brand:</label>{" "}
        {/* Added htmlFor attribute */}
        <input
          id="brand-input" //Added id attribute
          type="text"
          value={brand}
          onChange={(e) => setBrand(e.target.value)}
        />
        <label htmlFor="fiberType-input">Fiber Type:</label>{" "}
        {/* Added htmlFor attribute */}
        <input
          id="fiberType-input" //Added id attribute
          type="text"
          value={fiberType}
          onChange={(e) => setFiberType(e.target.value)}
        />
        <label htmlFor="fiberWeight-input">Fiber Weight:</label>{" "}
        {/* Added htmlFor attribute */}
        <input
          id="fiberWeight-input" //Added id attribute
          type="text"
          value={fiberWeight}
          onChange={(e) => setFiberWeight(e.target.value)}
        />
        <label htmlFor="color-input">Color:</label>{" "}
        {/* Added htmlFor attribute */}
        <input
          id="color-input" //Added id attribute
          type="text"
          value={color}
          onChange={(e) => setColor(e.target.value)}
        />
      </div>

      {/* Error Message */}
      {error && <div className="error-message">{error}</div>}

      {/* Buttons */}
      <div className="button-group">
        <button className="button" onClick={handleDelete}>
          Delete Yarn
        </button>
        <button className="button" onClick={() => navigate("/")}>
          Back to Start
        </button>
      </div>
    </div>
  );
};

export default DeleteDataYarn;
