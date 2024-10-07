import React from "react";
import { useNavigate } from "react-router-dom"; //For navigation

const AddData: React.FC = () => {
  const navigate = useNavigate();

  return (
    <div className="container">
      {/* Title Section */}
      <header className="header">
        <h1 className="title">What data do you want to add?</h1>
        <hr className="title-separator" />
      </header>

      {/* Button Group */}
      <div className="button-group">
        <button className="button" onClick={() => navigate("/add-data/yarn")}>
          New yarn
        </button>
        <button
          className="button"
          onClick={() => navigate("/add-data/safety-eyes")}
        >
          New safety eyes
        </button>
        <button
          className="button"
          onClick={() => navigate("/add-data/stuffing")}
        >
          New stuffing
        </button>
        <button
          className="button"
          onClick={() => navigate("/add-data/product")}
        >
          New finished product
        </button>
        <button
          className="button"
          onClick={() => navigate("/add-data/product-material")}
        >
          New material for finished product
        </button>
        <button
          className="button"
          onClick={() => navigate("/add-data/customer")}
        >
          New customer
        </button>
        <button className="button" onClick={() => navigate("/add-data/order")}>
          New order
        </button>
        {/* Back Button */}
        <button className="button" onClick={() => navigate("/")}>
          Back
        </button>
      </div>
    </div>
  );
};

export default AddData;
