using Flights_TQS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights_TQS.Interfaces
{
    interface IDelieverFlight
    {
        Airplane InsertAirplanes(AirplaneToAdd airplaneInsert);
        Flight InsertFlights(EssencialFlight flight);
    }
}
