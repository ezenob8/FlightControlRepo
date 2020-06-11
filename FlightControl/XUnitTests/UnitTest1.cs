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
            flightPlanController.SetDataBaseCalls(new DataBaseCalls());
            flightPlanController.SetOptionForDBContext(options);
            var testPlan = GetTestFlightPlanVer1();

            //Act
            await flightPlanController.Post(testPlan);
            var ans = await flightPlanController.Get(new DataBaseCalls().FindLastID(mockDB.Object));

            //Assert
            Assert.Equal(testPlan.CompanyName, ans.Value.CompanyName);
            Assert.Equal(testPlan.InitialLocation.Latitude, ans.Value.InitialLocation.Latitude);
            Assert.Equal(testPlan.InitialLocation.Longitude, ans.Value.InitialLocation.Longitude);
            Assert.Equal(testPlan.Passengers, ans.Value.Passengers);
            Assert.Equal(testPlan.Segments, ans.Value.Segments);
        }

        [Fact]
        public async void Does_Server_sycnorize_With_Another_Server()
        {
            //Arrange
            var optionsForServerOne = new DbContextOptionsBuilder<FlightPlanDBContext>()
                .UseInMemoryDatabase(databaseName: "First Test database");
            var mockDBForServerOne = new Mock<FlightPlanDBContext>(optionsForServerOne) { CallBase = true };

            var optionsForServerTwo = new DbContextOptionsBuilder<FlightPlanDBContext>()
                .UseInMemoryDatabase(databaseName: "Seconed Test database");
            var mockDBForServerTwo = new Mock<FlightPlanDBContext>(optionsForServerTwo) { CallBase = true };

            var firstFlightPlanController = new FlightPlanController(null, mockDBForServerOne.Object);

            var dateBaseCallsOfServer1 = new Mock<DataBaseCalls>();
            dateBaseCallsOfServer1.Setup(x => x.FindLastID(It.IsAny<FlightPlanDBContext>()))
                .Returns(new DataBaseCalls().FindLastID(mockDBForServerOne.Object));
            var dateBaseCallsOfServer2 = new Mock<DataBaseCalls>();
            dateBaseCallsOfServer2.Setup(x => x.FindLastID(It.IsAny<FlightPlanDBContext>()))
                .Returns(new DataBaseCalls().FindLastID(mockDBForServerTwo.Object));

            firstFlightPlanController.SetDataBaseCalls(dateBaseCallsOfServer1.Object);
            firstFlightPlanController.SetOptionForDBContext(optionsForServerOne);
            var seconedFlightPlanController = new FlightPlanController(null, mockDBForServerTwo.Object);
            seconedFlightPlanController.SetDataBaseCalls(dateBaseCallsOfServer2.Object);
            seconedFlightPlanController.SetOptionForDBContext(optionsForServerTwo);

            var firstFlightController = new Mock<FlightsController>(null, mockDBForServerOne.Object);
            firstFlightController.Object.SetDataBaseCalls(dateBaseCallsOfServer1.Object);
            var seconedFlightController = new Mock<FlightsController>(null, mockDBForServerTwo.Object);
            seconedFlightController.Object.SetDataBaseCalls(dateBaseCallsOfServer2.Object);

            var firstServerController = new ServersController(null, mockDBForServerOne.Object);
            var seconedServerController = new ServersController(null, mockDBForServerTwo.Object);        

            var testPlan1 = GetTestFlightPlanVer1();
            var testPlan2 = GetTestFlightPlanVer2();

            await firstFlightPlanController.Post(testPlan1);
            await seconedFlightPlanController.Post(testPlan2);

            await firstServerController.Post(server2());
            await seconedServerController.Post(server1());

            //Act
            dateBaseCallsOfServer1.Setup(x => x.GetAllFlightsFromServer(It.IsAny<List<FlightDTO>>(), It.IsAny<Server>()))
                .Returns((List<FlightDTO>)seconedFlightController.Object.Get(DateTime.Now, false).Value);
            dateBaseCallsOfServer2.Setup(x => x.GetAllFlightsFromServer(It.IsAny<List<FlightDTO>>(), It.IsAny<Server>()))
                .Returns((List<FlightDTO>)firstFlightController.Object.Get(DateTime.Now, false).Value);

            var responseFromFirstController = (List<FlightDTO>)firstFlightController.Object.Get(DateTime.Now, true).Value;
            var responseFromSeconedController = (List<FlightDTO>)seconedFlightController.Object.Get(DateTime.Now, true).Value;

            //Assert
            Assert.Contains(responseFromSeconedController.ToArray()[1], responseFromFirstController);
            Assert.Contains(responseFromSeconedController.ToArray()[0], responseFromFirstController);
            Assert.Contains(responseFromFirstController.ToArray()[0], responseFromSeconedController);
            Assert.Contains(responseFromFirstController.ToArray()[1], responseFromSeconedController);
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
