using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlightControlWeb.Controllers;
using FlightControlWeb.Model;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace FlightsTests
{
    [TestClass]
    public class FlightPlanUnitTest
    {
        [TestMethod]
        public void CheackIfValid()
        {
            // Arrange
            FlightDBContext dBContext = new FlightDBContext();
            ILogger<FlightsController> log = new Logger<FlightsController>(new LoggerFactory());
            FlightsController flightsController = new FlightsController(log, dBContext);
            var rawData = dBContext.Flights.FromSqlRaw("Select");

            // Act
            var data = flightsController.Get();
            

            //Assert
            Assert.IsNotNull(data);
        }
    }
}
