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
        public ISearch Search { get; set; }
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
        //private readonly ISearch Search;
        [SetUp]
        public void Setup()
        {
            Configuration = new ConfigurationBuilder().Build();
            IHostingEnvironment environment = null;
            ISessionFactory sessionFactory = initSession();
            IAppServices appServices = new AppServices(environment, Configuration, sessionFactory);



            IUnitOfWork unitOfWork = new UnitOfWork(sessionFactory, appServices);
            Search = new Search(unitOfWork);
        }

        [Test]
        public void listAirportsByPorto()
        {
            String filter = "Porto";
            var list = Search.listAirports(filter);
            Assert.NotNull(list);
            Assert.IsInstanceOf(typeof(Airport), list.FirstOrDefault());
            Assert.IsTrue(list.FirstOrDefault().Name.StartsWith(filter) 
                || list.FirstOrDefault().City.StartsWith(filter) || list.FirstOrDefault().Country.StartsWith(filter));
        }
        [Test]
        public void listFlights_Porto_Lisbon()
        {
            Flight flight = new Flight
            {
                AirportDeparture = 71,
                AirportArrive = 70,
                DatetimeDeparture = DateTime.Parse("2019-07-01")
            };
            var list = Search.listFlights(flight);
            Assert.NotNull(list);
            var firstRestult = list.FirstOrDefault();
            Assert.IsNotNull(firstRestult);
            Assert.IsInstanceOf(typeof(Flight), firstRestult);
            Assert.AreEqual(flight.AirportDeparture, firstRestult.AirportDeparture);
            Assert.AreEqual(flight.AirportArrive, firstRestult.AirportArrive);
            Assert.LessOrEqual(flight.AirportDeparture, firstRestult.AirportDeparture);
        }
    }
}