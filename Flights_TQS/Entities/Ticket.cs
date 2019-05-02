using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights_TQS.Entities
{
    public class Ticket
    {
        public virtual string Id { get; set; }
        public virtual long Person { get; set; }
        public virtual long Seat { get; set; }
        public virtual long Flight { get; set; }
        public virtual double Price { get; set; }
        public virtual int Luggage { get; set; }
        public virtual string SpecialFood { get; set; }
        public virtual string SpecialTreatment { get; set; }
    }
}
