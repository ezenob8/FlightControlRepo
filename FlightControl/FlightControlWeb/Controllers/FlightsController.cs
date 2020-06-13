using FlightControlWeb.Algorithms;
using FlightControlWeb.DTO;
using FlightControlWeb.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FlightControlWeb.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly ILogger<FlightPlanController> _logger;
        private readonly FlightPlanDBContext _context;
        private DataBaseCalls dataBaseCalls;

        public FlightsController(ILogger<FlightPlanController> logger, FlightPlanDBContext context)
        {
            _logger = logger;
            _context = context;
            dataBaseCalls = new DataBaseCalls();
        }

        [HttpGet]
        public OkObjectResult Get(DateTime relative_to, bool? sync_all)
        {
            IEnumerable<Flight> flights = null;
            try { flights = dataBaseCalls.GetFlights(_context, relative_to); }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
            IEnumerable<FlightDTO> output = ArrangeAllFlights(relative_to, flights);
            var addedOutput = output.ToList();
            var flightsData = new List<FlightDTO>();

            if ((sync_all != null && sync_all == true) ||
                ((this.Request != null) && this.Request.QueryString.ToString().Contains("sync_all")))
            {
                // We have a flag for sync_all
                List<Server> servers;
                try { servers = dataBaseCalls.GetServers(_context); }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                    return null;
                }
                foreach (var server in servers)
                {
                    flightsData = dataBaseCalls.GetAllFlightsFromServer(flightsData, server);
                }
            }

            addedOutput.AddRange(flightsData);
            return new OkObjectResult(addedOutput);
        }

        // Arranging all flight from DTO form to normal form
        private static IEnumerable<FlightDTO> ArrangeAllFlights(DateTime relative_to, IEnumerable<Flight> flights)
        {
            return from flight in flights
                   select new FlightDTO
                   {
                       FlightIdentifier = flight.FlightIdentifier,
                       Longitude = CalculateNewLocation
                       .Calculate(flight.FlightPlan.InitialLocation.DateTime.ToLocalTime(),
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
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try { await dataBaseCalls.RemoveFlight(_context, id); }
            catch (Exception ex) { _logger.LogError(ex.Message, ex); }
            return NoContent();
        }

        // For testing purpose
        public void SetDataBaseCalls(DataBaseCalls dbc)
        {
            dataBaseCalls = dbc;
        }
    }
}
