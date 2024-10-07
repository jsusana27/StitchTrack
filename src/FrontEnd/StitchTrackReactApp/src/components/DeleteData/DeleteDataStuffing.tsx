import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const DeleteDataStuffing: React.FC = () => {
  const [brand, setBrand] = useState("");
  const [type, setType] = useState("");
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleSubmit = async () => {
    try {
      //Make DELETE request to the backend API
      const response = await axios.delete(
        `${process.env.REACT_APP_API_URL}/Stuffing/Delete`,
        {
          params: {
            brand,
            type,
          },
        }
      );

      if (response.status === 200) {
        alert("Stuffing deleted successfully!");
        navigate("/"); //Navigate back to home page
      } else {
        throw new Error("Failed to delete stuffing.");
      }
    } catch (error) {
      console.error("Error deleting stuffing", error);
      setError("Failed to delete stuffing. Please try again.");
    }
  };

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">Enter Stuffing Details to Delete</h1>
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
        <label htmlFor="type-input">Type:</label>{" "}
        {/* Added htmlFor attribute */}
        <input
          id="type-input" //Added id attribute
          type="text"
          value={type}
          onChange={(e) => setType(e.target.value)}
        />
      </div>

      {/* Error Message */}
      {error && <div className="error-message">{error}</div>}

      {/* Buttons */}
      <div className="button-group">
        <button className="button" onClick={handleSubmit}>
          Delete Stuffing
        </button>
        <button className="button" onClick={() => navigate("/delete-data")}>
          Back
        </button>
      </div>
    </div>
  );
};

export default DeleteDataStuffing;
