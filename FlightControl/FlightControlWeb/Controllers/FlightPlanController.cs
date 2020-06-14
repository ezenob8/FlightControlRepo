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
        public FlightPlanController(ILogger<FlightPlanController> logger, FlightPlanDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("{id?}")]
        public async Task<ActionResult<FlightPlanDTO>> Get(string id)
        {
            var flightPlans = DataBaseCalls.FindFlightPlanId(_context, id);
            var output = from flightPlan in flightPlans
                         select new FlightPlanDTO
                         {
                             Passengers = flightPlan.Passengers,
                             CompanyName = flightPlan.CompanyName,
                             InitialLocation = FactoryInitialLocationDTO(flightPlan),
                             Segments = (from location in flightPlan.Segments
                                         select FactoryLocationDTO(location)).ToArray()
                         };
            var outputAsync = await output.FirstAsync<FlightPlanDTO>();
            return outputAsync;
        }

        /// <summary>
        /// Map Location Entity to LocationDTO
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private static LocationDTO FactoryLocationDTO(Location location)
        {
            return new LocationDTO
            {
                Longitude = location.Longitude,
                Latitude = location.Latitude,
                TimeSpanSeconds = location.TimeSpanSeconds
            };
        }

        /// <summary>
        /// Map InitialLocation Entity to InitialLocationDTO
        /// </summary>
        /// <param name="flightPlan"></param>
        /// <returns></returns>
        private static InitialLocationDTO FactoryInitialLocationDTO(FlightPlan flightPlan)
        {
            return new InitialLocationDTO
            {
                Longitude = flightPlan.InitialLocation.Longitude,
                Latitude = flightPlan.InitialLocation.Latitude,
                DateTime = flightPlan.InitialLocation.DateTime.ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss"),
            };
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post(FlightPlanDTO flightPlanDTO)
        {
            FlightPlan flightPlan = new FlightPlan
            {
                Passengers = flightPlanDTO.Passengers,
                CompanyName = flightPlanDTO.CompanyName,
                InitialLocation = FactoryInitialLocation(flightPlanDTO),
                Segments = (FactoryLocation(flightPlanDTO)).ToList()
            };
            string lastId = !_context.FlightPlans.Include(item => item.Flight)
                                                   .OrderByDescending(
                                                    item => item.Flight.FlightIdentifier).Any() ? "AAAA-0000" 
                                                    : _context.FlightPlans.Include(item => item.Flight)
                                                        .OrderByDescending(item => item.Flight.FlightIdentifier)
                                                    .First().Flight.FlightIdentifier;
            Flight flight = FactoryFlight(flightPlan, lastId);
            await DataBaseCalls.AddAFlightPlanAndAFlight(_context, flightPlan, flight);
            return Created("", null);
        }

        /// <summary>
        /// Map FlightDTO to Flight Entity
        /// </summary>
        /// <param name="flightPlanDTO"></param>
        /// <returns></returns>
        private static Flight FactoryFlight(FlightPlan flightPlan, string lastId)
        {
            return new Flight
            {
                FlightPlan = flightPlan,
                FlightPlanId = flightPlan.Id,
                FlightIdentifier = GenerateFlightId.GenerateFlightIdentifier(lastId),
                IsExternal = false
            };
        }

        /// <summary>
        /// Map LocationDTO to Location Entity
        /// </summary>
        /// <param name="flightPlanDTO"></param>
        /// <returns></returns>
        private static IEnumerable<Location> FactoryLocation(FlightPlanDTO flightPlanDTO)
        {
            return from segment in flightPlanDTO.Segments
                   select new Location
                   {
                       Longitude = segment.Longitude,
                       Latitude = segment.Latitude,
                       TimeSpanSeconds = segment.TimeSpanSeconds
                   };
        }

        /// <summary>
        /// Map InitialLocationDTO to InitialLocation Entity
        /// </summary>
        /// <param name="flightPlanDTO"></param>
        /// <returns></returns>
        private static InitialLocation FactoryInitialLocation(FlightPlanDTO flightPlanDTO)
        {
            return new InitialLocation
            {
                Longitude = flightPlanDTO.InitialLocation.Longitude,
                Latitude = flightPlanDTO.InitialLocation.Latitude,
                DateTime = DateTimeOffset.Parse(flightPlanDTO.InitialLocation.DateTime).UtcDateTime
            };
        }
    }

}
