import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

//Component to display a table of yarns sorted by yardage per skein
const LookAtDataYarnYardagePerSkein: React.FC = () => {
  const navigate = useNavigate(); //React Router's hook for navigating between pages

  //State variables to manage yarn data, loading status, and potential errors
  const [yarns, setYarns] = useState<any[]>([]); //Holds the yarn data fetched from the API
  const [loading, setLoading] = useState(true); //Indicates whether data is being loaded
  const [error, setError] = useState<string | null>(null); //Stores any error messages from API call

  //useEffect hook to fetch yarn data on component mount
  useEffect(() => {
    const fetchYarns = async () => {
      try {
        //API call to get yarns sorted by yardage per skein
        const response = await axios.get(
          `${process.env.REACT_APP_API_URL}/Yarn/sorted-by-yardage`
        );

        //Check the format of the response data and update state accordingly
        if (Array.isArray(response.data)) {
          setYarns(response.data); //Set yarns if the data is a direct array
        } else if (response.data.$values) {
          setYarns(response.data.$values); //Handle cases where data is wrapped in a $values property (e.g., .NET serialized data)
        } else {
          setError("Unexpected data format from API"); //Handle unexpected response structure
        }

        setLoading(false); //Set loading state to false once data is fetched
      } catch (err) {
        console.error("Error fetching yarns by yardage:", err); //Log error in console for debugging
        setError("Error fetching yarns by yardage"); //Set error message if the API call fails
        setLoading(false); //Ensure loading state is reset
      }
    };

    fetchYarns(); //Call the fetch function on component mount
  }, []); //Empty dependency array means this effect runs only once when the component mounts

  //Display loading message while fetching data
  if (loading) {
    return <div>Loading yarns...</div>;
  }

  //Display error message if an error occurs
  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div className="container">
      {/* Page header */}
      <header className="header">
        <h1 className="title">Yarns Sorted by Yardage/Skein</h1>
        <hr className="title-separator" />
      </header>

      {/* Yarn data table */}
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
          {/* Map through the yarn data and create table rows */}
          {yarns.length > 0 ? (
            yarns.map((yarn) => (
              <tr
                key={`${yarn.brand}-${yarn.fiberType}-${yarn.color}-${yarn.yardagePerSkein}-${yarn.numberOfSkeinsOwned}`}
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

export default LookAtDataYarnYardagePerSkein;
