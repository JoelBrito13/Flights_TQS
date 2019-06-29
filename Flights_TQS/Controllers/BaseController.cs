using System;
using System.Security.Claims;
using Flights_TQS.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Flights_TQS.Controllers {
  public class BaseController: Controller {
    protected readonly IAppServices AppServices;

    public BaseController(IAppServices appServices) {
      AppServices = appServices;

      //AppServices.LoginId = LoginId;
      AppServices.LoginIP = LoginIP;
    }

        //public int LoginId {
        //  get {
        //    if (this.User == null)
        //      return 0;
        //    else {
        //      string idFlightUser= this.User.FindFirstValue(AppServices.Auth0Settings["ClaimIdUsuer"]);
        //      return (String.IsNullOrEmpty(idFlightUser) ? 0 : Convert.ToInt32(idFlightUser));
        //    }
        //  }
        //}

        public string LoginIP
        {
            get
            {
                try
                {
                    var ip = Request.HttpContext.Connection.RemoteIpAddress;
                    return (ip?.ToString() ?? "0.0.0.0");
                }
                catch
                {
                    return "0.0.0.0";
                }
            }
        }

        public bool IsAdmin
        {
            get { return this.User.IsInRole("admin"); }
        }
    }
}