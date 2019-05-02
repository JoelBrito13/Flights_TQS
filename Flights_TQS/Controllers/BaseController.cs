using System;
using System.Security.Claims;
using Flights_TQS.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Flights_TQS.Controllers {
  public class BaseController: Controller {
    protected readonly IAppServices AppServices;

    public BaseController(IAppServices appServices) {
      AppServices = appServices;

      //AppServices.LoginIdx = LoginIdx;
      //AppServices.LoginIP = LoginIP;
    }

    //public int LoginIdx {
    //  get {
    //    if (this.User == null)
    //      return 0;
    //    else {
    //      string idxUsuario = this.User.FindFirstValue(AppServices.Auth0Settings["ClaimIdxUsuario"]);
    //      return (String.IsNullOrEmpty(idxUsuario) ? 0 : Convert.ToInt32(idxUsuario));
    //    }
    //  }
    //}

    //public string LoginIP {
    //  get {
    //    try {
    //      var ip = Request.HttpContext.Connection.RemoteIpAddress;
    //      return (ip?.ToString() ?? "0.0.0.0");
    //    }
    //    catch {
    //      return "0.0.0.0";
    //    }
    //  }
    //}

    //public bool IsAdmin {
    //  get { return this.User.IsInRole("admin"); }
    //}
  }
}