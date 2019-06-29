using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Auth0.ManagementApi.Models;

namespace Flights_TQS.Entities
{
    public class FlightUser
    {
        public static ValidationResult ValidateEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return ((addr.Address == email) ? ValidationResult.Success : new ValidationResult("Invalid Email"));
            }
            catch
            {
                return new ValidationResult("Invalid Email");
            }
        }
        public virtual long Id { get; set; }

        [Required(ErrorMessage = "First Name is Required")]
        public virtual string FName { get; set; }

        [Required(ErrorMessage = "Last Name is Required")]
        public virtual string LName { get; set; }

        public virtual Nullable<DateTime> BornDate { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [CustomValidation(typeof(FlightUser), "ValidateEmail")]
        public virtual string Email { get; set; }
        [NotMapped]
        public virtual User Auth0 { get; set; }

        public virtual string GetFullName()
        { return FName + LName; }
    }

    public class FlightUserAdd : FlightUser
    {
        [Required(ErrorMessage = "Password is Required")]
        public virtual string Password { get; set; }
    }
}





