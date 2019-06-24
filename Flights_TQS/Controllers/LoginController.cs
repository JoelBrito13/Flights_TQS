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
    public class LoginController : BaseController
    {
        private readonly ILogin Login;
        public LoginController(IAppServices appServices, ILogin login) : base(appServices)
        {
            Login = login;
        }

        // GET: api/Login/Authenticate
        [HttpPost]
        [Route("Authenticate")]
        public IActionResult Authenticate(AuthenticateFlightUser authenticateUser)
        {
            FlightUser user;
            if ((user = Login.authenticate(authenticateUser)) != null) return Ok(user);
            return BadRequest(Login.Message);
        }

        // GET: api/Login/AddUser
        [HttpPut]
        [Route("AddUser")]
        public IActionResult AddFlightUser(FlightUser userToAdd)
        {
            Console.WriteLine(this.User.ToString());
            FlightUser user;
            if ((user = Login.addUser(userToAdd)) != null) return Ok(user);
            return BadRequest(Login.Message);
        }
    }
}
