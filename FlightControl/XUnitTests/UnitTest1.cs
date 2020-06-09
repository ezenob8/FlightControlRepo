//using System;
//using Xunit;
//using FlightControlWeb.Controllers;
//using FlightControlWeb.Model;
//using Microsoft.Extensions.Logging;
//using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;
//using FlightControlWeb.DTO;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore.Query.Internal;
//using Moq;
//using Autofac.Extras.Moq;

//namespace XUnitTests
//{
//    public class UnitTest1
//    {
//        private readonly FlightPlanController FlightPlanControllerTest;
//        private readonly Mock<FlightPlanDBContext> DbMock = new Mock<FlightPlanDBContext>();
//        private readonly Mock<ILogger<FlightPlanController>> logMock = new Mock<ILogger<FlightPlanController>>();
//        public UnitTest1()
//        {
//            FlightPlanControllerTest = new FlightPlanController(logMock.Object, DbMock.Object);
//        }

//        [Fact]
//        public void GetValid()
//        {
            
//            using(var mock = AutoMock.GetLoose())
//            {
//                // Arrange
//                mock.Mock<FlightPlanController>()
//                    .Setup(x => x.Get("1"))
//                    .Returns(new ObjectResult(GetTestFlightPlan()));
//                var controllerInTest = mock.Create<FlightPlanController>();

//                // Act
//                var expected = new ObjectResult(GetTestFlightPlan());
//                var actual = controllerInTest.Get("1");

//                Assert.True(actual != null);
//                Assert.Equal(expected.Value, actual.Value);
//            } 

//        }

//        public FlightPlanDTO GetTestFlightPlan()
//        {
//            var flightPlan = new FlightPlanDTO();
//            flightPlan.CompanyName = "Test Company";
//            flightPlan.InitialLocation = new InitialLocationDTO();
//            flightPlan.InitialLocation.Latitude = 12.5; flightPlan.InitialLocation.Longitude = 12.5;
//            DateTime dateTime = DateTime.UtcNow;

//            flightPlan.InitialLocation.DateTime = dateTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'");
//            flightPlan.Passengers = 8;
//            var mockSegmants = new List<LocationDTO>();
//            var loc1 = new LocationDTO(); loc1.Latitude = 10; loc1.Longitude = 10; loc1.TimeSpanSeconds = 10;
//            mockSegmants.Add(loc1);
//            flightPlan.Segments = mockSegmants.ToArray();

//            return flightPlan;
//        }
//    }

//}
