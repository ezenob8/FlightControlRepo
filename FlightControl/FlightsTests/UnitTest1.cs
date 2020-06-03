using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlightControlWeb.Controllers;
using FlightControlWeb.Model;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FlightsTests
{
    public static class Comparer
    {
        public static bool Compare(InitialLocation x, InitialLocation y)
        {
            if (x == null && y == null)
                return true;
            if (x.DateTime == y.DateTime && x.FlightPlanId == y.FlightPlanId && x.Id == y.Id
                && x.Latitude == y.Latitude && x.Longitude == y.Longitude)
                return true;
            return false;
        }
        public static bool Compare(Flight x, Flight y)
        {
            if (x == null && y == null)
                return true;
            // Not checking the flight plan in the flight
            if (x.CompanyName == y.CompanyName && x.DateTime == y.DateTime
                && x.FlightPlanId == y.FlightPlanId && x.Id == y.Id
                && x.IsExternal == y.IsExternal && x.Latitude == y.Latitude && x.Longitude == y.Longitude
                && x.Passengers == y.Passengers && (x.FlightIdentifier == y.FlightIdentifier || (x.FlightIdentifier.Length == 0 && y.FlightIdentifier.Length == 0)))
                return true;
            return false;
        }
        public static bool Compare(FlightPlan x, FlightPlan y)
        {
            if (x == null && y == null)
                return true;
            if (x.CompanyName == y.CompanyName && Compare(x.Flight, y.Flight) && x.Id == y.Id && Compare(x.InitialLocation, y.InitialLocation)
                && x.Passengers == y.Passengers && x.Segments == y.Segments)
                return true;
            else return false;
        }
    }

    [TestClass]
    public class FlightPlanUnitTest
    {

        [TestMethod]
        public void CheckIfFirstFlightPlanIsTheSame()
        {
            // Arrange
            FlightPlanDBContext dBContext = new FlightPlanDBContext();
            ILogger<FlightPlanController> log = new Logger<FlightPlanController>(new LoggerFactory());
            FlightPlanController flightsController = new FlightPlanController(log, dBContext);

            long check = 1;
            var rawData = dBContext.Find<FlightPlan>(check);
            if (rawData != null)
            {
                rawData.Flight = dBContext.Find<Flight>(rawData.Id);
                rawData.InitialLocation = dBContext.Find<InitialLocation>(rawData.Id);
            } 
            // Act
            Microsoft.AspNetCore.Mvc.OkObjectResult data = (Microsoft.AspNetCore.Mvc.OkObjectResult)flightsController.Get("1");

            //Assert
            Assert.IsTrue(Comparer.Compare((FlightPlan)data.Value, rawData));
        }
    }
}
