

namespace TrainTravelAgency.Test
{

    public class ReservationServiceTest
    {

        public ReservationService(
          IUserService userService,
          ILoggerService loggerService,
          IDistanceCalculationService distanceCalculationService)
        {
            _userService = userService;
            _loggerService = loggerService;
            _distanceCalculationService = distanceCalculationService;
        }

        [Test]
        public void CalculateTicketPriceForUser_FirstClass_NormalPrice_ReturnsDistanceTimes01()
        {
            User user = new User
            {
                NumberOfTicketsPurchasedInTheLastMonth = 5
            };

            var userService = new FakeUserService(user);
            var loggerService = new FakeLoggerService();
            var distanceService = new FakeDistanceCalculationService(0);

            var service = new ReservationService(userService, loggerService, distanceService);

            double result = service.CalculateTicketPriceForUser(1000, TicketType.FirstClass, Guid.NewGuid());

            Assert.That(result, Is.EqualTo(100));
        }

        [Test]
        public void CalculateTicketPriceForUser_FirstClass_DiscountByDistance_ReturnsDistanceTimes006()
        {
            User user = new User
            {
                NumberOfTicketsPurchasedInTheLastMonth = 5
            };

            var service = new ReservationService(
            new FakeUserService(user),
            new FakeLoggerService(),
            new FakeDistanceCalculationService(0));

            double result = service.CalculateTicketPriceForUser(1600, TicketType.FirstClass, Guid.NewGuid());

            Assert.That(result, Is.EqualTo(96));
        }

        [Test]
        public void CalculateTicketPriceForUser_FirstClass_DiscountByTickets_ReturnsDistanceTimes006()
        {
            User user = new User
            {
                NumberOfTicketsPurchasedInTheLastMonth = 11
            };

            var service = new ReservationService(
            new FakeUserService(user),
            new FakeLoggerService(),
            new FakeDistanceCalculationService(0));

            double result = service.CalculateTicketPriceForUser(1000, TicketType.FirstClass, Guid.NewGuid());

            Assert.That(result, Is.EqualTo(60));
        }

        [Test]
        public void CalculateTicketPriceForUser_SecondClass_NormalPrice_ReturnsDistanceTimes005()
        {
            User user = new User
            {
                NumberOfTicketsPurchasedInTheLastMonth = 10
            };

            var service = new ReservationService(
            new FakeUserService(user),
            new FakeLoggerService(),
            new FakeDistanceCalculationService(0));

            double result = service.CalculateTicketPriceForUser(1000, TicketType.SecondClass, Guid.NewGuid());

            Assert.That(result, Is.EqualTo(50));
        }

        [Test]
        public void CalculateTicketPriceForUser_SecondClass_Discount_ReturnsDistanceTimes004()
        {
            User user = new User
            {
                NumberOfTicketsPurchasedInTheLastMonth = 15
            };

            var service = new ReservationService(
            new FakeUserService(user),
            new FakeLoggerService(),
            new FakeDistanceCalculationService(0));

            double result = service.CalculateTicketPriceForUser(1200, TicketType.SecondClass, Guid.NewGuid());

            Assert.That(result, Is.EqualTo(48));
        }

        [Test]
        public void CalculateTicketPriceForUser_Economic_ReturnsDistanceTimes001()
        {
            User user = new User
            {
                NumberOfTicketsPurchasedInTheLastMonth = 20
            };

            var service = new ReservationService(
            new FakeUserService(user),
            new FakeLoggerService(),
            new FakeDistanceCalculationService(0));

            double result = service.CalculateTicketPriceForUser(1000, TicketType.Economic, Guid.NewGuid());

            Assert.That(result, Is.EqualTo(10));
        }

        [Test]
        public void CalculateTicketPriceForUser_UserServiceThrowsException_LogsErrorAndThrowsException()
        {
            User user = new User();
            var loggerService = new FakeLoggerService();

            var service = new ReservationService(
            new FakeUserService(user, true),
            loggerService,
            new FakeDistanceCalculationService(0));

            Assert.Throws<ExternalServiceErrorException>(() =>
            service.CalculateTicketPriceForUser(1000, TicketType.FirstClass, Guid.NewGuid()));

            Assert.That(loggerService.IsLogged, Is.True);
        }

        [Test]
        public void RecommendTicketType_BeverageAndStandardSeat_ReturnsFirstClass()
        {
            var service = new ReservationService(
            new FakeUserService(new User()),
            new FakeLoggerService(),
            new FakeDistanceCalculationService(0));

            TicketType? result = service.RecommendTicketType(SeatType.Standard, 10, true, 1);

            Assert.That(result, Is.EqualTo(TicketType.FirstClass));
        }

        [Test]
        public void RecommendTicketType_BeverageAndRegularSeat_ReturnsNull()
        {
            var service = new ReservationService(
            new FakeUserService(new User()),
            new FakeLoggerService(),
            new FakeDistanceCalculationService(0));

            TicketType? result = service.RecommendTicketType(SeatType.Regular, 10, true, 1);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void RecommendTicketType_HeavyLuggage_ReturnsSecondClass()
        {
            var service = new ReservationService(
            new FakeUserService(new User()),
            new FakeLoggerService(),
            new FakeDistanceCalculationService(0));

            TicketType? result = service.RecommendTicketType(SeatType.Regular, 35, false, 1);

            Assert.That(result, Is.EqualTo(TicketType.SecondClass));
        }

        [Test]
        public void RecommendTicketType_HeavyLuggageAndTravelHourBetween2And5_ReturnsNull()
        {
            var service = new ReservationService(
            new FakeUserService(new User()),
            new FakeLoggerService(),
            new FakeDistanceCalculationService(0));

            TicketType? result = service.RecommendTicketType(SeatType.Regular, 35, false, 3);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void RecommendTicketType_NoOptions_ReturnsEconomic()
        {
            var service = new ReservationService(
            new FakeUserService(new User()),
            new FakeLoggerService(),
            new FakeDistanceCalculationService(0));

            TicketType? result = service.RecommendTicketType(SeatType.Regular, 10, false, 1);

            Assert.That(result, Is.EqualTo(TicketType.Economic));
        }

        [Test]
        public void GetDistanceBetweenCities_ConvertsMilesToKilometers()
        {
            var service = new ReservationService(
            new FakeUserService(new User()),
            new FakeLoggerService(),
            new FakeDistanceCalculationService(10));

            double result = service.GetDistanceBetweenCities(Guid.NewGuid(), Guid.NewGuid());

            Assert.That(result, Is.EqualTo(16.09).Within(0.01));
        }

    }
        
}


