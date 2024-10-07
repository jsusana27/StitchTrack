import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

//Define the types for Order and Customer
interface Order {
  orderID: number;
  customer: { name: string };
  orderDate: string;
  formOfPayment: string;
  totalPrice: number;
}

const LookAtDataAllOrders: React.FC = () => {
  const [orders, setOrders] = useState<Order[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  //Fetch orders from the API
  useEffect(() => {
    const fetchOrders = async () => {
      try {
        const response = await axios.get(
          `${process.env.REACT_APP_API_URL}/Order`
        );

        //Check if response.data is an array and set the orders state
        if (Array.isArray(response.data)) {
          setOrders(response.data);
        } else {
          setError("Unexpected data format from API");
        }
        setLoading(false);
      } catch (err) {
        console.error("Error fetching orders:", err);
        setError("Error fetching orders");
        setLoading(false);
      }
    };

    fetchOrders();
  }, []);

  //Display loading indicator or error message
  if (loading) {
    return <div>Loading orders...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div className="container">
      {/* Title Section */}
      <header className="header">
        <h1 className="title">All Orders</h1>
        <hr className="title-separator" />
      </header>

      {/* Table to display order information */}
      <table className="table">
        <thead>
          <tr>
            <th>Name</th>
            <th>Order Date</th>
            <th>Form of Payment</th>
            <th>Total Price</th>
          </tr>
        </thead>
        <tbody>
          {orders.length > 0 ? (
            orders.map((order) => (
              <tr key={order.orderID}>
                <td>{order.customer.name}</td>
                <td>{new Date(order.orderDate).toLocaleString()}</td>
                <td>{order.formOfPayment}</td>
                <td>{order.totalPrice.toFixed(2)}</td>
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan={4}>No orders available</td>
            </tr>
          )}
        </tbody>
      </table>

      {/* Back Button */}
      <button className="button" onClick={() => navigate("/look-at-data")}>
        Back
      </button>
    </div>
  );
};

export default LookAtDataAllOrders;
