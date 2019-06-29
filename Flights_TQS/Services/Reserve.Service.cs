using Flights_TQS.Entities;
using Flights_TQS.Interfaces;
using FluentNHibernate.Testing.Values;
using NHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace Flights_TQS.Services
{
    public class Reserve : BaseService, IReserve
    {
        public Reserve(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public List<Reservation> CreateReserve(int id, List<Ticket> tickets)
        {
            if (tickets == null || tickets.FirstOrDefault() == null)
                throw new InvalidOperationException("[Reservation] Null tickets");
            if (id == null || id <= 0)
                throw new InvalidOperationException("[Reservation] Null Id");
            try
            {
                List<Reservation> returnList = new List<Reservation>();
                Reservation reservation;
                string code = this.getNewCode();
                UnitOfWork.BeginTransaction();
                foreach (Ticket ticket in tickets)
                {
                    UnitOfWork.Tickets.Add(ticket);
                    string t = UnitOfWork.Tickets.GetAll().Last().Id;
                    reservation = new Reservation
                    {
                        Code = code,
                        Ticket = t,
                        User = id,
                        Datetime = DateTime.UtcNow
                    };
                    UnitOfWork.Reservations.Add(reservation);
                    returnList.Add(reservation);
                    UnitOfWork.SaveChanges();
                }
                UnitOfWork.Commit();
                return returnList;
            }

            catch (Exception ex)
            {
                UnitOfWork.Rollback();
                throw ex;
            }
        }
        public void DeleteTicket(List<Ticket> tickets)
        {
            if (tickets == null || tickets.FirstOrDefault() == null)
                throw new InvalidOperationException("[Reservation] Null tickets");
            try
            {
                UnitOfWork.BeginTransaction();
                foreach (Ticket ticket in tickets)
                {
                    UnitOfWork.Tickets.Remove(ticket.Id);
                    UnitOfWork.SaveChanges();
                }
                UnitOfWork.Commit();
            }
            catch (Exception ex)
            {
                UnitOfWork.Rollback();
                throw ex;
            }
        }

        public List<Tuple<Reservation, Ticket>> ListOwnReserves(int id)
        {
            if (id == null) throw new InvalidOperationException("[Reservation] Null Id");

            List<Tuple<Reservation, Ticket>> returnList = new List<Tuple<Reservation, Ticket>>();
            try
            {
                List<Reservation> reservations = UnitOfWork.Reservations.AsQueryable().Where(u => u.User == id).ToList();
                foreach (Reservation reserve in reservations)
                {
                    returnList.Add(
                        Tuple.Create(reserve,
                        UnitOfWork.Tickets.Get(reserve.Ticket)));
                }
                return returnList;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Seat> listSeats(Airplane airplane)
        {
            if (airplane == null) throw new InvalidOperationException("[Reservation] Null Airplane");
            List<Seat> list = UnitOfWork.Seats.AsQueryable().Where(u => u.Airplane == airplane.Id).ToList();

            try
            {

                if (list.FirstOrDefault() == null) throw new InvalidOperationException("[Reservation] Airplane not registered");
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateTickets(List<Ticket> tickets)
        {
            if (tickets == null || tickets.FirstOrDefault() == null)
                throw new InvalidOperationException("[Reservation] Null ticket");

            try
            {
                UnitOfWork.BeginTransaction();
                foreach (Ticket ticket in tickets)
                {
                    UnitOfWork.Tickets.Update(ticket);
                    UnitOfWork.SaveChanges();
                }
                UnitOfWork.Commit();

            }
            catch (Exception ex)
            {
                UnitOfWork.Rollback();
                throw ex;
            }
        }
        public string getNewCode()
        {
            string hex = UnitOfWork.Reservations.GetAll().Last().Code;
            BigInteger b1 = BigInteger.Parse(hex, NumberStyles.AllowHexSpecifier);
            return BigInteger.Add(b1, 1).ToString();
        }

    }
}
