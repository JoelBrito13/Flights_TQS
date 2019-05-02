using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights_TQS.Entities
{
    public class Airplane
    {
        public virtual long Id { get; set; }
        public virtual string Model { get; set; }
        public virtual string Company { get; set; }
        public virtual int NumSeats { get; set; }
    }
}
