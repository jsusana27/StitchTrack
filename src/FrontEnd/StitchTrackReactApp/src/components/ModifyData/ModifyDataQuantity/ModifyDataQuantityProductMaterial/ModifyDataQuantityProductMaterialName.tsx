import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

const ModifyDataQuantityProductMaterialName: React.FC = () => {
  const [productName, setProductName] = useState<string>("");
  const navigate = useNavigate();

  const handleNext = () => {
    if (productName.trim() === "") {
      alert("Please enter a product name");
    } else {
      //Navigate to the next component, including the product name as a URL parameter
      navigate(
        `/modify-data/quantity-update/product-material-name/${encodeURIComponent(
          productName
        )}/product-material-type`
      );
    }
  };

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">
          What is the name of the product that has the material whose quantity
          you are modifying?
        </h1>
        <hr className="title-separator" />
      </header>

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

      <div className="button-group">
        <button onClick={handleNext} className="button">
          Next
        </button>

        <div className="button-group">
          <button
            onClick={() => navigate("/modify-data")}
            className="button back-button"
          >
            Back
          </button>
        </div>
      </div>
    </div>
  );
};

export default ModifyDataQuantityProductMaterialName;
