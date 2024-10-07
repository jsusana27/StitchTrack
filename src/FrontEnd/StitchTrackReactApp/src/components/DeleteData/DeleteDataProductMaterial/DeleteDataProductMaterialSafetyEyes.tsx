import React, { useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import axios from "axios";

const DeleteDataProductMaterialSafetyEyes: React.FC = () => {
  const { productName } = useParams<{ productName: string }>();
  const [size, setSize] = useState<string>("");
  const [color, setColor] = useState<string>("");
  const [shape, setShape] = useState<string>("");
  const navigate = useNavigate();

  //Handle the delete request for Safety Eyes
  const handleDelete = async () => {
    //Validate inputs
    if (!size || !color || !shape) {
      alert("Please fill in all fields.");
      return;
    }

    try {
      //Call the API with query parameters
      const response = await axios.delete(
        `${process.env.REACT_APP_API_URL}/FinishedProductMaterial/delete-material-safety-eyes`,
        {
          params: {
            productName,
            size: parseFloat(size), //Convert size to number
            color,
            shape,
          },
        }
      );

      if (response.status === 200) {
        alert("Safety Eyes deleted successfully from the product!");
        navigate("/delete-data");
      } else {
        alert("Failed to delete Safety Eyes. Please try again.");
      }
    } catch (error) {
      console.error("Error deleting Safety Eyes:", error);
      alert("An error occurred while deleting the Safety Eyes.");
    }
  };

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">
          Enter Safety Eyes Details to Delete from Product
        </h1>
        <hr className="title-separator" />
      </header>

      {/* Form Inputs for Safety Eyes Details */}
      <div className="form-group">
        <label htmlFor="size" className="input-label">
          Size in MM:
        </label>{" "}
        {/* Updated label */}
        <input
          id="size" //Added id for the input field
          type="text"
          value={size}
          onChange={(e) => setSize(e.target.value)}
          className="input"
        />
      </div>
      <div className="form-group">
        <label htmlFor="color" className="input-label">
          Color:
        </label>{" "}
        {/* Updated label */}
        <input
          id="color" //Added id for the input field
          type="text"
          value={color}
          onChange={(e) => setColor(e.target.value)}
          className="input"
        />
      </div>
      <div className="form-group">
        <label htmlFor="shape" className="input-label">
          Shape:
        </label>{" "}
        {/* Updated label */}
        <input
          id="shape" //Added id for the input field
          type="text"
          value={shape}
          onChange={(e) => setShape(e.target.value)}
          className="input"
        />
      </div>

      {/* Delete Button */}
      <div className="button-group">
        <button onClick={handleDelete} className="button">
          Delete Safety Eyes
        </button>
        <button
          onClick={() => navigate("/delete-data")}
          className="button back-button"
        >
          Back
        </button>
      </div>
    </div>
  );
};

export default DeleteDataProductMaterialSafetyEyes;
