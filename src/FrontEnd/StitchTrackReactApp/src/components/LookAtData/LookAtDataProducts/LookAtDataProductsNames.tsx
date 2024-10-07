import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const LookAtDataProductNames: React.FC = () => {
  const navigate = useNavigate(); //React Router's navigation hook
  const [productNames, setProductNames] = useState<string[]>([]); //State to store product names
  const [loading, setLoading] = useState(true); //State to track loading status
  const [error, setError] = useState<string | null>(null); //State for error messages

  //Fetch product names from the API when the component mounts
  useEffect(() => {
    const fetchProductNames = async () => {
      try {
        const response = await axios.get(
          `${process.env.REACT_APP_API_URL}/FinishedProduct/names`
        );

        //Handle both array and non-array responses, adjusting state accordingly
        setProductNames(
          Array.isArray(response.data)
            ? response.data
            : response.data.$values || [] //Fallback for alternative API responses
        );
        setLoading(false); //Set loading to false after data is fetched
      } catch (err) {
        setError("Error fetching product names"); //Capture and store error message
        setLoading(false); //Stop the loading indicator on error
      }
    };

    fetchProductNames(); //Call the function to fetch product names
  }, []); //Empty dependency array ensures this runs only on initial mount

  //Conditional rendering based on loading and error states
  if (loading) return <div>Loading product names...</div>;
  if (error) return <div>{error}</div>;

  return (
    <div className="container">
      {/* Page Header */}
      <header className="header">
        <h1 className="title">Names of Products</h1>
        <hr className="title-separator" />
      </header>

      {/* Unordered list to display product names */}
      <ul className="list">
        {/* Display the product names or show a fallback message if no names are available */}
        {productNames.length > 0 ? (
          productNames.map((name) => (
            <li key={name} className="item">
              {/* Use the product name as the key since it's a unique identifier */}
              {name}
            </li>
          ))
        ) : (
          <li className="item">No product names available</li>
        )}
      </ul>

      {/* Back button to navigate to the Finished Products overview page */}
      <button
        className="button"
        onClick={() => navigate("/look-at-data/finished-products")}
      >
        Back
      </button>
    </div>
  );
};

export default LookAtDataProductNames;
