using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights_TQS.Entities
{
    public class Seat
    {
        public virtual long Id { get; set; }
        public virtual int Row { get; set; }
        public virtual string Herder { get; set; }
        public virtual string Class { get; set; }
        public virtual long Airplane { get; set; }
    }
}
