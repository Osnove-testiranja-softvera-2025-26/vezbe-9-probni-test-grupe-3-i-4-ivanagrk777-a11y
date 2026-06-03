using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTravelAgency.Exceptions;
using TrainTravelAgency.Models;
using TrainTravelAgency.Services;

namespace TrainTravelAgency.fake
{
        public class FakeUserService : IUserService
        {
            private readonly User _user;
            private readonly bool _throwException;

            public FakeUserService(User user, bool throwException = false)
            {
                _user = user;
                _throwException = throwException;
            }

            public User GetUserById(Guid userId)
            {
                if (_throwException)
                {
                    throw new ExternalServiceErrorException();
                }

                return _user;
            }
        }
    
}

