import React, { useState, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import axios from "axios";

const ModifyDataQuantityProductMaterialSafetyEyes: React.FC = () => {
  const { productName } = useParams<{ productName: string }>();
  const [size, setSize] = useState<string>(""); //Change size to string initially for easier state management
  const [color, setColor] = useState<string>("");
  const [shape, setShape] = useState<string>("");
  const [newQuantity, setNewQuantity] = useState<string>(""); //Change newQuantity to string for consistency
  const navigate = useNavigate();

  //Debug the product name in the component
  useEffect(() => {
    if (!productName) {
      console.error("Product name is not defined!");
    } else {
      console.log(`Product name: ${productName}`);
    }
  }, [productName]);

  //Handle the API request to update the quantity
  const handleModify = async () => {
    //Convert size and newQuantity to numbers for backend compatibility
    const parsedSize = parseFloat(size);
    const parsedNewQuantity = parseFloat(newQuantity);

    //Check for invalid inputs
    if (
      !size ||
      !color ||
      !shape ||
      isNaN(parsedSize) ||
      isNaN(parsedNewQuantity)
    ) {
      alert("Please fill in all fields with valid data.");
      return;
    }

    try {
      //Construct the query parameters for the update request
      const queryParams = `productName=${encodeURIComponent(
        productName ?? ""
      )}&size=${encodeURIComponent(size)}&color=${encodeURIComponent(
        color
      )}&shape=${encodeURIComponent(shape)}&newQuantity=${encodeURIComponent(
        newQuantity
      )}`;

      //Log the complete API URL to debug
      const apiUrl = `${process.env.REACT_APP_API_URL}/FinishedProductMaterial/update-quantity-safety-eyes?${queryParams}`;
      console.log(`API URL: ${apiUrl}`);

      //Make the API call using query parameters instead of a request body
      const response = await axios.put(apiUrl);

      if (response.status === 200) {
        alert("Safety eyes quantity updated successfully!");
        navigate("/modify-data/quantity-update");
      } else {
        alert("Failed to update quantity. Please try again.");
      }
    } catch (error) {
      console.error("Error updating the quantity:", error);
      alert("An error occurred while updating the quantity.");
    }
  };

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">
          Modify Product Material Quantity - Safety Eyes
        </h1>
        <hr className="title-separator" />
      </header>

      {/* Form for entering safety eyes details */}
      <div className="input-group">
        <input
          type="text"
          value={size}
          onChange={(e) => setSize(e.target.value)} //No parsing here
          placeholder="Size in MM"
          className="input"
          style={{
            width: "300px",
            height: "40px",
            padding: "10px",
            fontSize: "16px",
            marginBottom: "20px",
          }}
        />
      </div>
      <div className="input-group">
        <input
          type="text"
          value={color}
          onChange={(e) => setColor(e.target.value)}
          placeholder="Color"
          className="input"
          style={{
            width: "300px",
            height: "40px",
            padding: "10px",
            fontSize: "16px",
            marginBottom: "20px",
          }}
        />
      </div>
      <div className="input-group">
        <input
          type="text"
          value={shape}
          onChange={(e) => setShape(e.target.value)}
          placeholder="Shape"
          className="input"
          style={{
            width: "300px",
            height: "40px",
            padding: "10px",
            fontSize: "16px",
            marginBottom: "20px",
          }}
        />
      </div>
      <div className="input-group">
        <input
          type="text"
          value={newQuantity}
          onChange={(e) => setNewQuantity(e.target.value)} //No parsing here
          placeholder="New Quantity"
          className="input"
          style={{
            width: "300px",
            height: "40px",
            padding: "10px",
            fontSize: "16px",
            marginBottom: "20px",
          }}
        />
      </div>

      {/* Modify Button */}
      <div className="button-group">
        <button onClick={handleModify} className="button">
          Modify
        </button>

        {/* Back Button */}
        <div className="button-group">
          <button
            onClick={() => navigate("/modify-data/quantity-update")}
            className="button back-button"
          >
            Back
          </button>
        </div>
      </div>
    </div>
  );
};

export default ModifyDataQuantityProductMaterialSafetyEyes;
