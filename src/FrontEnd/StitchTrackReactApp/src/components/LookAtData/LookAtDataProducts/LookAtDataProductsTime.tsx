import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

type Product = {
  productID?: number; //Optional, if present
  name: string;
  timeToMake: string;
  totalCostToMake: string;
  salePrice: string;
  numberInStock: number;
};

const LookAtDataProductsTime: React.FC = () => {
  const navigate = useNavigate();
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchProducts = async () => {
      try {
        const response = await axios.get(
          `${process.env.REACT_APP_API_URL}/FinishedProduct/sorted-by-time`
        );
        setProducts(
          Array.isArray(response.data)
            ? response.data
            : response.data.$values || []
        );
        setLoading(false);
      } catch (err) {
        setError("Error fetching products sorted by time");
        setLoading(false);
      }
    };
    fetchProducts();
  }, []);

  if (loading) return <div>Loading products...</div>;
  if (error) return <div>{error}</div>;

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">Products by Time to Make</h1>
        <hr className="title-separator" />
      </header>
      <table className="table">
        <thead>
          <tr>
            <th>Product Name</th>
            <th>Time to Make (Hours)</th>
            <th>Total Cost</th>
            <th>Sale Price</th>
            <th>Stock</th>
          </tr>
        </thead>
        <tbody>
          {products.length > 0 ? (
            products.map((product) => (
              <tr key={product.productID || product.name}>
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
      <button
        className="button"
        onClick={() => navigate("/look-at-data/finished-products")}
      >
        Back
      </button>
    </div>
  );
};

export default LookAtDataProductsTime;
