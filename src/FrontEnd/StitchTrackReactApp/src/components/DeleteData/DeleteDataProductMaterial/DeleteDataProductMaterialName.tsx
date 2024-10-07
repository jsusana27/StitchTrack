import React, { useState } from "react";
import { useNavigate } from "react-router-dom"; //For navigate

const DeleteDataProductMaterialName: React.FC = () => {
  const [productName, setProductName] = useState<string>("");
  const navigate = useNavigate();

  const handleNext = () => {
    if (productName.trim() === "") {
      alert("Please enter a product name.");
    } else {
      navigate(`/delete-data/product-material/${productName}/type`);
    }
  };

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">
          What is the name of the product that has the material you want to
          delete?
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

export default DeleteDataProductMaterialName;
