using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flights_TQS.Entities;
using Flights_TQS.Interfaces;
using Flights_TQS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Flights_TQS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchersController : BaseController    {

        private readonly ISearch Search;
        public SearchersController(IAppServices appServices, ISearch search) : base(appServices)
        {
           Search = search;
        }

        // GET: api/Searchers
        [HttpGet]
        public IEnumerable<string> GetS()
        {
            return new string[] { "value1", "value2" };
        }
        // GET: api/Searchers/ListAirplanes
        [HttpGet]
        [Route("ListAirplanes")]
        public IActionResult ListAirplane()
        {
            try
            {
                List<Airplane> airplanes = Search.listAirplanes();
                //if ((numRegistros > -1) && (pagina > -1)) pedidos = pedidos.Skip(numRegistros * pagina).Take(numRegistros).ToList();

                //var resultado = pedidos.Select(PedidoDto)
                //  .OrderByDescending(p => p["DataIni"])
                //  .ToList();
                IActionResult response = Ok(airplanes);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // GET: api/Searchers/ListAirports
        [HttpGet]
        [Route("ListAirports")]
        
        public IActionResult ListAirport()
        {
            try
            {
                List<Airport> airports = Search.listAirports();
                IActionResult response = Ok(airports);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // GET: api/Searchers/ListFlights
        [HttpGet]
        [Route("ListFlights")]
        public IActionResult ListFlight()
        {
            try
            {
                List<Flight> flights = Search.listFlights();
                IActionResult response = Ok(flights);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // GET: api/Searchers/Persons
        [HttpGet]
        [Route("ListPersons")]
        public IActionResult ListPerson()
        {
            try
            {
                List<Person> persons = Search.listPersons();
                IActionResult response = Ok(persons);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // GET: api/Searchers/ListReservations
        [HttpGet]
        [Route("ListReservations")]
        public IActionResult ListReservations()
        {
            try
            {
                List<Reservation> reservations = Search.listReservations();
                IActionResult response = Ok(reservations);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // GET: api/Searchers/ListSeats
        [HttpGet]
        [Route("ListSeats")]
        public IActionResult ListSeat()
        {
            try
            {
                List<Seat> seats = Search.listSeats();
                IActionResult response = Ok(seats);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // GET: api/Searchers/ListTickets
        [HttpGet]
        [Route("ListTickets")]
        public IActionResult ListTicket()
        {
            try
            {
                List<Ticket> tickets = Search.listTickets();
                IActionResult response = Ok(tickets);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // GET: api/Searchers/ListUsers
        [HttpGet]
        [Route("ListUsers")]
        public IActionResult ListUser()
        {
            try
            {
                List<User> users = Search.listUsers();
                IActionResult response = Ok(users);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // GET: api/Searchers/ListUsers
        [HttpPut]
        [Route("PutTicket")]
        public IActionResult PutTicket(Ticket ticket)
        {
            try
            {
                List<User> users = Search.listUsers();
                if (users != null)
                {
                    IActionResult response = Ok(users);
                    return response;
                }
                return BadRequest(Search.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //// GET: api/Searchers/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/Searchers
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT: api/Searchers/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
