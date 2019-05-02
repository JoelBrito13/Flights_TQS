using System;
using NHibernate;
using Flights_TQS.Entities;
using Flights_TQS.Interfaces;

namespace Flights_TQS.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        #region // Atributos //
        // Private
        private ISessionFactory _SessionFactory { get; }
        private ISession _Session { get; set; }
        private IAppServices _AppServices { get; }

        private Repository<Airplane> _Airplanes;
        private Repository<Airport> _Airports;
        private Repository<Flight> _Flights;
        private Repository<Person> _Persons;
        private Repository<Reservation> _Reservations;
        private Repository<Seat> _Seats;
        private Repository<Ticket> _Tickets;
        private Repository<User> _Users;

        // Public
        public ISession Session
        {
            get
            {
                if ((_Session == null) || !_Session.IsOpen) _Session = _SessionFactory.OpenSession();
                return _Session;
            }
        }

        public IAppServices AppServices { get => _AppServices; }
        #endregion

        public UnitOfWork(ISessionFactory sessionFactory, IAppServices appServices)
        {
            _SessionFactory = sessionFactory;
            _AppServices = appServices;
        }

        public IRepository<Airplane> Airplanes
        {
            get
            {
                if (_Airplanes == null) _Airplanes = new Repository<Airplane>(Session);
                return _Airplanes;
            }
        }
        public IRepository<Airport> Airports
        {
            get
            {
                if (_Airports == null) _Airports = new Repository<Airport>(Session);
                return _Airports;
            }
        }
        public IRepository<Flight> Flights
        {
            get
            {
                if (_Flights == null) _Flights = new Repository<Flight>(Session);
                return _Flights;
            }
        }
        public IRepository<Person> Persons
        {
            get
            {
                if (_Persons == null) _Persons = new Repository<Person>(Session);
                return _Persons;
            }
        }
        public IRepository<Reservation> Reservations
        {
            get
            {
                if (_Reservations == null) _Reservations = new Repository<Reservation>(Session);
                return _Reservations;
            }
        }
        public IRepository<Seat> Seats
        {
            get
            {
                if (_Seats == null) _Seats = new Repository<Seat>(Session);
                return _Seats;
            }
        }
        public IRepository<Ticket> Tickets
        {
            get
            {
                if (_Tickets == null) _Tickets = new Repository<Ticket>(Session);
                return _Tickets;
            }
        }
        public IRepository<User> Users
        {
            get
            {
                if (_Users == null) _Users = new Repository<User>(Session);
                return _Users;
            }
        }

        public void SaveChanges()
        {
            Session.Flush();
        }

        public void BeginTransaction()
        {
            Session.BeginTransaction();
        }

        public void Commit()
        {
            if (Session.Transaction != null) Session.Transaction.Commit();
        }

        public void Rollback()
        {
            if ((Session.Transaction != null) && Session.Transaction.IsActive) Session.Transaction.Rollback();
        }

        // IDisposable
        private bool disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed && disposing) Session.Dispose(); // --> Com o conceito de Injeção de Dependência do DbContext, o GC encarrega-se de dar Dispose
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}