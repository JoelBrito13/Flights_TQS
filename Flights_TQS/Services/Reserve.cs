using Flights_TQS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights_TQS.Services
{
    public class Reserve : BaseService, IReserve
    {
        public Reserve(IUnitOfWork unitOfWork) : base(unitOfWork) { }


    }
}
