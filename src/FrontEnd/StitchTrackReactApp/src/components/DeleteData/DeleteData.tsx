import React from "react";
import { useNavigate } from "react-router-dom"; //For navigation

const DeleteData: React.FC = () => {
  const navigate = useNavigate();

  return (
    <div className="container">
      {/* Title Section */}
      <header className="header">
        <h1 className="title">What data do you want to delete?</h1>
        <hr className="title-separator" />
      </header>

      {/* Button Group */}
      <div className="button-group">
        <button
          className="button"
          onClick={() => navigate("/delete-data/yarn")}
        >
          Yarn
        </button>
        <button
          className="button"
          onClick={() => navigate("/delete-data/safety-eyes")}
        >
          Safety Eye
        </button>
        <button
          className="button"
          onClick={() => navigate("/delete-data/stuffing")}
        >
          Stuffing
        </button>
        <button
          className="button"
          onClick={() => navigate("/delete-data/product")}
        >
          Product
        </button>
        <button
          className="button"
          onClick={() => navigate("/delete-data/product-material")}
        >
          Material for a Product
        </button>
        <button
          className="button"
          onClick={() => navigate("/delete-data/customer")}
        >
          Customer
        </button>
        <button
          className="button"
          onClick={() => navigate("/delete-data/order")}
        >
          Order
        </button>
        {/* Back Button */}
        <button className="button" onClick={() => navigate("/")}>
          Back
        </button>
      </div>
    </div>
  );
};

export default DeleteData;
