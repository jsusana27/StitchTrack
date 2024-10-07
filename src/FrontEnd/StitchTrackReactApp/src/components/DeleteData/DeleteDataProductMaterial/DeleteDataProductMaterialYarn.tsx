import React, { useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import axios from "axios";

const DeleteDataProductMaterialYarn: React.FC = () => {
  const { productName } = useParams<{ productName: string }>();
  const [brand, setBrand] = useState<string>("");
  const [fiberType, setFiberType] = useState<string>("");
  const [fiberWeight, setFiberWeight] = useState<string>("");
  const [color, setColor] = useState<string>("");
  const navigate = useNavigate();

  const handleDelete = async () => {
    //Validate inputs
    if (!brand || !fiberType || !fiberWeight || !color) {
      alert("Please fill in all fields.");
      return;
    }

    try {
      const response = await axios.delete(
        `${process.env.REACT_APP_API_URL}/FinishedProductMaterial/delete-material-yarn`,
        {
          params: {
            productName,
            brand,
            fiberType,
            fiberWeight: parseFloat(fiberWeight), //Convert fiberWeight to a number
            color,
          },
        }
      );

      if (response.status === 200) {
        alert("Yarn material deleted successfully from the product!");
        navigate("/delete-data");
      } else {
        alert("Failed to delete yarn material. Please try again.");
      }
    } catch (error) {
      console.error("Error deleting yarn material:", error);
      alert("An error occurred while deleting the yarn material.");
    }
  };

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">Enter Yarn Details to Delete from Product</h1>
        <hr className="title-separator" />
      </header>

      {/* Form Inputs for Yarn Details */}
      <div className="form-group">
        <label htmlFor="brand" className="input-label">
          Brand:
        </label>{" "}
        {/* Updated label */}
        <input
          id="brand" //Added id for the input field
          type="text"
          value={brand}
          onChange={(e) => setBrand(e.target.value)}
          className="input"
        />
      </div>
      <div className="form-group">
        <label htmlFor="fiberType" className="input-label">
          Fiber Type:
        </label>{" "}
        {/* Updated label */}
        <input
          id="fiberType" //Added id for the input field
          type="text"
          value={fiberType}
          onChange={(e) => setFiberType(e.target.value)}
          className="input"
        />
      </div>
      <div className="form-group">
        <label htmlFor="fiberWeight" className="input-label">
          Fiber Weight:
        </label>{" "}
        {/* Updated label */}
        <input
          id="fiberWeight" //Added id for the input field
          type="text"
          value={fiberWeight}
          onChange={(e) => setFiberWeight(e.target.value)}
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

      {/* Delete Button */}
      <div className="button-group">
        <button onClick={handleDelete} className="button">
          Delete Yarn
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

export default DeleteDataProductMaterialYarn;
