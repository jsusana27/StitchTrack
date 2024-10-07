import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const LookAtDataStuffingPrices: React.FC = () => {
  const navigate = useNavigate(); //Hook for programmatic navigation
  const [stuffing, setStuffing] = useState<any[]>([]); //State to store the list of stuffing items
  const [loading, setLoading] = useState(true); //State to track the loading status
  const [error, setError] = useState<string | null>(null); //State to handle and display any errors

  //useEffect hook to fetch stuffing data from the API when the component mounts
  useEffect(() => {
    const fetchStuffing = async () => {
      try {
        //API call to get stuffing data sorted by price
        const response = await axios.get(
          `${process.env.REACT_APP_API_URL}/Stuffing/sorted-by-price`
        );

        //Handle different response formats and set the state accordingly
        if (Array.isArray(response.data)) {
          setStuffing(response.data);
        } else if (response.data.$values) {
          setStuffing(response.data.$values);
        } else {
          setError("Unexpected data format from API");
        }

        setLoading(false); //Data fetching is complete
      } catch (err) {
        console.error("Error fetching stuffing sorted by price:", err);
        setError("Error fetching stuffing sorted by price"); //Set the error message
        setLoading(false); //Stop loading indicator
      }
    };

    fetchStuffing(); //Trigger the API call on component mount
  }, []); //Empty dependency array ensures this runs only once

  //Conditional rendering to display loading and error states
  if (loading) return <div>Loading stuffing data...</div>;
  if (error) return <div>{error}</div>;

  return (
    <div className="container">
      {/* Page header */}
      <header className="header">
        <h1 className="title">Stuffing Sorted by Price per 5 lbs</h1>
        <hr className="title-separator" />
      </header>

      {/* Table to display the stuffing data */}
      <table className="table">
        <thead>
          <tr>
            <th>Brand</th>
            <th>Type</th>
            <th>Price per 5 lbs</th>
          </tr>
        </thead>
        <tbody>
          {/* Render the stuffing list or show a message if no data is available */}
          {stuffing.length > 0 ? (
            stuffing.map((item) => (
              <tr key={`${item.brand}-${item.type}`}>
                <td>{item.brand}</td>
                <td>{item.type}</td>
                <td>{item.pricePerFivelbs}</td>
              </tr>
            ))
          ) : (
            <tr>
              {/* Display this message if the stuffing list is empty */}
              <td colSpan={3}>No stuffing available</td>
            </tr>
          )}
        </tbody>
      </table>

      {/* Back button to navigate back to the main stuffing data page */}
      <button
        className="button"
        onClick={() => navigate("/look-at-data/stuffing")}
      >
        Back
      </button>
    </div>
  );
};

export default LookAtDataStuffingPrices;
