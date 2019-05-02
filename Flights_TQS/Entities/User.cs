using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights_TQS.Entities
{
    public class User
    {
        public virtual long Id { get; set; }
        public virtual string FName { get; set; }
        public virtual string LName { get; set; }
        public virtual DateTime BornDate { get; set; }
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }

        public virtual string GetFullName()
        { return FName + LName; }         
        //public virtual string FullName => FName + LName;
    }
}
