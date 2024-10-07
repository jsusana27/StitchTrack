import React, { useState } from "react";
import axios from "axios";
import { useNavigate, useLocation } from "react-router-dom";

const AddProductMaterialSafetyEyes: React.FC = () => {
  //State variables for safety eyes attributes
  const [size, setSize] = useState<number | "">(""); //Allow empty string for initial state
  const [color, setColor] = useState("");
  const [shape, setShape] = useState("");
  const [quantityUsed, setQuantityUsed] = useState<number | "">(""); //Allow empty string for initial state
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();
  const location = useLocation();

  //Retrieve the product name from the state using optional chaining
  const productName = location.state?.productName || "";

  const handleAddSafetyEyes = async () => {
    //Validate inputs
    if (!size || !color || !shape || !quantityUsed) {
      setError("Please fill out all fields.");
      return;
    }

    try {
      //Convert inputs to appropriate data types
      const parsedSize = typeof size === "string" ? parseInt(size, 10) : size;
      const parsedQuantityUsed =
        typeof quantityUsed === "string"
          ? parseFloat(quantityUsed)
          : quantityUsed;

      //Send the POST request with query parameters
      await axios.post(
        `${process.env.REACT_APP_API_URL}/FinishedProductMaterial/add-material-safety-eyes`,
        null,
        {
          params: {
            productName,
            size: parsedSize,
            color,
            shape,
            quantityUsed: parsedQuantityUsed,
          },
        }
      );

      //Success message with relevant details
      alert(
        `Success! Added ${quantityUsed} units of ${size}mm ${shape} safety eyes (${color}) to product '${productName}'.`
      );
      navigate("/"); //Navigate back to home
    } catch (error) {
      setError("Failed to add safety eyes. Please try again.");
      console.error("Error adding safety eyes:", error);
    }
  };

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">
          Enter New Safety Eyes Material Details for {productName}
        </h1>
        <hr className="title-separator" />
      </header>

      <div className="form-group">
        <label htmlFor="size">Size in mm:</label>
        <input
          id="size"
          type="number"
          value={size === "" ? "" : size} //Handle empty string for numeric input
          onChange={(e) =>
            setSize(e.target.value ? parseInt(e.target.value) : "")
          }
        />

        <label htmlFor="color">Color:</label>
        <input
          id="color"
          type="text"
          value={color}
          onChange={(e) => setColor(e.target.value)}
        />

        <label htmlFor="shape">Shape:</label>
        <input
          id="shape"
          type="text"
          value={shape}
          onChange={(e) => setShape(e.target.value)}
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
        <button className="button" onClick={handleAddSafetyEyes}>
          Add Safety Eyes
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

export default AddProductMaterialSafetyEyes;
