using FlightControlWeb.Algorithms;
using FlightControlWeb.CORSManager;
using FlightControlWeb.DTO;
using FlightControlWeb.Model;
using Microsoft.AspNetCore.Cors;
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
        private readonly DataBaseCalls dataBaseCalls;

        public FlightsController(ILogger<FlightPlanController> logger, FlightPlanDBContext context)
        {
            _logger = logger;
            _context = context;
            dataBaseCalls = new DataBaseCalls();
        }

        [HttpGet]
        public OkObjectResult Get(DateTime relative_to, bool? sync_all)
        {
            var flights = dataBaseCalls.GetFlights(_context, relative_to);

            var output = from flight in flights
                         select new FlightDTO
                         {
                             FlightIdentifier = flight.FlightIdentifier,
                             Longitude = CalculateNewLocation.Calculate(flight.FlightPlan.InitialLocation.DateTime.ToLocalTime(),
                                                                                           relative_to,
                                                                                           flight.FlightPlan.InitialLocation,
                                                                                           flight.FlightPlan.Segments.ToArray()).Longitude,
                             Latitude = CalculateNewLocation.Calculate(flight.FlightPlan.InitialLocation.DateTime.ToLocalTime(),
                                                                                          relative_to,
                                                                                          flight.FlightPlan.InitialLocation,
                                                                                          flight.FlightPlan.Segments.ToArray()).Latitude,
                             Passengers = flight.FlightPlan.Passengers,
                             CompanyName = flight.FlightPlan.CompanyName,
                             DateTime = flight.FlightPlan.InitialLocation.DateTime.ToString(),
                             IsExternal = flight.IsExternal
                         };
            var addedOutput = output.ToList();
            var flightsData = new List<FlightDTO>();

            if ((sync_all != null && sync_all == true) ||
                ((this.Request != null) && this.Request.QueryString.ToString().Contains("sync_all")))
            {
                var servers = dataBaseCalls.GetServers(_context);

                foreach (var server in servers)
                {
                    flightsData = GetAllFlightsFromServer(flightsData, server);
                }
            }

            addedOutput.AddRange(flightsData);
            return new OkObjectResult(addedOutput);
        }

        public virtual List<FlightDTO> GetAllFlightsFromServer(List<FlightDTO> flightsData, Server server)
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(server.ServerURL + "api/Flights?relative_to="
                    + DateTime.Now.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'")).Result;
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
            return flightsData;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await dataBaseCalls.RemoveFlight(_context, id);
            return NoContent();
        }

    }
}
