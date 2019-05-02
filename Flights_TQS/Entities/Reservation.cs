using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights_TQS.Entities
{
    public class Reservation
    {
        public virtual long Id { get; set; }
        public virtual long User { get; set; }
        public virtual string Ticket { get; set; }
        public virtual DateTime Datetime { get; set; }
    }
}
