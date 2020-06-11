using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using FlightControlWeb.Algorithms;
using FlightControlWeb.CORSManager;
using FlightControlWeb.DTO;
using FlightControlWeb.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightControlWeb.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class FlightPlanController : ControllerBase
    {
        private readonly ILogger<FlightPlanController> _logger;
        private readonly FlightPlanDBContext _context;
        private DataBaseCalls dataBaseCalls;

        public FlightPlanController(ILogger<FlightPlanController> logger, FlightPlanDBContext context)
        {
            _logger = logger;
            _context = context;
            dataBaseCalls = new DataBaseCalls();
        }

        public void SetDataBaseCalls(DataBaseCalls dbc)
        {
            dataBaseCalls = dbc;
        }


        [HttpGet("{id?}")]
        public async Task<ActionResult<FlightPlanDTO>> Get(string id)
        {
            var flightPlans = dataBaseCalls.FindFlightPlanId(_context, id);
            var output = from flightPlan in flightPlans
                         select new FlightPlanDTO
                         {
                             Passengers = flightPlan.Passengers,
                             CompanyName = flightPlan.CompanyName,
                             InitialLocation = new InitialLocationDTO
                             {
                                 Longitude = flightPlan.InitialLocation.Longitude,
                                 Latitude = flightPlan.InitialLocation.Latitude,
                                 DateTime = flightPlan.InitialLocation.DateTime.ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss"),
                             },
                             Segments = (from location in flightPlan.Segments
                                         select new LocationDTO
                                         {
                                             Longitude = location.Longitude,
                                             Latitude = location.Latitude,
                                             TimeSpanSeconds = location.TimeSpanSeconds
                                         }).ToArray()
                         };
            try
            {
                var outputAsync = await output.FirstAsync<FlightPlanDTO>();
                return outputAsync;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post(FlightPlanDTO flightPlanDTO)
        {
            using (var db = new FlightPlanDBContext())
            {
                // Create FlightPlan Plan
                FlightPlan flightPlan = new FlightPlan
                {

                    Passengers = flightPlanDTO.Passengers,
                    CompanyName = flightPlanDTO.CompanyName,
                    InitialLocation = new InitialLocation
                    {

                        Longitude = flightPlanDTO.InitialLocation.Longitude,
                        Latitude = flightPlanDTO.InitialLocation.Latitude,
                        DateTime = DateTimeOffset.Parse(flightPlanDTO.InitialLocation.DateTime).UtcDateTime
                    },
                    Segments = (from segment in flightPlanDTO.Segments
                                select new Location
                                {
                                    Longitude = segment.Longitude,
                                    Latitude = segment.Latitude,
                                    TimeSpanSeconds = segment.TimeSpanSeconds
                                }).ToList()
                };
                string lastId = dataBaseCalls.FindLastID(db);
                Flight flight = new Flight
                {
                    FlightPlan = flightPlan,
                    FlightPlanId = flightPlan.Id,
                    FlightIdentifier = GenerateFlightId.GenerateFlightIdentifier(lastId),
                    IsExternal = false
                };

                await dataBaseCalls.AddAFlightPlanAndAFlight(_context, flightPlan, flight);
            }

            return Created("", null);
        }


    }

}
