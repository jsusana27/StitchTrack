import React, { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

const ModifyDataQuantityProductCheck: React.FC = () => {
  const [name, setName] = useState("");
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleNext = async () => {
    try {
      //Validate input
      if (!name.trim()) {
        setError("Product name is required.");
        return;
      }

      //Check if the finished product exists in the backend
      const response = await axios.get(
        `${process.env.REACT_APP_API_URL}/FinishedProduct/check-existence`,
        { params: { name } }
      );

      if (response.data.exists) {
        //Alert the user that a matching finished product was found
        alert("Matching finished product found!");

        //Navigate to the update page if the product exists
        navigate("/modify-data/quantity-update/product-check/product-update", {
          state: { name },
        });
      } else {
        setError(
          "The specified finished product does not exist in the database."
        );
      }
    } catch (err) {
      console.error("Error checking finished product existence", err);
      setError("Failed to check finished product existence. Please try again.");
    }
  };

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">Modify Finished Product Quantity</h1>
        <hr className="title-separator" />
      </header>

      {/* Form Fields */}
      <div className="form-group">
        <label htmlFor="productName-input">Product Name:</label>{" "}
        {/* Added htmlFor attribute */}
        <input
          id="productName-input" //Added id attribute
          type="text"
          value={name}
          onChange={(e) => setName(e.target.value)}
        />
      </div>

      {/* Error Message */}
      {error && <div className="error-message">{error}</div>}

      {/* Buttons */}
      <div className="button-group">
        <button className="button" onClick={handleNext}>
          Next
        </button>
        <button className="button" onClick={() => navigate("/modify-data")}>
          Back
        </button>
      </div>
    </div>
  );
};

export default ModifyDataQuantityProductCheck;
