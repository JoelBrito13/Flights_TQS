using Flights_TQS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights_TQS.Interfaces
{
    public interface ISearch
    {
        string Message { get; set; }
        List<Airplane> listAirplanes();
        List<Airport> listAirports();
        List<Flight> listFlights();
        List<Person> listPersons();
        List<Reservation> listReservations();
        List<Seat> listSeats();
        List<Ticket> listTickets();
        List<User> listUsers();
    }
}
