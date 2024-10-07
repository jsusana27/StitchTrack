import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const LookAtDataSafetyEyesShapes: React.FC = () => {
  const navigate = useNavigate(); //React Router's hook for navigation
  const [shapes, setShapes] = useState<string[]>([]); //Holds the list of safety eye shapes
  const [loading, setLoading] = useState(true); //Tracks if data is being fetched
  const [error, setError] = useState<string | null>(null); //Stores error messages

  //Fetch the list of safety eye shapes on component mount
  useEffect(() => {
    const fetchShapes = async () => {
      try {
        const response = await axios.get(
          `${process.env.REACT_APP_API_URL}/SafetyEye/shapes`
        );

        //Check the response format and update state accordingly
        if (Array.isArray(response.data)) {
          setShapes(response.data);
        } else if (response.data.$values) {
          setShapes(response.data.$values);
        } else {
          setError("Unexpected data format from API");
        }

        setLoading(false); //Stop loading once data is fetched
      } catch (err) {
        console.error("Error fetching safety eye shapes:", err);
        setError("Error fetching safety eye shapes"); //Set a user-facing error message
        setLoading(false);
      }
    };

    fetchShapes(); //Call the fetch function on component mount
  }, []); //Empty dependency array means this effect runs only once

  //Show loading indicator while data is being fetched
  if (loading) return <div>Loading shapes...</div>;
  //Show error message if there's an issue fetching data
  if (error) return <div>{error}</div>;

  return (
    <div className="container">
      {/* Page header */}
      <header className="header">
        <h1 className="title">Shapes</h1>
        <hr className="title-separator" />
      </header>

      {/* List of shapes */}
      <ul className="list">
        {shapes.length > 0 ? (
          shapes.map((shape) => (
            <li key={shape} className="item">
              {shape}
            </li>
          ))
        ) : (
          <li className="item">No shapes available</li>
        )}
      </ul>

      {/* Back button to navigate to safety eyes overview */}
      <button
        className="button"
        onClick={() => navigate("/look-at-data/safety-eyes")}
      >
        Back
      </button>
    </div>
  );
};

export default LookAtDataSafetyEyesShapes;
