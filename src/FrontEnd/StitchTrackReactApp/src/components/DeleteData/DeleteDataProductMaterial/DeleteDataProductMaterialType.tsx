import React from "react";
import { useNavigate, useParams } from "react-router-dom"; //For navigate

const DeleteDataProductMaterialType: React.FC = () => {
  const navigate = useNavigate();
  const { productName } = useParams<{ productName: string }>();

  const handleNavigation = (materialType: string) => {
    navigate(`/delete-data/product-material/${productName}/${materialType}`);
  };

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">Select the Material Type to Delete</h1>
        <hr className="title-separator" />
      </header>

      <div className="button-group">
        <button onClick={() => handleNavigation("Yarn")} className="button">
          Yarn
        </button>
        <button
          onClick={() => handleNavigation("SafetyEyes")}
          className="button"
        >
          Safety Eyes
        </button>
        <button onClick={() => handleNavigation("Stuffing")} className="button">
          Stuffing
        </button>

        <button
          onClick={() => navigate("/delete-data")}
          className="button back-button"
        >
          Back
        </button>
      </div>
    </div>
  );
};

export default DeleteDataProductMaterialType;
