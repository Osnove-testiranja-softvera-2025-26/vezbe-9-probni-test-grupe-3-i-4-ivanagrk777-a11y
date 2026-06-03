using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTravelAgency.Services;

namespace TrainTravelAgency.fake
{
    public class FakeLoggerService : ILoggerService
    {
        public bool IsLogged { get; private set; }

        public void LogError(string message)
        {
            IsLogged = true;
        }
    }
} 

