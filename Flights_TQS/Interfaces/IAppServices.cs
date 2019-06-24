using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NHibernate;

namespace Flights_TQS.Interfaces {
  public interface IAppServices {
    #region // Attributes //
    int LoginId { get; set; }
    string LoginIP { get; set; }
    #endregion

    IHostingEnvironment Environment { get; set; }
    IConfiguration Configuration { get; set; }

    Dictionary<string, string> Auth0Settings { get; }

  }
}
