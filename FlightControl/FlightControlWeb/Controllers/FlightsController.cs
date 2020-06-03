using FlightControlWeb.Algorithms;
using FlightControlWeb.DTO;
using FlightControlWeb.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace FlightControlWeb.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly ILogger<FlightPlanController> _logger;
        private readonly FlightPlanDBContext _context;

        public FlightsController(ILogger<FlightPlanController> logger, FlightPlanDBContext context)
        {
            _logger = logger;
            _context = context;
        }

       
        [HttpGet]
        public ActionResult Get(DateTime relative_to, bool? sync_all)
        {
            var flights = _context.Flight.Include(item => item.FlightPlan).Include(item => item.FlightPlan.InitialLocation).ToList().Where(item => item.FlightPlan.InitialLocation.DateTime.Day == relative_to.Day &&
                                                    item.FlightPlan.InitialLocation.DateTime.Month == relative_to.Month &&
                                                    item.FlightPlan.InitialLocation.DateTime.Year == relative_to.Year &&
                                                    item.FlightPlan.InitialLocation.DateTime.Hour == relative_to.Hour &&
                                                    item.FlightPlan.InitialLocation.DateTime.Minute == relative_to.Minute &&
                                                    ((!sync_all.HasValue && !item.IsExternal) || sync_all.HasValue ));
            var output = from flight in flights select new FlightDTO { FlightIdentifier=flight.FlightIdentifier,
                                                                       Longitude=flight.FlightPlan.InitialLocation.Longitude,
                                                                       Latitude=flight.FlightPlan.InitialLocation.Latitude,
                                                                       Passengers=flight.FlightPlan.Passengers,
                                                                       CompanyName=flight.FlightPlan.CompanyName,
                                                                       DateTime=flight.FlightPlan.InitialLocation.DateTime.ToString(),
                                                                       IsExternal =flight.IsExternal};
            return Ok(output);
        }

        [HttpGet("ActiveInternalFlights")]
        public ActionResult ActiveInternalFlights()
        {
            var flights = _context.Flight.Include(item => item.FlightPlan).Include(item => item.FlightPlan.InitialLocation).Include(item => item.FlightPlan.Segments).ToList().Where(filter=> filter.FlightPlan.EndDateFlight.ToLocalTime() >= DateTime.Now && filter.FlightPlan.InitialLocation.DateTime.ToLocalTime() <= DateTime.Now);
            var output = from flight in flights
                         select new FlightDTO
                         {
                             FlightIdentifier = flight.FlightIdentifier,
                             Longitude = CalculateNewLocation.Calculate(flight.FlightPlan.InitialLocation.DateTime,
                                                                        DateTime.Now,
                                                                        flight.FlightPlan.InitialLocation,
                                                                        flight.FlightPlan.Segments.ToArray()).Longitude,
                             Latitude = CalculateNewLocation.Calculate(flight.FlightPlan.InitialLocation.DateTime,
                                                                        DateTime.Now,
                                                                        flight.FlightPlan.InitialLocation,
                                                                        flight.FlightPlan.Segments.ToArray()).Latitude,
                             Passengers = flight.FlightPlan.Passengers,
                             CompanyName = flight.FlightPlan.CompanyName,
                             DateTime = flight.FlightPlan.InitialLocation.DateTime.ToString(),
                             IsExternal = flight.IsExternal
                         };
            return Ok(output);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            Flight flight = _context.Flight.Include(item => item.FlightPlan).Where(item => item.FlightIdentifier == id).First();
            FlightPlan flightPlan = flight.FlightPlan;
            _context.FlightPlans.Remove(flightPlan);
            _context.Flight.Remove(flight);
            _context.SaveChanges();
            return Ok(null);
        }

    }
}
