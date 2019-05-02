using Flights_TQS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flights_TQS.Interfaces;
using Flights_TQS.Entities;


namespace Flights_TQS.Services
{
    public class Search : BaseService, ISearch
    {

        public string Message { get; set; }

        public Search(IUnitOfWork unitOfWork) : base(unitOfWork) {

            Message = string.Empty;
        }


        public List<Airplane> listAirplanes()
        {
            return UnitOfWork.Airplanes.GetAll().ToList();
        }

        public List<Airport> listAirports()
        {
            return UnitOfWork.Airports.GetAll().ToList();
        }
        
        public List<Flight> listFlights()
        {
            return UnitOfWork.Flights.GetAll().ToList();
        }

        public List<Person> listPersons()
        {
            return UnitOfWork.Persons.GetAll().ToList();
        }

        public List<Reservation> listReservations()
        {
            return UnitOfWork.Reservations.GetAll().ToList();
        }

        public List<Seat> listSeats()
        {
            return UnitOfWork.Seats.GetAll().ToList();
        }

        public List<Ticket> listTickets()
        {
            return UnitOfWork.Tickets.GetAll().ToList();
        }

        public List<User> listUsers()
        {
            Message = "ERROR 404";
            return null;
           // return UnitOfWork.Users.GetAll().ToList();
        }

    }
}
