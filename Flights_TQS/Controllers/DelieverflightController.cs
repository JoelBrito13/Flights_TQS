using Flights_TQS.Entities;
using Flights_TQS.Interfaces;
using Flights_TQS.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights_TQS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DelieverflightController : BaseController
    {
        private readonly DelieverFlight DelieverFlight;
        public DelieverflightController(IAppServices appServices, DelieverFlight delieverFlight) : base(appServices)
        {
            this.DelieverFlight = delieverFlight;
        }


        // Put: api/delieverflight/airport
        [HttpPut]
        [Route("InsertAirplane")]
        public IActionResult InsertAirplane([FromBody]AirplaneToAdd airplaneInsert)
        {
            try
            {
                Airplane airplane = DelieverFlight.InsertAirplanes(airplaneInsert);
                IActionResult response = Ok(airplane);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Put: api/delieverflight/airport
        [HttpPut]
        [Route("InsertFlight")]
        public IActionResult InsertFlight([FromBody]EssencialFlight essencialFlight)
        {
            try
            {
                Flight flight = DelieverFlight.InsertFlights(essencialFlight);
                IActionResult response = Ok(flight);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
