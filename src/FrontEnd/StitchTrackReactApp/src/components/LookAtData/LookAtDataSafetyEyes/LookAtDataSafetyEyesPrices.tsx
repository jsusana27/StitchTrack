import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

//Component to display a table of safety eyes sorted by price
const LookAtDataSafetyEyesPrices: React.FC = () => {
  const navigate = useNavigate(); //React Router's hook for navigation

  //State variables to manage safety eyes data, loading status, and any potential errors
  const [safetyEyes, setSafetyEyes] = useState<any[]>([]); //Holds the safety eyes data fetched from the API
  const [loading, setLoading] = useState(true); //Tracks whether the data is currently being fetched
  const [error, setError] = useState<string | null>(null); //Stores any error messages encountered during data fetch

  //useEffect hook to fetch safety eyes data from the API on component mount
  useEffect(() => {
    const fetchSafetyEyes = async () => {
      try {
        //Make an API call to retrieve safety eyes data sorted by price
        const response = await axios.get(
          `${process.env.REACT_APP_API_URL}/SafetyEye/sorted-by-price`
        );

        //Check the format of the response and update state accordingly
        if (Array.isArray(response.data)) {
          setSafetyEyes(response.data); //Use the direct array if it is returned
        } else if (response.data.$values) {
          setSafetyEyes(response.data.$values); //Use $values if the data is in a nested format (e.g., .NET serialized data)
        } else {
          setError("Unexpected data format from API"); //Handle unexpected response structure
        }

        setLoading(false); //Set loading state to false once data is fetched
      } catch (err) {
        console.error("Error fetching safety eyes by price:", err); //Log error to console for debugging
        setError("Error fetching safety eyes by price"); //Set a user-facing error message
        setLoading(false); //Ensure loading state is reset even if there's an error
      }
    };

    fetchSafetyEyes(); //Call the fetch function on component mount
  }, []); //Empty dependency array means this effect runs only once when the component mounts

  //Display loading message while data is being fetched
  if (loading) return <div>Loading safety eyes...</div>;

  //Display error message if an error occurred during data fetch
  if (error) return <div>{error}</div>;

  return (
    <div className="container">
      {/* Page header with title and styling */}
      <header className="header">
        <h1 className="title">Safety Eyes Sorted by Price</h1>
        <hr className="title-separator" />
      </header>

      {/* Table displaying the safety eyes data */}
      <table className="table">
        <thead>
          <tr>
            {/* Define the table column headers */}
            <th>Size (mm)</th>
            <th>Color</th>
            <th>Shape</th>
            <th>Number in Stock</th>
            <th>Price</th>
          </tr>
        </thead>
        <tbody>
          {/* Conditionally render safety eyes data if available */}
          {safetyEyes.length > 0 ? (
            safetyEyes.map((eye) => (
              <tr key={`${eye.sizeInMM}-${eye.color}-${eye.shape}`}>
                <td>{eye.sizeInMM}</td>
                <td>{eye.color}</td>
                <td>{eye.shape}</td>
                <td>{eye.numberOfEyesOwned}</td>
                <td>{eye.price}</td>
              </tr>
            ))
          ) : (
            //Fallback message when no safety eyes data is available
            <tr>
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

export default LookAtDataSafetyEyesPrices;
