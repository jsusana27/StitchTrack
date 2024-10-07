import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

const LookAtDataMaterialsForProductInput: React.FC = () => {
  const navigate = useNavigate();
  const [productName, setProductName] = useState<string>(""); //Store the input value

  //Function to handle form submission
  const handleSubmit = () => {
    if (productName.trim() === "") {
      alert("Please enter a product name");
    } else {
      navigate(`/look-at-data/materials-needed-output/${productName}`);
    }
  };

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">
          What is the name of the product whose materials you want to see?
        </h1>
        <hr className="title-separator" />
      </header>

      {/* Input field for product name */}
      <div className="input-group">
        <input
          type="text"
          value={productName}
          onChange={(e) => setProductName(e.target.value)}
          placeholder="Enter product name"
          className="input"
          style={{
            width: "300px",
            height: "40px",
            padding: "10px",
            fontSize: "16px",
            marginBottom: "20px",
          }}
        />
      </div>

      {/* Submit button */}
      <div className="button-group">
        <button
          onClick={handleSubmit}
          className="button"
          style={{
            marginBottom: "20px",
          }}
        >
          Next
        </button>
      </div>

      {/* Back button */}
      <div className="button-group">
        <button onClick={() => navigate("/look-at-data")} className="button">
          Back
        </button>
      </div>
    </div>
  );
};

export default LookAtDataMaterialsForProductInput;
