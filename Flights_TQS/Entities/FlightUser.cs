using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


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

        [Required(ErrorMessage = "Password is Required")]
        public virtual string Password { get; set; }

        public virtual string GetFullName()
        { return FName + LName; }         
    }

    public class AuthenticateFlightUser 
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
        [Required(ErrorMessage = "Email is Required")]
        [CustomValidation(typeof(AuthenticateFlightUser), "ValidateEmail")]
        public virtual string Email { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        public virtual string Password { get; set; }
    }
  
}


//        public static ValidationResult IdxMaiorOuIgualAZero(int idx)
//{
//    return ((idx >= 0) ? ValidationResult.Success : new ValidationResult("Índice inválido"));
//}

//#region // Atributos //
//[Required(ErrorMessage = "O Id field is Required")]
//[CustomValidation(typeof(Base), "IdxMaiorOuIgualAZero")]
//public virtual int Id { get; set; }





