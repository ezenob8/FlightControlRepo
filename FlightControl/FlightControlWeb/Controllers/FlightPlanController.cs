using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using FlightControlWeb.Algorithms;
using FlightControlWeb.DTO;
using FlightControlWeb.Model;
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
        public FlightPlanDBContext _context;

        public FlightPlanController(ILogger<FlightPlanController> logger, FlightPlanDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("{id?}")]
        public ObjectResult Get(string id)
        {
            IQueryable<FlightPlanDTO> output = DataBaseCalls.FindFlightPlanId(_context, id);

            return Ok(output.First());
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public CreatedResult Post(FlightPlanDTO flightPlanDTO)
        {
            FlightPlan flightPlan = CreateFlightPlan(flightPlanDTO);

            string lastId = !_context.FlightPlans.Include(item => item.Flight)
                .OrderByDescending(item => item.Flight.FlightIdentifier).Any() ? "AAAA-0000"
                : _context.FlightPlans.Include(item => item.Flight)
                .OrderByDescending(item => item.Flight.FlightIdentifier)
                .First().Flight.FlightIdentifier;
            // Create a new flight
            Flight flight = CreateFlight(flightPlan, lastId);

            DataBaseCalls.AddAFlightPlanAndAFlight(_context, flightPlan, flight);

            return Created("", flightPlanDTO);
        }

        private Flight CreateFlight(FlightPlan flightPlan, string lastId)
        {
            return new Flight
            {
                FlightPlan = flightPlan,
                FlightPlanId = flightPlan.Id,
                FlightIdentifier = GenerateFlightId.GenerateFlightIdentifier(lastId),
                IsExternal = false
            };
        }

        private FlightPlan CreateFlightPlan(FlightPlanDTO flightPlanDTO)
        {
            // Create FlightPlan Plan
            return new FlightPlan
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
        }
    }

}
