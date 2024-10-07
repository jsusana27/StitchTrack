import React from "react";
import { useNavigate } from "react-router-dom"; //For navigate

const LookAtDataSafetyEyes: React.FC = () => {
  const navigate = useNavigate();

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">
          What data from Safety Eyes do you want to see?
        </h1>
        <hr className="title-separator" />
      </header>

      <div className="button-group">
        <button
          className="button"
          onClick={() => navigate("/look-at-data/safety-eyes/sizes")}
        >
          Sizes in Millimeters
        </button>
        <button
          className="button"
          onClick={() => navigate("/look-at-data/safety-eyes/colors")}
        >
          Colors
        </button>
        <button
          className="button"
          onClick={() => navigate("/look-at-data/safety-eyes/shapes")}
        >
          Shapes
        </button>
        <button
          className="button"
          onClick={() => navigate("/look-at-data/safety-eyes/prices")}
        >
          Eyes by Price
        </button>
        <button
          className="button"
          onClick={() => navigate("/look-at-data/safety-eyes/stock")}
        >
          Eyes by Number in Stock
        </button>

        <button className="button" onClick={() => navigate("/look-at-data")}>
          Back
        </button>
      </div>
    </div>
  );
};

export default LookAtDataSafetyEyes;
