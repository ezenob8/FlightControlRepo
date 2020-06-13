using System;
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
        private readonly FlightPlanDBContext _context;
        private DataBaseCalls dataBaseCalls;
        private DbContextOptionsBuilder dbContextOptionsBuilder;

        public FlightPlanController(ILogger<FlightPlanController> logger, FlightPlanDBContext context)
        {
            _logger = logger;
            _context = context;
            dataBaseCalls = new DataBaseCalls();
            dbContextOptionsBuilder = null;
        }

        [HttpGet("{id?}")]
        public async Task<ActionResult<FlightPlanDTO>> Get(string id)
        {
            IQueryable<FlightPlan> flightPlans = null;
            try { flightPlans = dataBaseCalls.FindFlightPlanId(_context, id); }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
            IQueryable<FlightPlanDTO> output = ArrangeFlightPlan(flightPlans);
            try
            {
                var outputAsync = await output.FirstAsync<FlightPlanDTO>();
                return outputAsync;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }

        // Arrange the flight plans from DTO form to a normal flight plan
        private static IQueryable<FlightPlanDTO> ArrangeFlightPlan(IQueryable<FlightPlan> flightPlans)
        {
            return from flightPlan in flightPlans
                   select new FlightPlanDTO
                   {
                       Passengers = flightPlan.Passengers,
                       CompanyName = flightPlan.CompanyName,
                       InitialLocation = new InitialLocationDTO
                       {
                           Longitude = flightPlan.InitialLocation.Longitude,
                           Latitude = flightPlan.InitialLocation.Latitude,
                           DateTime = flightPlan.InitialLocation.DateTime.ToLocalTime()
                           .ToString("yyyy/MM/dd HH:mm:ss"),
                       },
                       Segments = (from location in flightPlan.Segments
                                   select new LocationDTO
                                   {
                                       Longitude = location.Longitude,
                                       Latitude = location.Latitude,
                                       TimeSpanSeconds = location.TimeSpanSeconds
                                   }).ToArray()
                   };
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post(FlightPlanDTO flightPlanDTO)
        {
            FlightPlanDBContext db = null;
            if (this.dbContextOptionsBuilder == null)
                // Normal case
                db = new FlightPlanDBContext();
            else
                // Testing or using this class in another project that uses other databse forms
                db = new FlightPlanDBContext(dbContextOptionsBuilder);
            FlightPlan flightPlan = DTOToNormalForm(flightPlanDTO);
            // Giving an ID to this flight
            string lastId;
            try { lastId = dataBaseCalls.FindLastID(db); }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return NotFound();
            }
            Flight flight = CreateAFlightFromFlightPlan(flightPlan, lastId);
            try
            {
                await dataBaseCalls.AddAFlightPlanAndAFlight(_context, flightPlan, flight);
                return Created("", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return NotFound();
            }
        }

        private static Flight CreateAFlightFromFlightPlan(FlightPlan flightPlan, string lastId)
        {
            return new Flight
            {
                FlightPlan = flightPlan,
                FlightPlanId = flightPlan.Id,
                FlightIdentifier = GenerateFlightId.GenerateFlightIdentifier(lastId),
                IsExternal = false
            };
        }

        // Create FlightPlan from the DTO version
        private static FlightPlan DTOToNormalForm(FlightPlanDTO flightPlanDTO)
        {
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

        // For testing purpose
        public void SetDataBaseCalls(DataBaseCalls dbc)
        {
            dataBaseCalls = dbc;
        }
        // For testing purpose
        public void SetOptionForDBContext(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            this.dbContextOptionsBuilder = dbContextOptionsBuilder;
        }
    }

}
