import React from "react";
import { useNavigate } from "react-router-dom"; //For navigate

const LookAtDataStuffing: React.FC = () => {
  const navigate = useNavigate();

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">What data from Stuffing do you want to see?</h1>
        <hr className="title-separator" />
      </header>

      <div className="button-group">
        <button
          className="button"
          onClick={() => navigate("/look-at-data/stuffing/brands")}
        >
          Brands
        </button>
        <button
          className="button"
          onClick={() => navigate("/look-at-data/stuffing/types")}
        >
          Types
        </button>
        <button
          className="button"
          onClick={() => navigate("/look-at-data/stuffing/prices")}
        >
          Stuffing by Price per 5 lbs
        </button>

        <button className="button" onClick={() => navigate("/look-at-data")}>
          Back
        </button>
      </div>
    </div>
  );
};

export default LookAtDataStuffing;
