using Flights_TQS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights_TQS.Interfaces
{
    public interface ILogin
    {
        string Message { get; set; }
        FlightUser authenticate(AuthenticateFlightUser authenticateUser);
        FlightUser addUser(FlightUser userToAdd);

        //List<FlightUser> Listar(string filtro = null);
        //List<FlightUser> ListarNomes(string filtro = null);
        //FlightUser Ler(int idx, bool lerAuth = false);
        //FlightUser LerPorEmail(string email, bool lerAuth = false);
        //FlightUser Salvar(FlightUser usuario);
        //string SalvarFoto(FlightUser usuario, string fotoBase64);
        //bool Excluir(int idx);
        //FlightUser Verificar(string idxAuth0);
    }
}
