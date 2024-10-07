import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const LookAtDataYarnColors: React.FC = () => {
  const navigate = useNavigate();

  //State to store the colors fetched from the API
  const [yarnColors, setYarnColors] = useState<string[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  //Fetch colors from the API
  useEffect(() => {
    const fetchColors = async () => {
      try {
        const response = await axios.get(
          `${process.env.REACT_APP_API_URL}/Yarn/colors`
        );

        //Check if response.data is an array or has a $values key
        if (Array.isArray(response.data)) {
          setYarnColors(response.data);
        } else if (response.data.$values) {
          setYarnColors(response.data.$values);
        } else {
          setError("Unexpected data format from API");
        }
        setLoading(false); //Data fetched successfully
      } catch (err) {
        console.error("Error fetching yarn colors:", err);
        setError("Error fetching yarn colors");
        setLoading(false);
      }
    };

    fetchColors();
  }, []);

  //Display loading indicator or error message
  if (loading) {
    return <div>Loading colors...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div className="container">
      {/* Title Section */}
      <header className="header">
        <h1 className="title">Colors</h1>
        <hr className="title-separator" />
      </header>

      {/* Display the list of colors */}
      <ul className="list">
        {yarnColors.length > 0 ? (
          yarnColors.map((color) => (
            <li key={color} className="item">
              {color}
            </li>
          ))
        ) : (
          <li className="item">No colors available</li>
        )}
      </ul>

      {/* Back Button */}
      <button className="button" onClick={() => navigate("/look-at-data/yarn")}>
        Back
      </button>
    </div>
  );
};

export default LookAtDataYarnColors;
