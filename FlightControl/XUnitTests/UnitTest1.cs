using System;
using Xunit;
using FlightControlWeb.Controllers;
using FlightControlWeb.Model;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using FlightControlWeb.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Moq;
using Autofac.Extras.Moq;
using System.Threading.Tasks;
using System.Linq;
using Autofac.Core;
using Moq.Protected;

namespace XUnitTests
{
    public class UnitTest1
    {

        [Fact]
        public async void Are_Post_and_Then_Get_The_Same_For_FlightPlan()
        {
            //Arrange
            
            var options = new DbContextOptionsBuilder<FlightPlanDBContext>()
                .UseInMemoryDatabase(databaseName: "Products Test")
                .Options;
            var mockDB = new Mock<FlightPlanDBContext>(options) { CallBase = true };

            var mockDBCalls = new Mock<DataBaseCalls>();
            mockDBCalls.Setup((x) => x.FindLastID(It.IsAny<FlightPlanDBContext>())).Returns("");

            var flightPlanController = new FlightPlanController(null, mockDB.Object);
            flightPlanController.SetDataBaseCalls(mockDBCalls.Object);

            //Act
            await flightPlanController.Post(GetTestFlightPlan());
            var ans = await flightPlanController.Get("AAAA-0000");

            //Assert
            Assert.Equal(GetTestFlightPlan(), ans.Value);
            
        }

        public FlightPlanDTO GetTestFlightPlan()
        {
            var flightPlan = new FlightPlanDTO();
            flightPlan.CompanyName = "Test Company";
            flightPlan.InitialLocation = new InitialLocationDTO();
            flightPlan.InitialLocation.Latitude = 12.5; flightPlan.InitialLocation.Longitude = 12.5;
            DateTime dateTime = DateTime.UtcNow;

            flightPlan.InitialLocation.DateTime = dateTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'");
            flightPlan.Passengers = 8;
            var mockSegmants = new List<LocationDTO>();
            var loc1 = new LocationDTO(); loc1.Latitude = 10; loc1.Longitude = 10; loc1.TimeSpanSeconds = 10;
            mockSegmants.Add(loc1);
            flightPlan.Segments = mockSegmants.ToArray();

            return flightPlan;
        }
    }

}
