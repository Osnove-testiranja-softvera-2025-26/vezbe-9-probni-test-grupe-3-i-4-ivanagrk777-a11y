using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTravelAgency.Services;

namespace TrainTravelAgency.fake
{

    public class FakeDistanceCalculationService : IDistanceCalculationService
    {
        private readonly double _distance;

        public FakeDistanceCalculationService(double distance)
        {
            _distance = distance;
        }

        public double CalculateDistance(Guid source, Guid destination)
        {
            return _distance;

        }
        
    }
}

