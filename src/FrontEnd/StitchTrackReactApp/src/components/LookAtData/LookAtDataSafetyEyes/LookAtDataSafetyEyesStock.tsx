import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const LookAtDataSafetyEyesStock: React.FC = () => {
  const navigate = useNavigate(); //React Router's hook for navigation
  const [safetyEyes, setSafetyEyes] = useState<any[]>([]); //State to store the list of safety eyes
  const [loading, setLoading] = useState(true); //Loading state to indicate data is being fetched
  const [error, setError] = useState<string | null>(null); //State to handle and display any errors

  //useEffect hook to fetch safety eyes data when the component mounts
  useEffect(() => {
    const fetchSafetyEyes = async () => {
      try {
        //API call to retrieve safety eyes sorted by stock
        const response = await axios.get(
          `${process.env.REACT_APP_API_URL}/SafetyEye/sorted-by-stock`
        );

        //Handle the different response formats that might be returned
        if (Array.isArray(response.data)) {
          setSafetyEyes(response.data);
        } else if (response.data.$values) {
          setSafetyEyes(response.data.$values);
        } else {
          setError("Unexpected data format from API");
        }

        setLoading(false); //Data fetching is complete
      } catch (err) {
        console.error("Error fetching safety eyes by stock:", err);
        setError("Error fetching safety eyes by stock"); //Set the error message
        setLoading(false); //Stop loading indicator
      }
    };

    fetchSafetyEyes(); //Trigger the API call on component mount
  }, []); //Empty dependency array ensures this runs only once

  //Conditional rendering for loading and error states
  if (loading) return <div>Loading safety eyes...</div>;
  if (error) return <div>{error}</div>;

  return (
    <div className="container">
      {/* Page header */}
      <header className="header">
        <h1 className="title">Safety Eyes by Stock</h1>
        <hr className="title-separator" />
      </header>

      {/* Table displaying safety eyes data */}
      <table className="table">
        <thead>
          <tr>
            <th>Size (mm)</th>
            <th>Color</th>
            <th>Shape</th>
            <th>Price</th>
            <th>Number in Stock</th>
          </tr>
        </thead>
        <tbody>
          {/* Render the list of safety eyes or display a message if no data is available */}
          {safetyEyes.length > 0 ? (
            safetyEyes.map((eye) => (
              <tr key={`${eye.sizeInMM}-${eye.color}-${eye.shape}`}>
                <td>{eye.sizeInMM}</td>
                <td>{eye.color}</td>
                <td>{eye.shape}</td>
                <td>${eye.price}</td>
                <td>{eye.numberOfEyesOwned}</td>
              </tr>
            ))
          ) : (
            <tr>
              {/* If no data is present, show a message across all columns */}
              <td colSpan={5}>No safety eyes available</td>
            </tr>
          )}
        </tbody>
      </table>

      {/* Back button to navigate to the safety eyes overview page */}
      <button
        className="button"
        onClick={() => navigate("/look-at-data/safety-eyes")}
      >
        Back
      </button>
    </div>
  );
};

export default LookAtDataSafetyEyesStock;
