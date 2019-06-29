using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flights_TQS.Entities;

namespace Flights_TQS.Interfaces
{
    public interface IReserve
    {
        List<Seat> listSeats(Airplane airplane);
        void UpdateTickets(List<Ticket> tickets);
        void DeleteTicket(List<Ticket> tickets);
        List<Reservation> listOwnReserves(int id);
        List<Reservation> CreateReserve(int id, List<Ticket> tickets);
    }
}
