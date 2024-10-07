import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const LookAtDataSafetyEyesColors: React.FC = () => {
  const navigate = useNavigate();
  const [colors, setColors] = useState<string[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchColors = async () => {
      try {
        const response = await axios.get(
          `${process.env.REACT_APP_API_URL}/SafetyEye/colors`
        );

        //Check if response data is an array or has $values
        if (Array.isArray(response.data)) {
          setColors(response.data);
        } else if (response.data.$values) {
          setColors(response.data.$values);
        } else {
          setError("Unexpected data format from API");
        }

        setLoading(false);
      } catch (err) {
        console.error("Error fetching safety eye colors:", err);
        setError("Error fetching safety eye colors");
        setLoading(false);
      }
    };

    fetchColors();
  }, []);

  if (loading) return <div>Loading colors...</div>;
  if (error) return <div>{error}</div>;

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">Colors</h1>
        <hr className="title-separator" />
      </header>
      <ul className="list">
        {colors.length > 0 ? (
          colors.map((color) => (
            <li key={color} className="item">
              {color}
            </li>
          ))
        ) : (
          <li className="item">No colors available</li>
        )}
      </ul>
      <button
        className="button"
        onClick={() => navigate("/look-at-data/safety-eyes")}
      >
        Back
      </button>
    </div>
  );
};

export default LookAtDataSafetyEyesColors;
