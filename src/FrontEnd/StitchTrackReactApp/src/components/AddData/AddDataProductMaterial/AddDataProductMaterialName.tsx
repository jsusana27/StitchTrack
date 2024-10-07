import React, { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

const AddProductMaterialName: React.FC = () => {
  const [productName, setProductName] = useState("");
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleNext = async () => {
    if (!productName.trim()) {
      setError("Please enter the name of the finished product.");
      return;
    }

    try {
      //Call the API to get the product ID by name
      const response = await axios.get(
        `${process.env.REACT_APP_API_URL}/FinishedProduct/get-id-by-name`,
        {
          params: { name: productName },
        }
      );

      if (response.data?.productId) {
        //If product ID is found, navigate to the next step with the product ID and name
        navigate("/add-data/product-material/type", {
          state: { productName, productId: response.data.productId },
        });
      }
    } catch (error) {
      //Handle errors, such as product not found
      setError(
        "Failed to retrieve product ID. Make sure the product name is correct."
      );
      console.error("Error fetching product ID:", error);
    }
  };

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">
          What is the name of the product that is getting a new material added
          to it?
        </h1>
        <hr className="title-separator" />
      </header>

      <div className="form-group">
        <input
          type="text"
          value={productName}
          onChange={(e) => setProductName(e.target.value)}
          placeholder="Enter Finished Product Name"
        />
      </div>

      {error && <div className="error-message">{error}</div>}

      <div className="button-group">
        <button className="button" onClick={handleNext}>
          Next
        </button>
        <button className="button" onClick={() => navigate("/add-data")}>
          Back
        </button>
      </div>
    </div>
  );
};

export default AddProductMaterialName;
