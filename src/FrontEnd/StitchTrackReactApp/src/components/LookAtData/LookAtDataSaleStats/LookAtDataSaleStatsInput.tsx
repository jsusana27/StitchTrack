import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

const LookAtDataSaleStatsInput: React.FC = () => {
  const [productName, setProductName] = useState("");
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleSubmit = async () => {
    try {
      //Since API expects the product name, we will use it directly
      if (productName.trim() === "") {
        setError("Product name cannot be empty.");
        return;
      }

      //Navigate to the sale stats page using the product name
      navigate(`/look-at-data/sale-stats-output/${productName.trim()}`);
    } catch (error) {
      console.error("Error navigating to sale stats page", error);
      setError("Error navigating to sale stats page.");
    }
  };

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">What is the name of the product?</h1>
        <hr className="title-separator" />
      </header>

      <div className="form-group">
        <input
          type="text"
          value={productName}
          onChange={(e) => setProductName(e.target.value)}
          placeholder="Enter product name"
        />
      </div>

      {error && <div className="error">{error}</div>}

      <div className="button-group">
        <button className="button" onClick={handleSubmit}>
          Next
        </button>
        <button className="button" onClick={() => navigate("/look-at-data")}>
          Back
        </button>
      </div>
    </div>
  );
};

export default LookAtDataSaleStatsInput;
