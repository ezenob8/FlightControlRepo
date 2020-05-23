using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlightControlWeb.Controllers;
using FlightControlWeb.Model;
using Microsoft.Extensions.Logging;

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

            // Act
            var data = flightsController.Get();

            //Assert
            Assert.IsNotNull(data);
        }
    }
}
