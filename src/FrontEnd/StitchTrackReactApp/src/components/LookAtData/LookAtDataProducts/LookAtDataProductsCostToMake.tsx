import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

//Define the Product type for type-checking
type Product = {
  finishedProductID: number;
  name: string;
  timeToMake: string;
  totalCostToMake: number;
  salePrice: number;
  numberInStock: number;
};

const LookAtDataProductsCostToMake: React.FC = () => {
  const navigate = useNavigate(); //For navigation using React Router
  const [products, setProducts] = useState<Product[]>([]); //State to store products data
  const [loading, setLoading] = useState(true); //State to track loading status
  const [error, setError] = useState<string | null>(null); //State for storing error messages

  //Fetch the product data sorted by cost to make when the component mounts
  useEffect(() => {
    const fetchProducts = async () => {
      try {
        const response = await axios.get(
          `${process.env.REACT_APP_API_URL}/FinishedProduct/sorted-by-cost`
        );

        //Set the products based on the response format
        setProducts(
          Array.isArray(response.data)
            ? response.data
            : response.data.$values || [] //Fallback for unusual data formats
        );
        setLoading(false);
      } catch (err) {
        setError("Error fetching products sorted by cost to make");
        setLoading(false); //Ensure loading is stopped even if there's an error
      }
    };

    fetchProducts(); //Trigger data fetching
  }, []); //Empty dependency array ensures this runs once on component mount

  //Conditional rendering based on loading and error states
  if (loading) return <div>Loading products...</div>;
  if (error) return <div>{error}</div>;

  return (
    <div className="container">
      {/* Page Header */}
      <header className="header">
        <h1 className="title">Products by Cost to Make</h1>
        <hr className="title-separator" />
      </header>

      {/* Table to display product details */}
      <table className="table">
        <thead>
          <tr>
            <th>Product Name</th>
            <th>Time to Make</th>
            <th>Total Cost</th>
            <th>Sale Price</th>
            <th>Number in Stock</th>
          </tr>
        </thead>
        <tbody>
          {/* Check if products are available, else show a fallback message */}
          {products.length > 0 ? (
            products.map((product) => (
              <tr key={product.finishedProductID}>
                <td>{product.name}</td>
                <td>{product.timeToMake}</td>
                <td>{product.totalCostToMake}</td>
                <td>{product.salePrice}</td>
                <td>{product.numberInStock}</td>
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan={5}>No products available</td>
            </tr>
          )}
        </tbody>
      </table>

      {/* Back button to navigate to the main Finished Products page */}
      <button
        className="button"
        onClick={() => navigate("/look-at-data/finished-products")}
      >
        Back
      </button>
    </div>
  );
};

export default LookAtDataProductsCostToMake;
