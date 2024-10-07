import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const LookAtDataYarnBrands: React.FC = () => {
  const navigate = useNavigate();

  //State to store the brands fetched from the API
  const [yarnBrands, setYarnBrands] = useState<string[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  //Fetch brands from the API
  useEffect(() => {
    const fetchBrands = async () => {
      try {
        const response = await axios.get(
          `${process.env.REACT_APP_API_URL}/Yarn/brands`
        );

        //Check if response.data is an array
        if (Array.isArray(response.data)) {
          setYarnBrands(response.data); //Assuming the API returns an array of brands
        } else if (response.data.$values) {
          //If it's an object with "$values", handle it accordingly
          setYarnBrands(response.data.$values);
        } else {
          setError("Unexpected data format from API");
        }

        setLoading(false); //Data fetched successfully
      } catch (err) {
        console.error("Error fetching yarn brands:", err);
        setError("Error fetching yarn brands");
        setLoading(false);
      }
    };

    fetchBrands();
  }, []);

  //Display loading indicator or error message
  if (loading) {
    return <div>Loading brands...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div className="container">
      {/* Title Section */}
      <header className="header">
        <h1 className="title">Brands</h1>
        <hr className="title-separator" />
      </header>

      {/* Display the list of brands */}
      <ul className="list">
        {yarnBrands.length > 0 ? (
          yarnBrands.map((brand) => (
            <li key={brand} className="item">
              {brand}
            </li>
          ))
        ) : (
          <li className="item">No brands available</li>
        )}
      </ul>

      {/* Back Button */}
      <button className="button" onClick={() => navigate("/look-at-data/yarn")}>
        Back
      </button>
    </div>
  );
};

export default LookAtDataYarnBrands;
