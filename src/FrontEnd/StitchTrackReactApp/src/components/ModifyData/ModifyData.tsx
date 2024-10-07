import React from "react";
import { useNavigate } from "react-router-dom"; //For navigation

const ModifyData: React.FC = () => {
  const navigate = useNavigate();

  return (
    <div className="container">
      {/* Title Section */}
      <header className="header">
        <h1 className="title">What data do you want to modify?</h1>
        <hr className="title-separator" />
      </header>

      {/* Button Group */}
      <div className="button-group">
        <button
          className="button"
          onClick={() => navigate("/modify-data/quantity-update")}
        >
          Quantity Update
        </button>
        <button
          className="button"
          onClick={() => navigate("/modify-data/product-sale-price")}
        >
          Product Sale Price
        </button>
        <button
          className="button"
          onClick={() => navigate("/modify-data/product-time")}
        >
          Estimated Time to Make a Product
        </button>
        <button
          className="button"
          onClick={() => navigate("/modify-data/customer")}
        >
          Customer Phone # / Email Address
        </button>

        {/* Back Button */}
        <button className="button back-button" onClick={() => navigate("/")}>
          Back
        </button>
      </div>
    </div>
  );
};

export default ModifyData;
