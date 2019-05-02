using System;
using Flights_TQS.Entities;

namespace Flights_TQS.Interfaces {
  public interface IUnitOfWork: IDisposable {
    IAppServices AppServices { get; }
    IRepository<Airplane> Airplanes { get; }
    IRepository<Airport> Airports { get; }
    IRepository<Flight> Flights { get; }
    IRepository<Person> Persons { get; }
    IRepository<Reservation> Reservations { get; }
    IRepository<Seat> Seats { get; }
    IRepository<Ticket> Tickets { get; }
    IRepository<User> Users { get; }

    void SaveChanges();
    void BeginTransaction();
    void Commit();
    void Rollback();
  }
}