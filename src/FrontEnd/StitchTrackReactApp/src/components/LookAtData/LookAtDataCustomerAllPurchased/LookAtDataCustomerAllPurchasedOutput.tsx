import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import axios from "axios";

type Purchase = {
  finishedProduct: {
    name: string;
  };
};

const LookAtDataCustomerAllPurchasedOutput: React.FC = () => {
  const { customerId } = useParams<{ customerId: string }>();
  const [uniqueProducts, setUniqueProducts] = useState<string[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchPurchases = async () => {
      try {
        const response = await axios.get(
          `${process.env.REACT_APP_API_URL}/customerpurchase/customer/${customerId}`
        );

        if (response.data) {
          //Create a Set to store unique product names
          const uniqueProductNames = new Set<string>(
            response.data.map(
              (purchase: Purchase) => purchase.finishedProduct.name
            )
          );
          setUniqueProducts(Array.from(uniqueProductNames));
        } else {
          setError("No purchases found for this customer");
        }
      } catch (error) {
        console.error("Error fetching customer purchases", error);
        setError("Error fetching customer purchases");
      } finally {
        setLoading(false);
      }
    };

    fetchPurchases();
  }, [customerId]);

  if (loading) {
    return <div>Loading purchases...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">Customer Purchases</h1>
        <hr className="title-separator" />
      </header>

      <table className="table">
        <thead>
          <tr>
            <th>Product Name</th>
          </tr>
        </thead>
        <tbody>
          {uniqueProducts.map((productName) => (
            <tr key={productName}>
              <td>{productName}</td>
            </tr>
          ))}
        </tbody>
      </table>

      <button className="button" onClick={() => navigate("/look-at-data")}>
        Back
      </button>
    </div>
  );
};

export default LookAtDataCustomerAllPurchasedOutput;
