import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const LookAtDataAllCustomers: React.FC = () => {
  const navigate = useNavigate();

  //State to store the customers fetched from the API
  const [customers, setCustomers] = useState<
    {
      id: number;
      name: string;
      phoneNumber: string | null;
      emailAddress: string | null;
    }[]
  >([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  //Fetch customers from the API
  useEffect(() => {
    const fetchCustomers = async () => {
      try {
        const response = await axios.get(
          `${process.env.REACT_APP_API_URL}/customer/all`
        );

        //Check if response.data is an array
        if (Array.isArray(response.data)) {
          setCustomers(response.data); //Assuming the API returns an array of customers
        } else if (response.data.$values) {
          //If it's an object with "$values", handle it accordingly
          setCustomers(response.data.$values);
        } else {
          setError("Unexpected data format from API");
        }

        setLoading(false); //Data fetched successfully
      } catch (err) {
        console.error("Error fetching customers:", err);
        setError("Error fetching customers");
        setLoading(false);
      }
    };

    fetchCustomers();
  }, []);

  //Display loading indicator or error message
  if (loading) {
    return <div>Loading customers...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div className="container">
      {/* Title Section */}
      <header className="header">
        <h1 className="title">All Customers</h1>
        <hr className="title-separator" />
      </header>

      {/* Display the list of customers */}
      <table className="table">
        <thead>
          <tr>
            <th>Name</th>
            <th>Phone Number</th>
            <th>Email Address</th>
          </tr>
        </thead>
        <tbody>
          {customers.length > 0 ? (
            customers.map((customer) => (
              <tr key={customer.id}>
                <td>{customer.name}</td>
                <td>{customer.phoneNumber || "N/A"}</td>
                <td>{customer.emailAddress || "N/A"}</td>
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan={3}>No customers available</td>
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

export default LookAtDataAllCustomers;
