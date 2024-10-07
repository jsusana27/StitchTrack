import React from "react";
import { useNavigate } from "react-router-dom"; //For navigate

const ModifyDataQuantity: React.FC = () => {
  const navigate = useNavigate();

  return (
    <div className="container">
      {/* Title Section */}
      <header className="header">
        <h1 className="title">
          What type of data do you want to update the quantity of?
        </h1>
        <hr className="title-separator" />
      </header>

      {/* Button Group for Quantity Updates */}
      <div className="button-group">
        <button
          className="button"
          onClick={() => navigate("/modify-data/quantity-update/product-check")}
        >
          Finished Product
        </button>
        <button
          className="button"
          onClick={() => navigate("/modify-data/quantity-update/yarn-check")}
        >
          Yarn
        </button>
        <button
          className="button"
          onClick={() =>
            navigate("/modify-data/quantity-update/safety-eyes-check")
          }
        >
          Safety Eyes
        </button>
        <button
          className="button"
          onClick={() =>
            navigate("/modify-data/quantity-update/product-material-name")
          }
        >
          Material for a Product
        </button>

        {/* Back Button */}
        <button
          className="button back-button"
          onClick={() => navigate("/modify-data")}
        >
          Back
        </button>
      </div>
    </div>
  );
};

export default ModifyDataQuantity;
