using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights_TQS.Entities
{
    public class Flight
    {
        public virtual long Id { get; set; }
        public virtual long Airplane { get; set; }
        public virtual long AirportDeparture { get; set; }
        public virtual long AirportArrive { get; set; }
        public virtual Nullable<DateTime> DatetimeDeparture { get; set; }
        public virtual Nullable<DateTime> DatetimeArrive { get; set; }
        public virtual Nullable<int> FlightLeg { get; set; }
        public virtual double Price { get; set; }
    }
}
