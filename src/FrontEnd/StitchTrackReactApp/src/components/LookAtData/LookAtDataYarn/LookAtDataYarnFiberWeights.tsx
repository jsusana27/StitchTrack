import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const LookAtDataYarnFiberWeights: React.FC = () => {
  const navigate = useNavigate();

  const [yarnFiberWeights, setYarnFiberWeights] = useState<number[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchFiberWeights = async () => {
      try {
        const response = await axios.get(
          `${process.env.REACT_APP_API_URL}/Yarn/fiber-weights`
        );

        //Check if response data is an array or has $values
        if (Array.isArray(response.data)) {
          setYarnFiberWeights(response.data);
        } else if (response.data.$values) {
          setYarnFiberWeights(response.data.$values);
        } else {
          setError("Unexpected data format from API");
        }

        setLoading(false);
      } catch (err) {
        console.error("Error fetching yarn fiber weights:", err);
        setError("Error fetching yarn fiber weights");
        setLoading(false);
      }
    };

    fetchFiberWeights();
  }, []);

  if (loading) {
    return <div>Loading fiber weights...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">Fiber Weights</h1>
        <hr className="title-separator" />
      </header>

      <ul className="list">
        {yarnFiberWeights.length > 0 ? (
          yarnFiberWeights.map((weight) => (
            <li key={weight} className="item">
              {weight}
            </li>
          ))
        ) : (
          <li className="item">No fiber weights available</li>
        )}
      </ul>

      <button className="button" onClick={() => navigate("/look-at-data/yarn")}>
        Back
      </button>
    </div>
  );
};

export default LookAtDataYarnFiberWeights;
