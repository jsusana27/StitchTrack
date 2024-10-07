import React, { useState, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import axios from "axios";

const ModifyDataQuantityProductMaterialYarn: React.FC = () => {
  //Extract the `productName` from the URL parameters
  const { productName } = useParams<{ productName: string }>();

  //State variables to capture input values for the form fields
  const [brand, setBrand] = useState<string>("");
  const [fiberType, setFiberType] = useState<string>("");
  const [fiberWeight, setFiberWeight] = useState<string>("");
  const [color, setColor] = useState<string>("");
  const [newQuantity, setNewQuantity] = useState<string>("");

  const navigate = useNavigate(); //Hook to navigate programmatically

  //Log the received product name when the component mounts or updates
  useEffect(() => {
    console.log(`Received Product Name: ${productName}`);
  }, [productName]);

  //Function to handle the quantity modification process
  const handleModify = async () => {
    //Check if all required fields are filled in
    if (!brand || !fiberType || !fiberWeight || !color || !newQuantity) {
      alert("Please fill in all fields with valid data.");
      return;
    }

    try {
      //Construct query parameters for the API request
      const queryParams = `productName=${encodeURIComponent(
        productName ?? ""
      )}&brand=${encodeURIComponent(brand)}&fiberType=${encodeURIComponent(
        fiberType
      )}&fiberWeight=${encodeURIComponent(
        fiberWeight
      )}&color=${encodeURIComponent(color)}&newQuantity=${encodeURIComponent(
        newQuantity
      )}`;

      //Generate the full API URL for updating the quantity
      const apiUrl = `${process.env.REACT_APP_API_URL}/FinishedProductMaterial/update-quantity-yarn?${queryParams}`;
      console.log(`API URL: ${apiUrl}`);

      //Send the PUT request to update the quantity
      const response = await axios.put(apiUrl);

      //Check if the update was successful
      if (response.status === 200) {
        alert("Yarn quantity updated successfully!");
        navigate("/modify-data/quantity-update"); //Navigate back to the quantity update page
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
      {/* Page Header */}
      <header className="header">
        <h1 className="title">Modify Product Material Quantity - Yarn</h1>
        <hr className="title-separator" />
      </header>

      {/* Input Fields for Yarn Attributes */}
      <div className="form-group">
        <input
          type="text"
          value={brand}
          onChange={(e) => setBrand(e.target.value)}
          placeholder="Brand"
          className="input"
        />
      </div>
      <div className="form-group">
        <input
          type="text"
          value={fiberType}
          onChange={(e) => setFiberType(e.target.value)}
          placeholder="Fiber Type"
          className="input"
        />
      </div>
      <div className="form-group">
        <input
          type="text"
          value={fiberWeight}
          onChange={(e) => setFiberWeight(e.target.value)}
          placeholder="Fiber Weight"
          className="input"
        />
      </div>
      <div className="form-group">
        <input
          type="text"
          value={color}
          onChange={(e) => setColor(e.target.value)}
          placeholder="Color"
          className="input"
        />
      </div>
      <div className="form-group">
        <input
          type="text"
          value={newQuantity}
          onChange={(e) => setNewQuantity(e.target.value)}
          placeholder="New Quantity"
          className="input"
        />
      </div>

      {/* Button Group for Modify and Back actions */}
      <div className="button-group">
        <button onClick={handleModify} className="button">
          Modify
        </button>
        <button
          onClick={() => navigate("/modify-data")}
          className="button back-button"
        >
          Back
        </button>
      </div>
    </div>
  );
};

export default ModifyDataQuantityProductMaterialYarn;
