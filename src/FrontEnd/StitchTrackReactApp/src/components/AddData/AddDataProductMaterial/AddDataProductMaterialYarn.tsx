import React, { useState } from "react";
import axios from "axios";
import { useNavigate, useLocation } from "react-router-dom";

const AddProductMaterialYarn: React.FC = () => {
  const [brand, setBrand] = useState("");
  const [fiberType, setFiberType] = useState("");
  const [fiberWeight, setFiberWeight] = useState<number | "">(""); //Adjust type to allow empty string
  const [color, setColor] = useState("");
  const [quantityUsed, setQuantityUsed] = useState<number | "">(""); //Adjust type to allow empty string
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();
  const location = useLocation();

  //Retrieve the product name from the state using optional chaining
  const productName = location.state?.productName || "";

  const handleAddYarn = async () => {
    //Validate inputs
    if (!brand || !fiberType || !fiberWeight || !color || !quantityUsed) {
      setError("Please fill out all fields.");
      return;
    }

    try {
      //Convert inputs only if they're not empty strings
      const parsedFiberWeight =
        typeof fiberWeight === "string"
          ? parseInt(fiberWeight, 10)
          : fiberWeight;
      const parsedQuantityUsed =
        typeof quantityUsed === "string"
          ? parseFloat(quantityUsed)
          : quantityUsed;

      //Send the POST request with query parameters
      await axios.post(
        `${process.env.REACT_APP_API_URL}/FinishedProductMaterial/add-material-yarn`,
        null,
        {
          params: {
            productName,
            brand,
            fiberType,
            fiberWeight: parsedFiberWeight,
            color,
            quantityUsed: parsedQuantityUsed,
          },
        }
      );

      alert(
        `Success! Added ${quantityUsed} units of ${brand} yarn (${fiberType}, ${color}) to product '${productName}'.`
      ); //Show success message
      navigate("/"); //Navigate back to home
    } catch (error) {
      setError("Failed to add yarn. Please try again.");
      console.error("Error adding yarn:", error);
    }
  };

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">
          Enter New Yarn Material Details for {productName}
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
          type="number"
          value={fiberWeight === "" ? "" : fiberWeight} //Handle empty string for numeric input
          onChange={(e) =>
            setFiberWeight(e.target.value ? parseInt(e.target.value) : "")
          }
        />

        <label htmlFor="color">Color:</label>
        <input
          id="color"
          type="text"
          value={color}
          onChange={(e) => setColor(e.target.value)}
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
        <button className="button" onClick={handleAddYarn}>
          Add Yarn
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

export default AddProductMaterialYarn;
