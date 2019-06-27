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
        List<Airport> listAirports(String filter = null);
        List<Flight> listFlights(Flight flight, int pageFlight = 0);
    }
}
