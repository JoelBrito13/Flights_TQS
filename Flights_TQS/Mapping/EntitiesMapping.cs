using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using Flights_TQS.Entities;

namespace Flights_TQS.Mapping
{
    public class UsersMap : ClassMap<User>
    {
        public UsersMap()
        {
            Table("Users");

            Id(e => e.Id)
            .GeneratedBy
            .Identity();

            //Map(e => e.Id).Column("id");
            Map(e => e.FName).Column("f_name");
            Map(e => e.LName).Column("l_name");
            Map(e => e.BornDate).Column("born_date");
            Map(e => e.Email).Column("email");
            Map(e => e.Password).Column("password");
        }
    }

    public class PersonsMap : ClassMap<Person>
    {
        public PersonsMap()
        {
            Table("Persons");

            Id(e => e.Id)
            .GeneratedBy
            .Identity();

            //Map(e => e.Id).Column("id");
            Map(e => e.FName).Column("f_name");
            Map(e => e.LName).Column("l_name");
            Map(e => e.BornDate).Column("born_date");
            Map(e => e.Passport).Column("passport");
            Map(e => e.CitizenCard).Column("citizen_card");
            Map(e => e.PassportValid).Column("passport_valid");
            Map(e => e.CitizenCardValid).Column("citizen_card_valid");
        }
    }

    public class AirportsMap : ClassMap<Airport>
    {
        public AirportsMap()
        {
            Table("Airports");

            Id(e => e.Id)
            .GeneratedBy
            .Identity();

//          Map(e => e.Id).Column("id");
            Map(e => e.Name).Column("name");
            Map(e => e.City).Column("city");
            Map(e => e.Country).Column("country");
        }
    }

    public class AirplanesMap : ClassMap<Airplane>
    {
        public AirplanesMap()
        {
            Table("Airplanes");

            Id(e => e.Id)
            .GeneratedBy
            .Identity();

            //Map(e => e.Id).Column("id");
            Map(e => e.Model).Column("model");
            Map(e => e.Company).Column("company");
            Map(e => e.NumSeats).Column("num_seats");
        }
    }

    public class SeatsMap : ClassMap<Seat>
    {
        public SeatsMap()
        {
            Table("Seats");

            Id(e => e.Id)
            .GeneratedBy
            .Identity();

            //Map(e => e.Id).Column("id");
            Map(e => e.Row).Column("row");
            Map(e => e.Herder).Column("herder");
            Map(e => e.Class).Column("class");
            Map(e => e.Airplane).Column("airplane");
        }
    }

    public class FlightsMap : ClassMap<Flight>
    {
        public FlightsMap()
        {
            Table("Flights");

            Id(e => e.Id)
            .GeneratedBy
            .Identity();

            //Map(e => e.Id).Column("id");
            Map(e => e.Airplane).Column("airplane");
            Map(e => e.AirportDeparture).Column("airport_departure");
            Map(e => e.AirportArrive).Column("airport_arrive");
            Map(e => e.DatetimeDeparture).Column("datetime_departure");
            Map(e => e.DatetimeArrive).Column("datetime_arrive");
            Map(e => e.FlightLeg).Column("flight_leg");
            Map(e => e.Price).Column("price");
        }
    }

    public class TicketsMap : ClassMap<Ticket>
    {
        public TicketsMap()
        {
            Table("Tickets");

            Id(e => e.Id).Column("id");
            Map(e => e.Person).Column("person");
            Map(e => e.Seat).Column("seat");
            Map(e => e.Flight).Column("flight");
            Map(e => e.Price).Column("price");
            Map(e => e.Luggage).Column("luggage");
            Map(e => e.SpecialFood).Column("special_food");
            Map(e => e.SpecialTreatment).Column("special_treatment");
        }
    }

    public class ReservationsMap : ClassMap<Reservation>
    {
        public ReservationsMap()
        {
            Table("Reserves");

            Id(e => e.Id).Column("reservation_code");
            Map(e => e.User).Column("user");
            Map(e => e.Ticket).Column("ticket");
            Map(e => e.Datetime).Column("datetime");

        }
    }


}
