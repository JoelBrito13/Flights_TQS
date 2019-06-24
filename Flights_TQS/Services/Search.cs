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
        public class RecvStr
        {
            public string filtername { get; set; }
        }

        public Search(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            Message = string.Empty;
        }
        public List<Airport> listAirports(String filter = null)
        {
            if (filter == null) return UnitOfWork.Airports.GetAll().ToList();
            return UnitOfWork.Airports.AsQueryable().Where(u => u.City == filter
                || u.Country == filter).ToList();
        }
        public List<Flight> listFlights(Flight flight)
        {
            return UnitOfWork.Flights.AsQueryable().Where(u => u.AirportDeparture == flight.AirportDeparture
                && u.AirportArrive == flight.AirportArrive).ToList();
        }
    }
}

