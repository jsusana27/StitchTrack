import React, { useState } from "react";
import axios from "axios";
import { useNavigate, useLocation } from "react-router-dom";

const AddProductMaterialStuffing: React.FC = () => {
  const [brand, setBrand] = useState("");
  const [type, setType] = useState("");
  const [quantityUsed, setQuantityUsed] = useState<number | "">(""); //Allow empty string initially
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();
  const location = useLocation();

  //Retrieve the productName from the state
  const { productName } = location.state || {}; //Removed productId as it's not used

  const handleAddStuffing = async () => {
    //Validate inputs
    if (!brand || !type || !quantityUsed) {
      setError("Please fill out all fields.");
      return;
    }

    try {
      //Convert inputs only if they're not empty strings
      const parsedQuantityUsed =
        typeof quantityUsed === "string"
          ? parseFloat(quantityUsed)
          : quantityUsed;

      //Send the POST request with query parameters
      await axios.post(
        `${process.env.REACT_APP_API_URL}/FinishedProductMaterial/add-material-stuffing`,
        null,
        {
          params: {
            productName,
            brand,
            type,
            quantityUsed: parsedQuantityUsed,
          },
        }
      );

      alert(
        `Success! Added ${quantityUsed} units of ${brand} stuffing (${type}) to product '${productName}'.`
      ); //Show success message
      navigate("/"); //Navigate back to home
    } catch (error) {
      setError("Failed to add stuffing. Please try again.");
      console.error("Error adding stuffing:", error);
    }
  };

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">
          Enter New Stuffing Material Details for {productName}
        </h1>
        <hr className="title-separator" />
      </header>

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

        <label htmlFor="quantityUsed">Quantity:</label>
        <input
          id="quantityUsed"
          type="number"
          value={quantityUsed === "" ? "" : quantityUsed} //Handle empty string for numeric input
          onChange={(e) =>
            setQuantityUsed(e.target.value ? parseFloat(e.target.value) : "")
          }
        />
      </div>

      {error && <div className="error-message">{error}</div>}

      <div className="button-group">
        <button className="button" onClick={handleAddStuffing}>
          Add Stuffing
        </button>
        <button
          className="button"
          onClick={() => navigate("/add-data/product-material/type")}
        >
          Back
        </button>
      </div>
    </div>
  );
};

export default AddProductMaterialStuffing;
