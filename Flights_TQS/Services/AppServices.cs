using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NHibernate;
using Flights_TQS.Interfaces;

namespace Flights_TQS.Services {
  public class AppServices: IAppServices {
        #region // Atributos //
        public int LoginId { get; set; }
        public string LoginIP { get; set; }
        public IHostingEnvironment Environment { get; set; }
        public IConfiguration Configuration { get; set; }
        public ISessionFactory SessionFactory { get; set; }

        private Dictionary<string, string> _Settings(string idxSettings)
        {
            if (Configuration == null)
                return new Dictionary<string, string>();
            else
                return Configuration.GetSection(idxSettings)
                  .GetChildren()
                  .Select(item => new KeyValuePair<string, string>(item.Key, item.Value))
                  .ToDictionary(s => s.Key, s => s.Value);
        }

        public Dictionary<string, string> Auth0Settings { get => _Settings("Auth0"); }
         #endregion
        public AppServices(IHostingEnvironment environment, IConfiguration configuration, ISessionFactory sessionFactory)
        {
            Environment = environment;
            Configuration = configuration;
            SessionFactory = sessionFactory;
        }
    }
}
