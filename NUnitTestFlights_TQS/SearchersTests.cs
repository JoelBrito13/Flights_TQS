using Flights_TQS.Interfaces;
using Flights_TQS.Repository;
using Flights_TQS.Services;
using NUnit.Framework;

namespace Tests
{
    public class SearchersTests
    {
        private readonly ISearch Search;
        [SetUp]
        public void Setup()
        {
            IUnitOfWork unitOfWork = new UnitOfWork(sessionFactory, appServices);
            ISearch search = new Search(unitOfWork);

        }

        [Test]
        public void returnListAirplanes()
        {

                    private readonly ISearch Search;
            List<Airplane> airplanes = Search.listAirplanes();
            //if ((numRegistros > -1) && (pagina > -1)) pedidos = pedidos.Skip(numRegistros * pagina).Take(numRegistros).ToList();

            //var resultado = pedidos.Select(PedidoDto)
            //  .OrderByDescending(p => p["DataIni"])
            //  .ToList();
            IActionResult response = Ok(airplanes);
            return response;
            Assert.Pass();
        }
    }
}