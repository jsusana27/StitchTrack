import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const LookAtDataSafetyEyesSizes: React.FC = () => {
  const navigate = useNavigate(); //React Router's hook for navigation
  const [sizes, setSizes] = useState<number[]>([]); //State to store the list of sizes
  const [loading, setLoading] = useState(true); //Loading state to indicate data is being fetched
  const [error, setError] = useState<string | null>(null); //State to handle and display any errors

  //useEffect hook to fetch safety eye sizes when the component mounts
  useEffect(() => {
    const fetchSizes = async () => {
      try {
        //API call to retrieve safety eye sizes
        const response = await axios.get(
          `${process.env.REACT_APP_API_URL}/SafetyEye/sizes`
        );

        //Handle the different response formats that might be returned
        if (Array.isArray(response.data)) {
          setSizes(response.data);
        } else if (response.data.$values) {
          setSizes(response.data.$values);
        } else {
          setError("Unexpected data format from API");
        }

        setLoading(false); //Data fetching is complete
      } catch (err) {
        console.error("Error fetching safety eye sizes:", err);
        setError("Error fetching safety eye sizes"); //Set the error message
        setLoading(false); //Stop loading indicator
      }
    };

    fetchSizes(); //Trigger the API call on component mount
  }, []); //Empty dependency array ensures this runs only once

  //Conditional rendering for loading and error states
  if (loading) return <div>Loading sizes...</div>;
  if (error) return <div>{error}</div>;

  return (
    <div className="container">
      {/* Page header */}
      <header className="header">
        <h1 className="title">Sizes</h1>
        <hr className="title-separator" />
      </header>

      {/* Display the list of sizes or a message if no sizes are available */}
      <ul className="list">
        {sizes.length > 0 ? (
          sizes.map((size) => (
            <li key={size} className="item">
              {size} mm {/* Show size with "mm" suffix */}
            </li>
          ))
        ) : (
          <li className="item">No sizes available</li>
        )}
      </ul>

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

export default LookAtDataSafetyEyesSizes;
