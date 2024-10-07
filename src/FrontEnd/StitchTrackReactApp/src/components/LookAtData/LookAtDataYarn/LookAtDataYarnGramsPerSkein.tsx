import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

//Component to display a table of yarns sorted by grams per skein
const LookAtDataYarnGramsPerSkein: React.FC = () => {
  const navigate = useNavigate(); //React Router's navigation hook for handling route changes

  //State variables to store yarn data, loading status, and any errors encountered
  const [yarns, setYarns] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  //useEffect hook to fetch data from the API when the component mounts
  useEffect(() => {
    const fetchYarns = async () => {
      try {
        //API call to get yarns sorted by grams per skein
        const response = await axios.get(
          `${process.env.REACT_APP_API_URL}/Yarn/sorted-by-grams`
        );

        //Check for data format and update state accordingly
        if (Array.isArray(response.data)) {
          setYarns(response.data);
        } else if (response.data.$values) {
          setYarns(response.data.$values);
        } else {
          setError("Unexpected data format from API"); //Handle unexpected response structure
        }

        setLoading(false); //Set loading to false once data is fetched
      } catch (err) {
        console.error("Error fetching yarns by grams:", err);
        setError("Error fetching yarns by grams"); //Set error message if the API call fails
        setLoading(false);
      }
    };

    fetchYarns(); //Call the data fetching function
  }, []); //Empty dependency array ensures this effect runs only once when the component mounts

  //Conditional rendering: Show a loading message while fetching data
  if (loading) {
    return <div>Loading yarns...</div>;
  }

  //Conditional rendering: Show error message if there's an issue with the data fetching
  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div className="container">
      {/* Header section */}
      <header className="header">
        <h1 className="title">Yarns Sorted by Grams/Skein</h1>
        <hr className="title-separator" />
      </header>

      {/* Display the yarns data in a table */}
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
          {/* Map over the yarns data and create table rows for each item */}
          {yarns.length > 0 ? (
            yarns.map((yarn) => (
              <tr key={`${yarn.brand}-${yarn.fiberType}-${yarn.color}`}>
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

      {/* Button to navigate back to the previous page */}
      <button className="button" onClick={() => navigate("/look-at-data/yarn")}>
        Back
      </button>
    </div>
  );
};

export default LookAtDataYarnGramsPerSkein;
