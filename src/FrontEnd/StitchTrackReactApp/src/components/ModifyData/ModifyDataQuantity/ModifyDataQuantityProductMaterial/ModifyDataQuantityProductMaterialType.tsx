import React from "react";
import { useNavigate, useParams } from "react-router-dom"; //For navigate

const ModifyDataQuantityProductMaterialType: React.FC = () => {
  const navigate = useNavigate();
  const { productName } = useParams<{ productName: string }>();

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">Select the Product Material Type</h1>
        <hr className="title-separator" />
      </header>

      <div className="button-group">
        {/* Pass the product name correctly when navigating */}
        <button
          className="button"
          onClick={() =>
            navigate(
              `/modify-data/quantity-update/product-material-name/${encodeURIComponent(
                productName || ""
              )}/product-material-type/product-material-update/Yarn`
            )
          }
        >
          Yarn
        </button>
        <button
          className="button"
          onClick={() =>
            navigate(
              `/modify-data/quantity-update/product-material-name/${encodeURIComponent(
                productName || ""
              )}/product-material-type/product-material-update/SafetyEyes`
            )
          }
        >
          Safety Eyes
        </button>

        <div className="button-group">
          <button
            onClick={() => navigate("/modify-data")}
            className="button back-button"
          >
            Back
          </button>
        </div>
      </div>
    </div>
  );
};

export default ModifyDataQuantityProductMaterialType;
