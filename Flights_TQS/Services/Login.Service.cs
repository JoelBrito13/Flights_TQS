using Flights_TQS.Entities;
using Flights_TQS.Interfaces;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flights_TQS.Services;
using Auth0.ManagementApi.Models;

namespace Flights_TQS.Services
{
    public class Login : BaseService, ILogin
    {
        public Login(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        private bool ValidateExist(FlightUser flightUser)
        {
            if (flightUser.Id == 0)
                try
                {
                    FlightUser flightUserValidate;

                    if (!String.IsNullOrEmpty(flightUser.Email))
                    {
                        flightUserValidate = UnitOfWork.FlightUsers.FirstOrDefault(u => (flightUser.Email == u.Email));
                        if (flightUserValidate != null) return true;
                    }

                    return false;
                }
                catch
                {
                    return false;
                }
            else
                try
                {
                    Read((int)flightUser.Id);
                    return true;
                }
                catch
                {
                    return false;
                }
        }

        private bool ValidateExist(FlightUserAdd flightUserAdd)
        {
            FlightUser flightUser = new FlightUser()
            {
                Id = 0,
                Email = flightUserAdd.Email,
            };

            return ValidateExist(flightUser);
        }

        private bool ValidateExist(int Id)
        {
            FlightUser flightUser = new FlightUser() { Id = Id };
            return ValidateExist(flightUser);
        }

        private FlightUser Read(IQueryOver<FlightUser, FlightUser> query, bool readAuth)
        {
            FlightUser flightUser = query.SingleOrDefault();

            if (flightUser == null)
                throw new ArgumentException("[Usuários] User not founded");
                
    
            if (readAuth && !String.IsNullOrEmpty(flightUser.Email))
                flightUser.Auth0 = Task.Run(() => new Management(UnitOfWork.AppServices).ReadByEmailAsync(flightUser.Email)).Result;

            return flightUser;
        }
        public FlightUser Read(int Id, bool readAuth = false)
        {
            if (Id <= 0)
                return null;

            return Read(UnitOfWork.FlightUsers.AsQueryOver().Where(u => u.Id == Id), readAuth);
        }
        public FlightUser ReadByEmail(string email, bool readAuth = false)
        {
            if (String.IsNullOrEmpty(email))
                return null;

            email = email.ToLower();
            return Read(UnitOfWork.FlightUsers.AsQueryOver().WhereRestrictionOn(u => u.Email).IsInsensitiveLike(email), readAuth);
        }
        public FlightUser Add(FlightUserAdd flightUserAdd, bool AddAuth0, string IdAuth0 = null)
        {
            if (!ValidateExist(flightUserAdd))
                try
                {
                    // Database
                    UnitOfWork.BeginTransaction();

                    // Usuário
                    FlightUser flightUser = new FlightUser
                    {
                        Id = 0,
                        BornDate = flightUserAdd.BornDate,
                        Email = flightUserAdd.Email,
                        FName = flightUserAdd.FName,
                        LName = flightUserAdd.LName,
                    };

                    UnitOfWork.FlightUsers.Add(flightUser);
                    UnitOfWork.SaveChanges();

                    // Auth0
                    var management = new Management(UnitOfWork.AppServices);

                    if (AddAuth0)
                    {
                        var FlightUserAuth0 = Task.Run(() => management.IncludeAsync((int)flightUser.Id, flightUser.Email, flightUserAdd.Password, flightUser.FName, flightUser.LName)).Result;

                        if (FlightUserAuth0 == null)
                            throw new InvalidOperationException("[Login] Some problem while add User in Auth0");
                    }
                    else
                    {
                        User FlightUserAuth0 = Task.Run(() => management.ReadAsync(IdAuth0)).Result;

                        if (FlightUserAuth0 == null)
                            throw new InvalidOperationException("[Usuários] Ocorreu um problema ao Read o usuário em Auth0");
                        else
                          if (!(FlightUserAuth0.AppMetadata as Dictionary<string, object>).ContainsKey("IdFlightUser") || (FlightUserAuth0.AppMetadata["IdFlightUser"] == null))
                            Task.Run(() => management.EditAppMetadataAsync(flightUser));
                    }


                    UnitOfWork.Commit();
                    return Read((int)flightUser.Id);
                }
                catch(Exception ex)
                {
                    UnitOfWork.Rollback();
                    throw ex;
                }
            else
                throw new ArgumentException("[Login] User Already exists");
        }
        public FlightUser Save(FlightUser flightUser)
        {
            if (flightUser.Id <= 0)
                throw new ArgumentException(String.Format("[Login] Invalid Id: {0}", flightUser.Id));

            if (ValidateExist(flightUser))
                try
                {
                    FlightUser actualFlightUser = Read((int)flightUser.Id);

                    if (flightUser.Email != actualFlightUser.Email)
                    {
                        // Auth0
                        var auth0User = Task.Run(() => new Management(UnitOfWork.AppServices).EditAsync(flightUser)).Result;
                        if (auth0User == null) throw new InvalidOperationException("[Usuários] Ocorreu um problema ao Add usuário em Auth0");

                        actualFlightUser.Email = flightUser.Email;
                    }

                    // Database
                    UnitOfWork.BeginTransaction();

                    // Usuário                        BornDate = flightUserAdd.BornDate,
                    actualFlightUser.FName = flightUser.FName;
                    actualFlightUser.LName = flightUser.LName;
                    actualFlightUser.BornDate= flightUser.BornDate;

                    UnitOfWork.FlightUsers.Update(actualFlightUser);

                    UnitOfWork.Commit();

                    return Read((int)flightUser.Id);
                }
                catch
                {
                    UnitOfWork.Rollback();
                    throw;
                }
            else
                throw new ArgumentException(String.Format("[Login] User not Founded: {0}", flightUser.Id));
        }
        public bool Delete(int Id)
        {
            if (ValidateExist(Id))
                try
                {
                    FlightUser flightUser = Read(Id);

                    // Auth0
                    Task.Run(() => new Management(UnitOfWork.AppServices).DeleteAsync(flightUser));

                    // Database

                    UnitOfWork.BeginTransaction();
                    UnitOfWork.FlightUsers.Update(flightUser);
                    UnitOfWork.Commit();

                    return true;
                }
                catch
                {
                    UnitOfWork.Rollback();
                    throw;
                }
            else
                throw new ArgumentException(String.Format("[Usuários] Usuário não encontrado: {0}", Id));
        }
        public FlightUser Verify(string IdAuth0)
        {
            Management management = new Management(UnitOfWork.AppServices);
            User FlightUserAuth0 = Task.Run(() => management.ReadAsync(IdAuth0)).Result;

            if (FlightUserAuth0 == null)
                return null;
            else
            {
                try
                {
                    FlightUser flightUser = ReadByEmail(FlightUserAuth0.Email);
                    Task.Run(() => management.EditAppMetadataAsync(flightUser));

                    return flightUser;
                }
                catch
                {
                    FlightUserAdd FlightUserAdd = new FlightUserAdd
                    {
                        Email = FlightUserAuth0.Email,
                        FName = FlightUserAuth0.FirstName ?? String.Empty,
                        LName = FlightUserAuth0.LastName ?? String.Empty,
                        Password = UtilsString.RandomPassword(20)
                    };

                    return Add(FlightUserAdd, false, IdAuth0);
                }
            }
        }
        public List<FlightUser> listUsers()
        {
            return UnitOfWork.FlightUsers.GetAll().ToList();
        }



    }
}
