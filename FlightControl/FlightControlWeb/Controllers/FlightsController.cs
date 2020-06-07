using FlightControlWeb.Algorithms;
using FlightControlWeb.DTO;
using FlightControlWeb.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

            var flights = _context.Flight.Include(item => item.FlightPlan).Include(item => item.FlightPlan.InitialLocation).Include(item => item.FlightPlan.Segments).ToList().Where(filter => filter.FlightPlan.InitialLocation.DateTime.ToLocalTime() <= relative_to && relative_to <= filter.FlightPlan.EndDateFlight.ToLocalTime());

            var output = from flight in flights select new FlightDTO { FlightIdentifier=flight.FlightIdentifier,
                                                                       Longitude = CalculateNewLocation.Calculate(flight.FlightPlan.InitialLocation.DateTime.ToLocalTime(),
                                                                                                                  relative_to,
                                                                                                                  flight.FlightPlan.InitialLocation,
                                                                                                                  flight.FlightPlan.Segments.ToArray()).Longitude,
                                                                       Latitude = CalculateNewLocation.Calculate(flight.FlightPlan.InitialLocation.DateTime.ToLocalTime(),
                                                                                                                 relative_to,
                                                                                                                 flight.FlightPlan.InitialLocation,
                                                                                                                 flight.FlightPlan.Segments.ToArray()).Latitude,
                                                                       Passengers=flight.FlightPlan.Passengers,
                                                                       CompanyName=flight.FlightPlan.CompanyName,
                                                                       DateTime=flight.FlightPlan.InitialLocation.DateTime.ToString(),
                                                                       IsExternal =flight.IsExternal};
            var addedOutput = output.ToList();
            var flightsData = new List<FlightDTO>();

            if (this.Request.QueryString.ToString().Contains("sync_all"))
            {
                var servers = _context.Servers.ToList();

                foreach (var server in servers)
                {
                    using (var client = new HttpClient())
                    {
                        var response = client.GetAsync(server.ServerURL + "api/Flights?relative_to=" + DateTime.Now.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'")).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var responseContent = response.Content;

                            string responseString = responseContent.ReadAsStringAsync().Result;
                            var array = JsonConvert.DeserializeObject(responseString);
                            flightsData.AddRange(((JArray)array).Select(x => new FlightDTO
                            {
                                FlightIdentifier = (string)x["flight_id"],
                                CompanyName = (string)x["company_name"],
                                Longitude = (double)x["longitude"],
                                Latitude = (double)x["longitude"],
                                Passengers = (int)x["passengers"],
                                DateTime = ((DateTime)x["date_time"]).ToLongDateString(),
                                IsExternal = true
                            }).ToList());
                        }
                    }
            
            }
           
                
            }

            addedOutput.AddRange(flightsData);

            return Ok(addedOutput);
        }

        [HttpGet("ActiveInternalFlights")]
        public ActionResult ActiveInternalFlights()
        {
            var flights = _context.Flight.Include(item => item.FlightPlan).Include(item => item.FlightPlan.InitialLocation).Include(item => item.FlightPlan.Segments).ToList().Where(filter=> filter.FlightPlan.EndDateFlight.ToLocalTime() >= DateTime.Now && filter.FlightPlan.InitialLocation.DateTime.ToLocalTime() <= DateTime.Now);
            var output = from flight in flights
                         select new FlightDTO
                         {
                             FlightIdentifier = flight.FlightIdentifier,
                             Longitude = CalculateNewLocation.Calculate(flight.FlightPlan.InitialLocation.DateTime.ToLocalTime(),
                                                                        DateTime.Now,
                                                                        flight.FlightPlan.InitialLocation,
                                                                        flight.FlightPlan.Segments.ToArray()).Longitude,
                             Latitude = CalculateNewLocation.Calculate(flight.FlightPlan.InitialLocation.DateTime.ToLocalTime(),
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
            return NoContent();
        }

    }
}
