using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Flights_TQS.Entities
{
    public class Flight
    {
        public virtual long Id { get; set; }
        public virtual long Airplane { get; set; }

        [Required(ErrorMessage = "Airport Departure is Required")]
        public virtual long AirportDeparture { get; set; }

        [Required(ErrorMessage = "Airport Arrive is Required")]
        public virtual long AirportArrive { get; set; }
        public virtual Nullable<DateTime> DatetimeDeparture { get; set; }
        public virtual Nullable<DateTime> DatetimeArrive { get; set; }
        public virtual Nullable<int> FlightLeg { get; set; }
        public virtual double Price { get; set; }
    }
}
