import React from "react";
import { useNavigate } from "react-router-dom"; //For navigate

const LookAtDataProducts: React.FC = () => {
  const navigate = useNavigate();

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">What data from Products do you want to see?</h1>
        <hr className="title-separator" />
      </header>

      <div className="button-group">
        <button
          className="button"
          onClick={() => navigate("/look-at-data/finished-products/names")}
        >
          Names of Products
        </button>
        <button
          className="button"
          onClick={() =>
            navigate("/look-at-data/finished-products/sorted-by-time")
          }
        >
          Products by Time to Make
        </button>
        <button
          className="button"
          onClick={() =>
            navigate("/look-at-data/finished-products/sorted-by-cost")
          }
        >
          Products by Total Cost to Make
        </button>
        <button
          className="button"
          onClick={() =>
            navigate("/look-at-data/finished-products/sorted-by-price")
          }
        >
          Products by Sale Price
        </button>
        <button
          className="button"
          onClick={() =>
            navigate("/look-at-data/finished-products/sorted-by-stock")
          }
        >
          Products by Number in Stock
        </button>

        <button className="button" onClick={() => navigate("/look-at-data")}>
          Back
        </button>
      </div>
    </div>
  );
};

export default LookAtDataProducts;
