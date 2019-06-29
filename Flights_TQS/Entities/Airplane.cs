using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public virtual bool Equals(Airplane airplane)
        {
            return this.Company == airplane.Company 
                && this.Model == airplane.Model 
                && this.NumSeats == airplane.NumSeats;
        }
    }
    public class AirplaneToAdd
    {
        public virtual long Id { get; set; }

        [Required(ErrorMessage = "Airplane model is Required")]
        public virtual string Model { get; set; }

        [Required(ErrorMessage = "Company Name is Required")]
        public virtual string Company { get; set; }

        [Required(ErrorMessage = "Number of Seats is Required")]
        public virtual int NumSeats { get; set; }

        public virtual int numSeatsFirstClass { get; set; }
        public virtual int numSeatsBusiness { get; set; }

        [Required(ErrorMessage = "Number of Economic Seats is Required")]
        public virtual int numSeatsEconomic { get; set; }

        [Required(ErrorMessage = "Number Seats per Header is Required")]
        public virtual int numSeatsPerHeader { get; set; }
    }
}
