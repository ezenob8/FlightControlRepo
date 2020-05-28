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
            FlightPlanDBContext dBContext = new FlightPlanDBContext();
            ILogger<FlightPlanController> log = new Logger<FlightPlanController>(new LoggerFactory());
            FlightController flightsController = new FlightController(log, dBContext);
            var rawData = dBContext.FlightPlans.FromSqlRaw("Select");

            // Act
            var data = flightsController.Get();
            

            //Assert
            Assert.IsNotNull(data);
        }
    }
}
