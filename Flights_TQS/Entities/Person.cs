using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights_TQS.Entities
{
    public class Person
    {
        public virtual long Id { get; set; }
        public virtual string FName { get; set; }
        public virtual string LName { get;  set; }
        public virtual string BornDate { get; set; }
        public virtual string Passport { get; set; }
        public virtual string CitizenCard { get; set; }
        public virtual Nullable<DateTime> PassportValid { get; set; }
        public virtual Nullable<DateTime> CitizenCardValid { get; set; }
        public virtual string GetFullName()
        { return FName + LName; }
    }
}
