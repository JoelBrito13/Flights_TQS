using System;
using Flights_TQS.Interfaces;

namespace Flights_TQS.Services {
  public class BaseService {
    public readonly IUnitOfWork UnitOfWork;

    public BaseService(IUnitOfWork unitOfWork) {
      UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork), "Falha ao inicializar o serviço");

    }
  }
}

