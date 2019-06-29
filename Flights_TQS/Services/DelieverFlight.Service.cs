using Flights_TQS.Entities;
using Flights_TQS.Interfaces;
using Flights_TQS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Flights_TQS.Services
{
    public class DelieverFlight : BaseService, IDelieverFlight
    {
        public DelieverFlight(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public Airplane InsertAirplanes(AirplaneToAdd airplaneInsert)
        {
            try
            {
                UnitOfWork.BeginTransaction();
                Airplane newAirplane = new Airplane
                {
                    Model = airplaneInsert.Model,
                    Company = airplaneInsert.Company,
                    NumSeats = airplaneInsert.NumSeats
                };
                UnitOfWork.Airplanes.Add(newAirplane);
                UnitOfWork.SaveChanges();
                UnitOfWork.Commit();
                newAirplane = UnitOfWork.Airplanes.AsQueryable().Where(u => newAirplane.Equals(u)).Last();
                airplaneInsert.Id = newAirplane.Id;
                this.InsertSeats(airplaneInsert);
                return newAirplane;
            }
            catch
            {
                UnitOfWork.Rollback();
                throw;
            }
        }
        public Flight InsertFlights(EssencialFlight flight)
        {
            try
            {
                UnitOfWork.Flights.Add(flight);
                UnitOfWork.SaveChanges();
                UnitOfWork.Commit();
                return flight;
            }
            catch
            {
                UnitOfWork.Rollback();
                throw;
            }

        }
        private bool InsertSeats(AirplaneToAdd airplaneInsert)
        {
            String seatClass;
            int herderCount = 0, row = 1;
            String headers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Substring(0, airplaneInsert.numSeatsPerHeader);

            if (airplaneInsert.numSeatsFirstClass == null)
            {
                if (airplaneInsert.numSeatsBusiness != null)
                {
                    seatClass = "business";
                }
                else { seatClass = "economy"; }
            }
            else
            {
                seatClass = "first class";
            }

            for (int i = 1; i < airplaneInsert.NumSeats + 1; i++)
            {

                if (airplaneInsert.numSeatsFirstClass != null && airplaneInsert.numSeatsFirstClass == i)
                {
                    row++;
                    herderCount = 0;
                    seatClass = "business";

                }
                else if (airplaneInsert.numSeatsBusiness != null
                    && ((airplaneInsert.numSeatsFirstClass != null
                        && airplaneInsert.numSeatsBusiness + airplaneInsert.numSeatsFirstClass == i)
                    || airplaneInsert.numSeatsBusiness == i))
                {
                    row++;
                    herderCount = 0;
                    seatClass = "economy";
                }
                else if (herderCount == airplaneInsert.numSeatsPerHeader)
                {
                    row++;
                    herderCount = 0;
                }
                herderCount++;
                try
                {
                    Seat seat = new Seat
                    {
                        Row = row,
                        Herder = (string)headers.Take(herderCount),
                        Class = seatClass,
                        Airplane = airplaneInsert.Id
                    };
                    UnitOfWork.BeginTransaction();
                    UnitOfWork.Seats.Add(seat);
                    UnitOfWork.SaveChanges();
                    UnitOfWork.Commit();
                }
                catch
                {
                    UnitOfWork.Rollback();
                    throw;
                    return false;
                }
            }
            return true;
        }

    }
}
