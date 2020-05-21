using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.DTO;
using FlightControlWeb.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightControlWeb.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class FlightPlanController : ControllerBase
    {
        private readonly ILogger<FlightsController> _logger;
        private readonly FlightDBContext _context;

        public FlightPlanController(ILogger<FlightsController> logger, FlightDBContext context)
        //public FlightsController(ILogger<FlightsController> logger)
        {
            _logger = logger;
            _context = context;
        }

        //public IEnumerable<Flight> Get()
        [HttpGet]
        public ActionResult Get(int id)
        {
            using (var db = new FlightDBContext())
            {
                //maybe try - catch
                FlightPlan flightPlane = db.Find<FlightPlan>(id);
            }

            return Ok(_context.Flights.Include(flightPlane => flightPlane.FlightPlan.Flight));
        }

        [HttpPost]
        public ActionResult Post(FlightPlanDTO flightPlanDTO)
        {
            using (var db = new FlightDBContext())
            {
                // Create Flight Plan
                Flight flight = new Flight
                {
                    Passengers = flightPlanDTO.Passengers,
                    CompanyName = flightPlanDTO.CompanyName,
                    InitialLocation = new InitialLocation
                    {
                        Longitude = flightPlanDTO.Longitude,
                        Latitude = flightPlanDTO.Latitude,
                        DateTime = flightPlanDTO.DateTime
                    },
                    //DTO does not have segment to give
                    Segments = null
                };

                db.Add(flight);

                FlightPlan newPlan = new FlightPlan
                {
                    //for context
                    FlightId = flight.Id,
                    //real data
                    Flight = flight,
                    FlightGuid = Guid.NewGuid(),
                    IsExternal = flightPlanDTO.IsExternal
                };

                db.Add(newPlan);

                //upload to db
                db.SaveChanges();
                return Ok(newPlan);
            }
        }
    }
}
