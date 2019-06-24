using Flights_TQS.Entities;
using Flights_TQS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights_TQS.Services
{
    public class Login : BaseService, ILogin
    {
        public string Message { get; set; }
        public Login(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            Message = string.Empty;
        }
        public FlightUser authenticate(AuthenticateFlightUser authenticateUser)
        {
            try
            {
                FlightUser user = UnitOfWork.Users
                    .GetAll(u => u.Email == authenticateUser.Email && u.Password == authenticateUser.Password)
                    .FirstOrDefault();
                if (user != null)
                {
                    user.Password = null;
                    return user;
                }
            }
            catch
            {

            }
            Message = "Authenticate ";
            return null;
        }
        public FlightUser addUser(FlightUser userToAdd)
        {
            if (this.validEmail(userToAdd.Email))
            {
                try
                {
                    // Database
                    UnitOfWork.BeginTransaction();
                    UnitOfWork.Users.Add(userToAdd);
                    UnitOfWork.SaveChanges();
                    UnitOfWork.Commit();

                    userToAdd.Password = null;
                    return userToAdd;
                }               
                catch
                {
                    UnitOfWork.Rollback();
                    throw;
                }
            }
            Message = userToAdd.Email + ": already registered";
            return null;
        }
        
        private bool validEmail(string email)
        {
            try
            {
                FlightUser user = UnitOfWork.Users
                    .GetAll(u => u.Email == email)
                    .FirstOrDefault();

                return (user == null);
            }
            catch
            {
                return false;
            }
        }
    }
}
