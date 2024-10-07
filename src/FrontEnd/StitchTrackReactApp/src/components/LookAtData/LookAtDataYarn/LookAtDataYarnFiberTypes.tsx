import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const LookAtDataYarnFiberTypes: React.FC = () => {
  const navigate = useNavigate();

  const [yarnFiberTypes, setYarnFiberTypes] = useState<string[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchFiberTypes = async () => {
      try {
        const response = await axios.get(
          `${process.env.REACT_APP_API_URL}/Yarn/fiber-types`
        );

        //Check if response.data is an array or has a $values key
        if (Array.isArray(response.data)) {
          setYarnFiberTypes(response.data);
        } else if (response.data.$values) {
          setYarnFiberTypes(response.data.$values);
        } else {
          setError("Unexpected data format from API");
        }
        setLoading(false); //Data fetched successfully
      } catch (err) {
        console.error("Error fetching yarn fiber types:", err);
        setError("Error fetching yarn fiber types");
        setLoading(false);
      }
    };

    fetchFiberTypes();
  }, []);

  //Display loading indicator or error message
  if (loading) {
    return <div>Loading fiber types...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">Fiber Types</h1>
        <hr className="title-separator" />
      </header>

      {/* Display the list of fiber types */}
      <ul className="list">
        {yarnFiberTypes.length > 0 ? (
          yarnFiberTypes.map((fiberType) => (
            <li key={fiberType} className="item">
              {fiberType}
            </li>
          ))
        ) : (
          <li className="item">No fiber types available</li>
        )}
      </ul>

      {/* Back Button */}
      <button className="button" onClick={() => navigate("/look-at-data/yarn")}>
        Back
      </button>
    </div>
  );
};

export default LookAtDataYarnFiberTypes;
