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
    public class LoginsController : BaseController
    {

        private readonly ILogin Login;
        public LoginsController(IAppServices appServices, ILogin login) : base(appServices)
        {
            Login = login;
        }

    }
}
