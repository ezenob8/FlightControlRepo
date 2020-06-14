using System;
using Xunit;
using FlightControlWeb.Controllers;
using FlightControlWeb.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using FlightControlWeb.DTO;
using Moq;

namespace XUnitTests
{
    public class UnitTest1
    {

        [Fact]
        public async void Are_Post_and_Then_Get_The_Same_For_FlightPlan()
        {
            //Arrange            
            var options = new DbContextOptionsBuilder<FlightPlanDBContext>()
                .UseInMemoryDatabase(databaseName: "Test database");
            var mockDB = new Mock<FlightPlanDBContext>(options) { CallBase = true };

            var flightPlanController = new FlightPlanController(null, mockDB.Object);
            var testPlan = GetTestFlightPlanVer1();

            //Act
            await flightPlanController.Post(testPlan);
            var ans = await flightPlanController.Get("AAAA-0001");

            //Assert
            Assert.Equal(testPlan.CompanyName, ans.Value.CompanyName);
            Assert.Equal(testPlan.InitialLocation.Latitude, ans.Value.InitialLocation.Latitude);
            Assert.Equal(testPlan.InitialLocation.Longitude, ans.Value.InitialLocation.Longitude);
            Assert.Equal(testPlan.Passengers, ans.Value.Passengers);
            Assert.Equal(testPlan.Segments, ans.Value.Segments);
        }
       
        public ServerDTO server1()
        {
            return new ServerDTO()
            {
                ServerId = "1",
                ServerURL = "http://www.Server1.com"
            };
        }
        public ServerDTO server2()
        {
            return new ServerDTO()
            {
                ServerId = "2",
                ServerURL = "http://www.Server2.com"
            };
        }
        public FlightPlanDTO GetTestFlightPlanVer1()
        {
            var flightPlan = new FlightPlanDTO();
            flightPlan.CompanyName = "Test Company";
            flightPlan.InitialLocation = new InitialLocationDTO();
            flightPlan.InitialLocation.Latitude = 12.5; flightPlan.InitialLocation.Longitude = 12.5;
            DateTime dateTime = DateTime.UtcNow;

            flightPlan.InitialLocation.DateTime = dateTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'");
            flightPlan.Passengers = 8;
            var mockSegmants = new List<LocationDTO>();
            var loc1 = new LocationDTO(); loc1.Latitude = 10; loc1.Longitude = 10; loc1.TimeSpanSeconds = 10000;
            var loc2 = new LocationDTO(); loc1.Latitude = 20; loc1.Longitude = 20; loc1.TimeSpanSeconds = 20000;

            mockSegmants.Add(loc1);
            mockSegmants.Add(loc2);
            flightPlan.Segments = mockSegmants.ToArray();

            return flightPlan;
        }
        public FlightPlanDTO GetTestFlightPlanVer2()
        {
            var flightPlan = new FlightPlanDTO();
            flightPlan.CompanyName = "A Cooler Test Company";
            flightPlan.InitialLocation = new InitialLocationDTO();
            flightPlan.InitialLocation.Latitude = 30; flightPlan.InitialLocation.Longitude = 30;
            DateTime dateTime = DateTime.UtcNow;

            flightPlan.InitialLocation.DateTime = dateTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'");
            flightPlan.Passengers = 55;
            var mockSegmants = new List<LocationDTO>();
            var loc1 = new LocationDTO(); loc1.Latitude = 4; loc1.Longitude = 2; loc1.TimeSpanSeconds = 5000;
            var loc2 = new LocationDTO(); loc1.Latitude = 7; loc1.Longitude = 9; loc1.TimeSpanSeconds = 40000;

            mockSegmants.Add(loc1);
            mockSegmants.Add(loc2);
            flightPlan.Segments = mockSegmants.ToArray();

            return flightPlan;
        }
    }

}
