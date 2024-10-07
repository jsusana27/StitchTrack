import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import axios from "axios";

const LookAtDataSaleStatsOutput: React.FC = () => {
  const { productId: productName } = useParams<{ productId: string }>(); //productId is treated as productName here
  const [totalQuantity, setTotalQuantity] = useState<number | null>(null);
  const [totalRevenue, setTotalRevenue] = useState<number | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchSalesStats = async () => {
      setLoading(true); //Set loading state while data is being fetched
      setError(null); //Reset error state before a new request

      try {
        //Fetch sales stats using productName in the query parameter
        const statsResponse = await axios.get(
          `${process.env.REACT_APP_API_URL}/orderproduct/sale-stats?productName=${productName}`
        );

        if (statsResponse.data) {
          setTotalQuantity(statsResponse.data.totalQuantity);
          setTotalRevenue(statsResponse.data.totalRevenue);
        } else {
          setError("No sales stats found for this product.");
        }
      } catch (err) {
        setError("Error fetching sales stats. Please try again.");
      } finally {
        setLoading(false); //Stop loading once data has been fetched or an error has occurred
      }
    };

    if (productName) {
      fetchSalesStats();
    }
  }, [productName]);

  //Display a loading message while the data is being fetched
  if (loading) {
    return <div>Loading sales stats...</div>;
  }

  //Display an error message if there was an error fetching the data
  if (error) {
    return <div className="error-message">{error}</div>;
  }

  return (
    <div className="container">
      <header className="header">
        <h1 className="title">Sale Stats for {productName}</h1>
        <hr className="title-separator" />
      </header>

      {/* Display the sales stats in a table format */}
      <div className="container">
        <table className="table">
          <thead>
            <tr>
              <th>Stat</th>
              <th>Value</th>
            </tr>
          </thead>
          <tbody>
            <tr>
              <td>Total Quantity Sold</td>
              <td>{totalQuantity !== null ? totalQuantity : "N/A"}</td>
            </tr>
            <tr>
              <td>Total Revenue</td>
              <td>
                $
                {totalRevenue !== null && !isNaN(totalRevenue)
                  ? totalRevenue.toFixed(2)
                  : "N/A"}
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <div className="button-group">
        <button className="button" onClick={() => navigate("/look-at-data")}>
          Back
        </button>
      </div>
    </div>
  );
};

export default LookAtDataSaleStatsOutput;
