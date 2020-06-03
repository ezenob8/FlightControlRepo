using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlightControlWeb.Controllers;
using FlightControlWeb.Model;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using FlightControlWeb.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace FlightsTests
{
    public static class Comparer
    {
        public static bool Compare(InitialLocationDTO x, InitialLocationDTO y)
        {
            if (x == null && y == null)
                return true;
            if (x.DateTime == y.DateTime && x.Latitude == y.Latitude && x.Longitude == y.Longitude)
                return true;
            return false;
        }
        //public static bool Compare(Flight x, Flight y)
        //{
        //    if (x == null && y == null)
        //        return true;
        //    // Not checking the flight plan in the flight
        //    if (x.CompanyName == y.CompanyName && x.DateTime == y.DateTime
        //        && x.FlightPlanId == y.FlightPlanId && x.Id == y.Id
        //        && x.IsExternal == y.IsExternal && x.Latitude == y.Latitude && x.Longitude == y.Longitude
        //        && x.Passengers == y.Passengers && (x.FlightIdentifier == y.FlightIdentifier || (x.FlightIdentifier.Length == 0 && y.FlightIdentifier.Length == 0)))
        //        return true;
        //    return false;
        //}
        public static bool Compare(FlightPlanDTO x, FlightPlanDTO y)
        {
            if (x == null && y == null)
                return true;
            if (x.CompanyName == y.CompanyName && Compare(x.InitialLocation, y.InitialLocation)
                && x.Passengers == y.Passengers && x.Segments == y.Segments)
                return true;
            else return false;
        }
    }

    [TestClass]
    public class FlightPlanUnitTest
    {
        [TestMethod]
        public void ATest()
        {
            // Arrange
            var stubFlightPlanController = new FlightPlanController(new Logger<FlightPlanController>(new LoggerFactory()),
                new FakeFlightPlanDBContex());
            var testFlightPlan = GetTestFlightPlan();

            // Act
            stubFlightPlanController.Post(testFlightPlan);
            var result = (OkObjectResult)stubFlightPlanController.Get("1");


            var ha = result as OkObjectResult;
            var da = ha.Value as FlightPlanDTO;

            // Assert
            Assert.IsTrue(Comparer.Compare(testFlightPlan, (FlightPlanDTO)result.Value));
        }

        public FlightPlanDTO GetTestFlightPlan()
        {
            var flightPlan = new FlightPlanDTO();
            flightPlan.CompanyName = "Test Company";
            flightPlan.InitialLocation = new InitialLocationDTO();
            flightPlan.InitialLocation.Latitude = 12.5; flightPlan.InitialLocation.Longitude = 12.5;
            flightPlan.InitialLocation.DateTime = "2020-08-12T15:30:22Z";
            flightPlan.Passengers = 8;
            var mockSegmants = new List<LocationDTO>();
            var loc1 = new LocationDTO(); loc1.Latitude = 10; loc1.Longitude = 10;
            mockSegmants.Add(loc1);
            flightPlan.Segments = mockSegmants.ToArray();

            return flightPlan;
        }
    }
}
