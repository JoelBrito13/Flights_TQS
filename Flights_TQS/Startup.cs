using Flights_TQS.Interfaces;
using Flights_TQS.Mapping;
using Flights_TQS.Repository;
using Flights_TQS.Services;
using FluentNHibernate.Cfg;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using NHibernate;
using NHibernate.Cfg;
using System;
using System.Globalization;
using System.Security.Claims;

namespace Flights_TQS
{
    public class Startup
    {
        public static IHostingEnvironment Environment { get; private set; }
        public static IConfiguration Configuration { get; private set; }

        public Startup(IHostingEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
              .AddAuthentication(options =>
              {
                  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
              })
              .AddJwtBearer(options =>
              {
                  options.Authority = $"https://{Configuration["Auth0:Domain"]}/";
                  options.Audience = Configuration["Auth0:Audience"];

                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/roles"
                  };

                  // Additional config snipped
                  //options.Events = new JwtBearerEvents
                  //{
                  //    OnTokenValidated = async ctx =>
                  //    {
                  //        // Get the calling app client id that came from the token produced by Azure AD
                  //        var AppServices = ctx.HttpContext.RequestServices.GetRequiredService<IAppServices>();
                  //        string idxUsuario = ctx.Principal.FindFirstValue(AppServices.Auth0Settings["ClaimIdxUsuario"]);

                  //        if (String.IsNullOrEmpty(idxUsuario))
                  //        {
                  //            var nameidentifier = ctx.Principal.FindFirstValue(AppServices.Auth0Settings["ClaimNameIdentifier"]);

                  //            var FlightUser = ctx.HttpContext.RequestServices.GetRequiredService<ILogin>();
                  //            idxUsuario = FlightUser.Verify(nameidentifier).Idx.ToString();

                  //            var appIdentity = new ClaimsIdentity(new List<Claim> {
                  //new Claim(AppServices.Auth0Settings["ClaimIdxUsuario"], idxUsuario)
                  //    });

                  //            ctx.Principal.AddIdentity(appIdentity);
                  //        }
                  //    }
                  //};
              });

            // Register NHibernate
            services
              .AddSingleton<ISessionFactory>(_ =>
              {
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
                          m.FluentMappings.AddFromAssemblyOf<FlightUsersMap>();
                      })
                      .BuildSessionFactory();

                  }
                  catch (Exception ex)
                  {
                      throw ex;
                  }
              });

            services
            .AddSingleton<IAuthorizationHandler, HasScopeHandler>()
            .AddSingleton<IAppServices, AppServices>()
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped<ISearch, Search>()
            .AddScoped<IReserve, Reserve>()
            .AddScoped<ILogin, Login>();

            // Add CORS
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials();
                });
            });

            // Add framework services.
            services
              .AddMvc(options =>
              {
                  if (!Environment.IsDevelopment())
                  {
                      AuthorizationPolicy authenticatedUserPolicy = new AuthorizationPolicyBuilder()
                  .RequireAuthenticatedUser()
                  .Build();

                      options.Filters.Add(new AuthorizeFilter(authenticatedUserPolicy));
                  }
              })
              .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
              .AddJsonOptions(options =>
              {
                  var settings = options.SerializerSettings;

                  settings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                  settings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;

                  var resolver = options.SerializerSettings.ContractResolver as DefaultContractResolver;
                  resolver.NamingStrategy = null;
              });

            services.Configure<FormOptions>(fo =>
            {
                fo.ValueLengthLimit = int.MaxValue;
                fo.MultipartBodyLengthLimit = int.MaxValue;
            });

            // Swagger
        //    services.AddSwaggerGen(c =>
        //    {
        //        c.SwaggerDoc("v1", new Info
        //        {
        //            Title = "Flight Reservetion | API .Net Core",
        //            Version = "v1",
        //            {
        //                Name = "TaxPayer Sistemas",
        //                Url = "https://www.taxpayer.com.br"
        //            }
        //        });

        //        string caminhoAplicacao = PlatformServices.Default.Application.ApplicationBasePath;
        //        string nomeAplicacao = PlatformServices.Default.Application.ApplicationName;
        //        string caminhoXmlDoc = Path.Combine(caminhoAplicacao, $"{nomeAplicacao}.xml");

        //        c.IncludeXmlComments(caminhoXmlDoc);
        //    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            var cultureInfo = new CultureInfo("en-US");

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors("CorsPolicy");

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "areaRoute",
                  template: "v1/{area:exists}/{controller=Admin}/{action=Index}/{id?}"
                );

                routes.MapRoute(
                  name: "default",
                  template: "v1/{controller=Home}/{action=Index}/{id?}"
                );
            });

            // Ativando middlewares para uso do Swagger
            //app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FlightTQS");
            //});
        }
    }
}
