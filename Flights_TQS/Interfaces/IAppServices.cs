using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NHibernate;

namespace Flights_TQS.Interfaces {
  public interface IAppServices {
    #region // Attributes //
    int LoginIdx { get; set; }
    string LoginIP { get; set; }
    #endregion

    IHostingEnvironment Environment { get; set; }

    IConfiguration Configuration { get; set; }

    //Dictionary<string, string> Auth0Settings { get; }

    //Dictionary<string, string> AwsS3Settings { get; }

    //Dictionary<string, string> AwsSesSettings { get; }

    //Dictionary<string, string> TrelloSettings { get; }

    //Dictionary<string, string> PagSeguroSettings { get; }
  }
}
