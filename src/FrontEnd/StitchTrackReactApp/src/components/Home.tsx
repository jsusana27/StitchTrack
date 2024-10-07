import React from "react";
import { useNavigate } from "react-router-dom";

const Home: React.FC = () => {
  const navigate = useNavigate();

  // Button click handlers
  const handleLookAtDataClick = () => {
    navigate("/look-at-data");
  };

  const handleAddDataClick = () => {
    navigate("/add-data");
  };

  const handleModifyDataClick = () => {
    navigate("/modify-data");
  };

  const handleDeleteDataClick = () => {
    navigate("/delete-data");
  };

  return (
    <div className="container">
      {/* Title Section */}
      <header className="header">
        <h1 className="title">Stitched by Ading</h1>
        <hr className="title-separator" />
      </header>

      {/* Button Group */}
      <div className="button-group">
        <button className="button" onClick={handleLookAtDataClick}>
          Look at data
        </button>
        <button className="button" onClick={handleAddDataClick}>
          Add data
        </button>
        <button className="button" onClick={handleModifyDataClick}>
          Modify data
        </button>
        <button className="button" onClick={handleDeleteDataClick}>
          Delete data
        </button>
      </div>
    </div>
  );
};

export default Home;
