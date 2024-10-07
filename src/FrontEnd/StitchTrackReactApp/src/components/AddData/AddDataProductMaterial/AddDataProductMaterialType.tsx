import React from "react";
import { useNavigate, useLocation } from "react-router-dom";

const AddDataProductMaterialType: React.FC = () => {
  const navigate = useNavigate();
  const location = useLocation();

  //Retrieve the product name and ID from the previous component's state
  const { productName, productId } = location.state || {};

  const handleSelectMaterial = (materialType: string) => {
    //Navigate to the appropriate material addition page, passing the product name and ID
    navigate(`/add-data/product-material/type/${materialType}`, {
      state: { productName, productId },
    });
  };

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">
          Is the new material Yarn, Safety Eyes, or Stuffing?
        </h1>
        <hr className="title-separator" />
      </header>

      <div className="button-group">
        <button className="button" onClick={() => handleSelectMaterial("yarn")}>
          Yarn
        </button>
        <button
          className="button"
          onClick={() => handleSelectMaterial("safety-eyes")}
        >
          Safety Eyes
        </button>
        <button
          className="button"
          onClick={() => handleSelectMaterial("stuffing")}
        >
          Stuffing
        </button>
        <button className="button" onClick={() => navigate("/add-data")}>
          Back to Start
        </button>
      </div>
    </div>
  );
};

export default AddDataProductMaterialType;
