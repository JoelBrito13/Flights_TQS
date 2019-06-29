using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Flights_TQS.Controllers;
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
        public static dynamic flightUserDto(FlightUser flightUser) {      

      return new {
          flightUser.Id,
          flightUser.FName,
          flightUser.LName,
          flightUser.BornDate,
          flightUser.Email,
          flightUser.Auth0
        };
    }
        private readonly ILogin Login;
        public LoginController(IAppServices appServices, ILogin login) : base(appServices)
        {
            Login = login;
        }

        [HttpGet]
        [Route("Read/{id}")]
        public IActionResult Read(int id)
        {
            //if (id <= 0)
            //    id = LoginId;
            try
            {
                var flightUser = Login.Read(id, true);
                var resultado = flightUserDto(flightUser);

                IActionResult response = Ok(resultado);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        [Route("AddUser")]
        public IActionResult AddUser([FromBody] FlightUserAdd flightUserIncluir)
        {
            try
            {
                Login.Add(flightUserIncluir, true);

                IActionResult response = CreatedAtAction("AddUser", true);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Save")]
        public IActionResult Save([FromBody] FlightUser flightUsertoSave)
        {
            try
            {
                var flightUser = Login.Save(flightUsertoSave);
                var resultado = flightUserDto(flightUser);

                IActionResult response = Ok(resultado);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {

            //if (id <= 0)
            //    id = LoginId;
            try
            {
                bool resultado = Login.Delete(id);

                IActionResult response = Ok(resultado);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("Verify")]
        public IActionResult Verify()
        {
            try
            {
                string idFlightUser = this.User.FindFirstValue(AppServices.Auth0Settings["ClaimidFlightUser"]);

                if (String.IsNullOrEmpty(idFlightUser))
                    throw new Exception("[Verify] Unidentified User");

                if (!Int32.TryParse(idFlightUser, out int id))
                    throw new Exception("[Verify] Invalid Index");

                // Verify informações do id do FlightUser vindo do Auth0 no banco de dados
                FlightUser FlightUser;

                try
                {
                    FlightUser = Login.Read(id);
                }
                catch
                {
                    var idAuth0 = this.User.FindFirstValue(AppServices.Auth0Settings["ClaimNameIdentifier"]);
                    FlightUser = Login.Verify(idAuth0);
                }

             
                var resultado = new
                {
                    FlightUser = FlightUser.Id,
                    Admin = this.User.IsInRole("admin")
                };

                IActionResult response = Ok(resultado);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("ListUsers")]
        public IActionResult ListUsers()
        {
            try
            {
                var resultado = Login.listUsers();
                IActionResult response = Ok(resultado);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
