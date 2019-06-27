using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights_TQS.Entities
{
    public class Airport 
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string City { get; set; }
        public virtual string Country { get; set; }

    }
}
