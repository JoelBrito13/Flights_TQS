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
        // GET: api/Searchers/ListAirports
        [HttpPost]
        [Route("ListAirports")]
        public IActionResult ListAirports([FromBody]Search.RecvStr recv)
        {
            try
            {
                List<Airport> airports = Search.listAirports(recv.filter);
                IActionResult response = Ok(airports);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Searchers/ListFlights
        [HttpPost]
        [Route("ListFlights")]
        public IActionResult ListFlights([FromBody]Flight flight, int pageNumber = 0)
        {
            try
            {
                List<Flight> flights = Search.listFlights(flight,pageNumber);
                IActionResult response = Ok(flights);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        
    }
}
