import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const LookAtDataStuffingBrands: React.FC = () => {
  const navigate = useNavigate();
  const [brands, setBrands] = useState<string[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchBrands = async () => {
      try {
        const response = await axios.get(
          `${process.env.REACT_APP_API_URL}/Stuffing/brands`
        );

        //Check if the response data is an array
        if (Array.isArray(response.data)) {
          setBrands(response.data);
        } else if (response.data.$values) {
          setBrands(response.data.$values);
        } else {
          setError("Unexpected data format from API");
        }

        setLoading(false);
      } catch (err) {
        console.error("Error fetching stuffing brands:", err);
        setError("Error fetching stuffing brands");
        setLoading(false);
      }
    };

    fetchBrands();
  }, []);

  if (loading) return <div>Loading brands...</div>;
  if (error) return <div>{error}</div>;

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">Stuffing Brands</h1>
        <hr className="title-separator" />
      </header>
      <ul className="list">
        {brands.length > 0 ? (
          brands.map((brand) => (
            <li key={brand} className="item">
              {brand}
            </li>
          ))
        ) : (
          <li className="item">No brands available</li>
        )}
      </ul>
      <button
        className="button"
        onClick={() => navigate("/look-at-data/stuffing")}
      >
        Back
      </button>
    </div>
  );
};

export default LookAtDataStuffingBrands;
