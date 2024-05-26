import { useEffect, useState } from 'react';
import AdminNavbar from '../AdminNavbar/AdminNavbar';
import axios from 'axios';
import './BusDetails.css';
import { Link, useNavigate, useParams } from 'react-router-dom';
import BusOperatorNavbar from '../BusOperatorNavbar/BusOperatorNavbar';

function BusDetails() {
  const [busDetails, setBusDetails] = useState([]);
  const token = sessionStorage.getItem('authToken');
  const navigate = useNavigate();
  const { userId } = useParams();
  const role = sessionStorage.getItem('role');

  useEffect(() => {
    const fetchBusDetails = async () => {
      if (!(token && (role === 'Bus Operator' || role === 'Admin'))) {
        navigate('/login');
        return;
      }
      try {
        const response = await axios.get(
          `https://localhost:7114/api/Buses/getBusByOperatorId?busOperatorId=${userId}`,
          {
            headers: {
              Authorization: `Bearer ${token}`
            }
          }
        );

        setBusDetails(response.data);
      } catch (error) {
        if (error.response && error.response.status === 403) {
          window.alert('Unauthorized');
          navigate('/login');
        }
        console.error('Error fetching bus details:', error.message);
      }
    };

    fetchBusDetails();
  }, [token, role, navigate, userId]);

  const handleDeleteBus = async (busId) => {
    const confirmed = window.confirm('Are you sure you want to delete this bus?');

    if (!confirmed) {
      return;
    }
    try {
      await axios.delete(
        `https://localhost:7114/api/Buses/${busId}`,
        {
          headers: {
            Authorization: `Bearer ${token}`
          }
        }
      );
      window.alert('Bus deleted successfully.');
      setBusDetails(busDetails.filter(bus => bus.busId !== busId)); // Update state to remove the deleted bus
    } catch (error) {
      if (error.response && error.response.status === 403) {
        window.alert('Unauthorized');
      }
      console.error('Error deleting bus:', error.message);
    }
  };

  return (
    <div>
      {role === 'Admin' ? <AdminNavbar /> : <BusOperatorNavbar />}
      <table className="table table-dark">
        <thead>
          <tr>
            <th>Bus Name</th>
            <th>Bus Number</th>
            <th>Bus Type</th>
            <th>Total Seats</th>
            <th>Origin</th>
            <th>Destination</th>
            <th>Start Time</th>
            <th>End Time</th>
            <th>Fare</th>
            <th>Departure Dates</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {busDetails.map((bus) => (
            <tr key={bus.busId}>
              <td>{bus.busName}</td>
              <td>{bus.busNumber}</td>
              <td>{bus.busType}</td>
              <td>{bus.noOfSeats}</td>
              <td>{bus.origin}</td>
              <td>{bus.destination}</td>
              <td>{bus.startTime}</td>
              <td>{bus.endTime}</td>
              <td>{bus.fare}</td>
              <td>
                <ul>
                  {bus.busDepartures && bus.busDepartures.map((departure, index) => (
                    <li key={index}>{new Date(departure.departureDate).toLocaleDateString()}</li>
                  ))}
                </ul>
              </td>
              <td>
                <button
                  type="button"
                  className="btn btn-danger btn-sm"
                  onClick={() => handleDeleteBus(bus.busId)}
                >
                  X
                </button>
                <Link to={`/booking-details/${bus.busId}`} className="btn btn-link">
                  Booking Details
                </Link>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default BusDetails;
