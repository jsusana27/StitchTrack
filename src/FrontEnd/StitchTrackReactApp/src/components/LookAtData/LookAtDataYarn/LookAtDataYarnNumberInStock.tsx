import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

//Component to display a table of yarns sorted by the number of skeins in stock
const LookAtDataYarnNumberInStock: React.FC = () => {
  const navigate = useNavigate(); //React Router's hook for navigation

  //State variables to manage yarn data, loading status, and potential errors
  const [yarns, setYarns] = useState<any[]>([]); //Holds the yarn data fetched from the API
  const [loading, setLoading] = useState(true); //Tracks whether the data is currently being fetched
  const [error, setError] = useState<string | null>(null); //Stores any error messages encountered during data fetch

  //useEffect hook to fetch yarn data from the API on component mount
  useEffect(() => {
    const fetchYarns = async () => {
      try {
        //Make an API call to retrieve yarn data sorted by number in stock
        const response = await axios.get(
          `${process.env.REACT_APP_API_URL}/Yarn/sorted-by-number-in-stock`
        );

        //Check the format of the response and update state accordingly
        if (Array.isArray(response.data)) {
          setYarns(response.data); //Use the direct array if it is returned
        } else if (response.data.$values) {
          setYarns(response.data.$values); //Use $values if the data is in a nested format (e.g., .NET serialized data)
        } else {
          setError("Unexpected data format from API"); //Handle unexpected response structure
        }

        setLoading(false); //Set loading state to false once data is fetched
      } catch (err) {
        console.error("Error fetching yarns by number in stock:", err); //Log error to console for debugging
        setError("Error fetching yarns by number in stock"); //Set a user-facing error message
        setLoading(false); //Ensure loading state is reset even if there's an error
      }
    };

    fetchYarns(); //Call the fetch function on component mount
  }, []); //Empty dependency array means this effect runs only once when the component mounts

  //Display loading message while data is being fetched
  if (loading) {
    return <div>Loading yarns...</div>;
  }

  //Display error message if an error occurred during data fetch
  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div className="container">
      {/* Page header with title and styling */}
      <header className="header">
        <h1 className="title">Yarns Sorted by Number in Stock</h1>
        <hr className="title-separator" />
      </header>

      {/* Table displaying the yarn data */}
      <table className="table">
        <thead>
          <tr>
            {/* Define the table column headers */}
            <th>Brand</th>
            <th>Fiber Type</th>
            <th>Fiber Weight</th>
            <th>Color</th>
            <th>Yardage/Skein</th>
            <th>Grams/Skein</th>
            <th>Number of Skeins in Stock</th>
            <th>Price</th>
          </tr>
        </thead>
        <tbody>
          {/* Conditionally render yarn data if available */}
          {yarns.length > 0 ? (
            yarns.map((yarn) => (
              <tr
                key={`${yarn.brand}-${yarn.fiberType}-${yarn.color}-${yarn.numberOfSkeinsOwned}`}
              >
                <td>{yarn.brand}</td>
                <td>{yarn.fiberType}</td>
                <td>{yarn.fiberWeight}</td>
                <td>{yarn.color}</td>
                <td>{yarn.yardagePerSkein}</td>
                <td>{yarn.gramsPerSkein}</td>
                <td>{yarn.numberOfSkeinsOwned}</td>
                <td>{yarn.price}</td>
              </tr>
            ))
          ) : (
            //Fallback message when no yarn data is available
            <tr>
              <td colSpan={8}>No yarns available</td>
            </tr>
          )}
        </tbody>
      </table>

      {/* Back button to navigate to the yarn overview page */}
      <button className="button" onClick={() => navigate("/look-at-data/yarn")}>
        Back
      </button>
    </div>
  );
};

export default LookAtDataYarnNumberInStock;
