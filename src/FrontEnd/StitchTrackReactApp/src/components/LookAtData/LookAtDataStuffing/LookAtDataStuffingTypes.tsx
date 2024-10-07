import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const LookAtDataStuffingTypes: React.FC = () => {
  const navigate = useNavigate(); //React Router's hook for navigation
  const [types, setTypes] = useState<string[]>([]); //State to store the list of stuffing types
  const [loading, setLoading] = useState(true); //State to indicate loading status
  const [error, setError] = useState<string | null>(null); //State to track and display any errors

  //useEffect hook to fetch stuffing types from the API when the component mounts
  useEffect(() => {
    const fetchTypes = async () => {
      try {
        //API call to retrieve stuffing types
        const response = await axios.get(
          `${process.env.REACT_APP_API_URL}/Stuffing/types`
        );

        //Check if the response data is an array or in a different format and set the state accordingly
        if (Array.isArray(response.data)) {
          setTypes(response.data);
        } else if (response.data.$values) {
          setTypes(response.data.$values);
        } else {
          setError("Unexpected data format from API"); //Set an error if the format is unexpected
        }

        setLoading(false); //Data fetching is complete
      } catch (err) {
        console.error("Error fetching stuffing types:", err);
        setError("Error fetching stuffing types"); //Set the error message
        setLoading(false); //Stop the loading indicator
      }
    };

    fetchTypes(); //Trigger the API call on component mount
  }, []); //Empty dependency array ensures this runs only once when the component is first rendered

  //Conditional rendering for loading and error states
  if (loading) return <div>Loading types...</div>;
  if (error) return <div>{error}</div>;

  return (
    <div className="container">
      {/* Header section */}
      <header className="header">
        <h1 className="title">Stuffing Types</h1>
        <hr className="title-separator" />
      </header>

      {/* Display the list of stuffing types */}
      <ul className="list">
        {/* Conditional rendering to show the list or a message if no types are available */}
        {types.length > 0 ? (
          types.map((type) => (
            <li key={type} className="item">
              {type}
            </li>
          ))
        ) : (
          <li className="item">No types available</li>
        )}
      </ul>

      {/* Back button to navigate to the main stuffing data page */}
      <button
        className="button"
        onClick={() => navigate("/look-at-data/stuffing")}
      >
        Back
      </button>
    </div>
  );
};

export default LookAtDataStuffingTypes;
