import React, { useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import axios from "axios";

const DeleteDataProductMaterialStuffing: React.FC = () => {
  const { productName } = useParams<{ productName: string }>();
  const [brand, setBrand] = useState<string>("");
  const [type, setType] = useState<string>("");
  const navigate = useNavigate();

  //Handle the delete request for Stuffing
  const handleDelete = async () => {
    //Validate inputs
    if (!brand || !type) {
      alert("Please fill in all fields.");
      return;
    }

    try {
      //Call the API with query parameters
      const response = await axios.delete(
        `${process.env.REACT_APP_API_URL}/FinishedProductMaterial/delete-material-stuffing`,
        {
          params: {
            productName,
            brand,
            type,
          },
        }
      );

      if (response.status === 200) {
        alert("Stuffing deleted successfully from the product!");
        navigate("/delete-data");
      } else {
        alert("Failed to delete Stuffing. Please try again.");
      }
    } catch (error) {
      console.error("Error deleting Stuffing:", error);
      alert("An error occurred while deleting the Stuffing.");
    }
  };

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">Enter Stuffing Details to Delete from Product</h1>
        <hr className="title-separator" />
      </header>

      {/* Form Inputs for Stuffing Details */}
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
        <label htmlFor="type" className="input-label">
          Type:
        </label>{" "}
        {/* Updated label */}
        <input
          id="type" //Added id for the input field
          type="text"
          value={type}
          onChange={(e) => setType(e.target.value)}
          className="input"
        />
      </div>

      {/* Delete Button */}
      <div className="button-group">
        <button onClick={handleDelete} className="button">
          Delete Stuffing
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

export default DeleteDataProductMaterialStuffing;
