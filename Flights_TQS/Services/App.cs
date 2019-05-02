using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NHibernate;
using Flights_TQS.Interfaces;

namespace Flights_TQS.Services {
  public class AppServices: IAppServices {
    #region // Atributos //
    public int LoginIdx { get; set; }

    public string LoginIP { get; set; }

    public IHostingEnvironment Environment { get; set; }

    public IConfiguration Configuration { get; set; }

    public ISessionFactory SessionFactory { get; set; }

    private Dictionary<string, string> _Settings(string idxSettings) {
      if (Configuration == null)
        return new Dictionary<string, string>();
      else
        return Configuration.GetSection(idxSettings)
          .GetChildren()
          .Select(item => new KeyValuePair<string, string>(item.Key, item.Value))
          .ToDictionary(s => s.Key, s => s.Value);
    }

    public Dictionary<string, string> Auth0Settings { get => _Settings("Auth0"); }

    public Dictionary<string, string> AwsS3Settings { get => _Settings("AWS_S3"); }

    public Dictionary<string, string> AwsSesSettings { get => _Settings("AWS_SES"); }

    public Dictionary<string, string> TrelloSettings { get => _Settings("Trello"); }

    public Dictionary<string, string> PagSeguroSettings { get => _Settings($"PagSeguro.{Configuration["Ambiente"] ?? "PRD"}"); }
    #endregion

    public AppServices(IHostingEnvironment environment, IConfiguration configuration, ISessionFactory sessionFactory) {
      Environment = environment;
      Configuration = configuration;
      SessionFactory = sessionFactory;
    }
  }
}
