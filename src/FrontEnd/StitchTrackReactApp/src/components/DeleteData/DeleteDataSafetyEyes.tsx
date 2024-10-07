import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const DeleteDataSafetyEyes: React.FC = () => {
  const [size, setSize] = useState("");
  const [color, setColor] = useState("");
  const [shape, setShape] = useState("");
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  //Function to handle the deletion of safety eyes
  const handleDelete = async () => {
    try {
      const deleteDetails = {
        SizeInMM: parseFloat(size), //Use the same property name as in the model
        Color: color,
        Shape: shape,
      };

      await axios.delete(`${process.env.REACT_APP_API_URL}/SafetyEye/delete`, {
        data: deleteDetails, //Ensure the data is sent in the request body
      });

      alert("Safety Eyes deleted successfully!");
      navigate("/");
    } catch (error) {
      console.error("Error deleting safety eyes", error);
      setError("Failed to delete safety eyes. Please try again.");
    }
  };

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">Enter Safety Eyes Details to Delete</h1>
        <hr className="title-separator" />
      </header>

      {/* Form Fields */}
      <div className="form-group">
        <label htmlFor="size">Size (mm):</label> {/* Updated label */}
        <input
          id="size" //Added id for the input field
          type="text"
          value={size}
          onChange={(e) => setSize(e.target.value)}
        />
      </div>

      <div className="form-group">
        <label htmlFor="color">Color:</label> {/* Updated label */}
        <input
          id="color" //Added id for the input field
          type="text"
          value={color}
          onChange={(e) => setColor(e.target.value)}
        />
      </div>

      <div className="form-group">
        <label htmlFor="shape">Shape:</label> {/* Updated label */}
        <input
          id="shape" //Added id for the input field
          type="text"
          value={shape}
          onChange={(e) => setShape(e.target.value)}
        />
      </div>

      {/* Error Message */}
      {error && <div className="error-message">{error}</div>}

      {/* Buttons */}
      <div className="button-group">
        <button className="button" onClick={handleDelete}>
          Delete Safety Eyes
        </button>
        <button className="button" onClick={() => navigate("/delete-data")}>
          Back
        </button>
      </div>
    </div>
  );
};

export default DeleteDataSafetyEyes;
