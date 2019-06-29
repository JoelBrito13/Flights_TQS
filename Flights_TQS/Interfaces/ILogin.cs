using Flights_TQS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights_TQS.Interfaces
{
    public interface ILogin
    {
        FlightUser Read(int Id, bool readAuth = false);
        FlightUser ReadByEmail(string email, bool readAuth = false);
        FlightUser Add(FlightUserAdd flightUserAdd, bool AddAuth0, string IdAuth0 = null);
        FlightUser Save(FlightUser flightUser);
        bool Delete(int Id);
        FlightUser Verify(string IdAuth0);
        List<FlightUser> listUsers();
    }
}
