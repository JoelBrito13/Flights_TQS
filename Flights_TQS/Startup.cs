using Flights_TQS.Interfaces;
using Flights_TQS.Mapping;
using Flights_TQS.Repository;
using Flights_TQS.Services;
using FluentNHibernate.Cfg;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Cfg;
using System;

namespace Flights_TQS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Register NHibernate
            services
              .AddSingleton<ISessionFactory>(_ =>
              {
                  //string ambiente = Configuration["Ambiente"];
                  string connectionString = Configuration.GetConnectionString("DefaultConnection");

                  try
                  {

                      return Fluently.Configure()
                      .Database(() => FluentNHibernate.Cfg.Db.MySQLConfiguration.Standard.ShowSql().ConnectionString(connectionString))
                      .Mappings(m =>
                      {
                          m.FluentMappings.AddFromAssemblyOf<AirplanesMap>();
                          m.FluentMappings.AddFromAssemblyOf<AirportsMap>();
                          m.FluentMappings.AddFromAssemblyOf<FlightsMap>();
                          m.FluentMappings.AddFromAssemblyOf<PersonsMap>();
                          m.FluentMappings.AddFromAssemblyOf<ReservationsMap>();
                          m.FluentMappings.AddFromAssemblyOf<SeatsMap>();
                          m.FluentMappings.AddFromAssemblyOf<TicketsMap>();
                          m.FluentMappings.AddFromAssemblyOf<UsersMap>();
                      })
//                      .ExposeConfiguration(BuildSchema)
                      .BuildSessionFactory();

                      //return Fluently.Configure()
                      //.Database(() => FluentNHibernate.Cfg.Db.MySQLConfiguration.Standard.ShowSql().ConnectionString(connectionString))
                      //.Mappings(m => m.FluentMappings
                      //  .AddFromAssemblyOf<AirplanesMap>()
                      //  .AddFromAssemblyOf<AirportsMap>()
                      //  .AddFromAssemblyOf<FlightsMap>()
                      //  .AddFromAssemblyOf<PersonsMap>()
                      //  .AddFromAssemblyOf<ReservesMap>()
                      //  .AddFromAssemblyOf<SeatsMap>()
                      //  .AddFromAssemblyOf<TicketsMap>()
                      //  .AddFromAssemblyOf<UsersMap>())
                      //.ExposeConfiguration(BuildSchema)
                      //.BuildSessionFactory();

                      // return Fluently
                      //.Configure()
                      //.Database(() => FluentNHibernate.Cfg.Db.MySQLConfiguration.Standard.ShowSql().ConnectionString(connectionString))
                      // AddFromAssemblyOf<AirportsMap>())
                      //.BuildSessionFactory();
                  }
                  catch (Exception ex)
                  {
                      throw ex;
                  }
              });

            services
              .AddSingleton<IAppServices, AppServices>()
              .AddScoped<IUnitOfWork, UnitOfWork>()
              .AddScoped<ISearch, Search>()
              .AddScoped<IReserve, Reserve>()
              .AddScoped<ILogin, Login>();
            //.AddSingleton<IAuthorizationHandler, HasScopeHandler>()
            //.AddScoped<IPagamentos, Pagamentos>()
            //.AddScoped<IPagSeguro, PagSeguro>()
            //.AddScoped<IPedidoTrello, PedidosTrello>()
            //.AddScoped<IUsuarios, Usuarios>();
        }

        private void BuildSchema(Configuration obj)
        {
            throw new NotImplementedException();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
