import React from "react";
import { useNavigate } from "react-router-dom"; //For navigation

const LookAtData: React.FC = () => {
  const navigate = useNavigate();

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">What data do you want to look at?</h1>
        <hr className="title-separator" />
      </header>

      {/* Button Group */}
      <div className="button-group">
        <button
          className="button"
          onClick={() => navigate("/look-at-data/yarn")}
        >
          Yarn
        </button>
        <button
          className="button"
          onClick={() => navigate("/look-at-data/safety-eyes")}
        >
          Safety Eyes
        </button>
        <button
          className="button"
          onClick={() => navigate("/look-at-data/stuffing")}
        >
          Stuffing
        </button>
        <button
          className="button"
          onClick={() => navigate("/look-at-data/finished-products")}
        >
          Products
        </button>
        <button
          className="button"
          onClick={() => navigate("/look-at-data/materials-needed-input")}
        >
          Materials Needed to Make a Product
        </button>
        <button
          className="button"
          onClick={() => navigate("/look-at-data/customers")}
        >
          All Customers
        </button>
        <button
          className="button"
          onClick={() => navigate("/look-at-data/orders")}
        >
          All General Order Information
        </button>
        <button
          className="button"
          onClick={() => navigate("/look-at-data/customer-purchased-input")}
        >
          Customer All Purchased Products
        </button>
        <button
          className="button"
          onClick={() => navigate("/look-at-data/sale-stats-input")}
        >
          Sale Stats for a Product
        </button>

        {/* Back Button */}
        <button className="button" onClick={() => navigate("/")}>
          Back
        </button>
      </div>
    </div>
  );
};

export default LookAtData;
