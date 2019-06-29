using Flights_TQS.Interfaces;
using Flights_TQS.Mapping;
using Flights_TQS.Repository;
using Flights_TQS.Services;
using FluentNHibernate.Cfg;
using NHibernate;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.AspNetCore.Hosting;
using Flights_TQS.Entities;
using System.Collections.Generic;
using System.Linq;

//using System.Configuration;

namespace Tests
{
    public class ReserveTests
    {
        public IConfiguration Configuration { get; set; }
        public IReserve Reserve { get; set; }
        private ISessionFactory initSession()
        {
            //string connectionString = Configuration.GetConnectionString("DefaultConnection");
            string connectionString = "Server = 192.168.1.15; User Id = jean; Password = wizard46; Database = Flight_TQS; ConvertZeroDatetime = true; SslMode = none";
            try
            {
                return Fluently.Configure()
                .Database(() => FluentNHibernate.Cfg.Db.MySQLConfiguration.Standard.ShowSql().ConnectionString(connectionString))
                .Mappings(m =>
                {
                    m.FluentMappings.AddFromAssemblyOf<AirplanesMap>();
                    m.FluentMappings.AddFromAssemblyOf<AirportsMap>();
                    m.FluentMappings.AddFromAssemblyOf<FlightsMap>();
                    m.FluentMappings.AddFromAssemblyOf<PersonsMap>();
                    m.FluentMappings.AddFromAssemblyOf<ReservationsMap>();
                    m.FluentMappings.AddFromAssemblyOf<SeatsMap>();
                    m.FluentMappings.AddFromAssemblyOf<TicketsMap>();
                    m.FluentMappings.AddFromAssemblyOf<FlightUsersMap>();
                })
                .BuildSessionFactory();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [SetUp]
        public void Setup()
        {
            Configuration = new ConfigurationBuilder().Build();
            IHostingEnvironment environment = null;
            ISessionFactory sessionFactory = initSession();
            IAppServices appServices = new AppServices(environment, Configuration, sessionFactory);



            IUnitOfWork unitOfWork = new UnitOfWork(sessionFactory, appServices);
            Reserve = new Reserve(unitOfWork);
        }

        [Test]
        public void listRersevation_Tickets_Jean()
        {
            int id = 1;
            var list_tuple = Reserve.ListOwnReserves(id);
            Assert.NotNull(list_tuple);
            Assert.AreEqual(list_tuple.FirstOrDefault().Item1.User, id);
            Assert.AreEqual(list_tuple.FirstOrDefault().Item1.Ticket, list_tuple.FirstOrDefault().Item2.Id);
        }
        [Test]
        public void listRersevation_Seat_Boeing777()
        {
            Airplane airplane = new Airplane
            {
                Id = 40,
                Model = "Boeing 777",
                Company = "KLM",
                NumSeats = 379
            };
            var list = Reserve.listSeats(airplane);
            Assert.NotNull(list);
            Assert.AreEqual(list.Count, airplane.NumSeats);
            Assert.AreEqual(list.FirstOrDefault().Airplane, airplane.Id);
        }
    }
}