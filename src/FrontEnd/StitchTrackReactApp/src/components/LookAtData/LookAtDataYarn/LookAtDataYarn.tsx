import React from "react";
import { useNavigate } from "react-router-dom";

const LookAtDataYarn: React.FC = () => {
  const navigate = useNavigate();

  const handleNumberInStockClick = () => {
    navigate("/look-at-data/yarn/number-in-stock"); //Navigates to the yarn sorted by stock page
  };

  return (
    <div className="container">
      {/* Title Section */}
      <header className="header">
        <h1 className="title">What data from Yarn do you want to see?</h1>
        <hr className="title-separator" />
      </header>

      {/* Button Group */}
      <div className="button-group">
        <button
          className="button"
          onClick={() => navigate("/look-at-data/yarn/brands")}
        >
          All Brands
        </button>
        <button
          className="button"
          onClick={() => navigate("/look-at-data/yarn/colors")}
        >
          All Colors
        </button>
        <button
          className="button"
          onClick={() => navigate("/look-at-data/yarn/fiber-types")}
        >
          All Fiber Types
        </button>
        <button
          className="button"
          onClick={() => navigate("/look-at-data/yarn/fiber-weights")}
        >
          All Fiber Weights
        </button>
        <button
          className="button"
          onClick={() => navigate("/look-at-data/yarn/prices")}
        >
          All Yarn by Prices
        </button>
        <button
          className="button"
          onClick={() => navigate("/look-at-data/yarn/yardage")}
        >
          All Yarn by Yardage/Skein
        </button>
        <button
          className="button"
          onClick={() => navigate("/look-at-data/yarn/grams")}
        >
          All Yarn by Grams/Skein
        </button>
        <button className="button" onClick={handleNumberInStockClick}>
          All Yarn by Number in Stock
        </button>
        <button className="button" onClick={() => navigate("/look-at-data")}>
          Back
        </button>
      </div>
    </div>
  );
};

export default LookAtDataYarn;
