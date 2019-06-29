using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth0.ManagementApi.Models;
using Flights_TQS.Entities;
using Flights_TQS.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Flights_TQS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReserversController : BaseController
    {
        private readonly IReserve Reserve;
        public ReserversController(IAppServices appServices, IReserve reserve) : base(appServices)
        {
            Reserve = reserve;
        }

        //Post: api/Reservers/list
        [HttpPost]
        [Route("ListOwnReserves/{id}")]
        public IActionResult listOwnReserves(int id)
        {
            try
            {
                var list = Reserve.listOwnReserves(id);

                IActionResult response = Ok(list);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //Get: api/Reservers/list
        [HttpPut]
        [Route("Insert/{id}")]
        public IActionResult CreateReseve(int id, [FromBody] List<Entities.Ticket> tickets)
        {
            try
            {
                var list = Reserve.CreateReserve(id, tickets);

                IActionResult response = Ok(list);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("ListSeats")]
        public IActionResult ListSeats([FromBody] Airplane airplane)
        {
            try
            {
                var list = Reserve.listSeats(airplane);

                IActionResult response = Ok(list);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateTickets")]
        public IActionResult UpdateTickets([FromBody] List<Entities.Ticket> tickets)
        {
            try
            {
                Reserve.UpdateTickets(tickets);

                IActionResult response = CreatedAtAction("Update Tickets", true);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [Route("DeleteTicket/{id}")]
        public IActionResult DeleteTicket(int id, [FromBody] List<Entities.Ticket> tickets)
        {
            try
            {
                Reserve.DeleteTicket(tickets);

                IActionResult response = CreatedAtAction("Deleted Ticket", true);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
