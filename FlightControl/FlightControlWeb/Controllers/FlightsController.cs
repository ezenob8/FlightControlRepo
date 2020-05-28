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
    public class FlightController : ControllerBase
    {
        private readonly ILogger<FlightPlanController> _logger;
        private readonly FlightPlanDBContext _context;

        public FlightController(ILogger<FlightPlanController> logger, FlightPlanDBContext context)
        //public FlightPlansController(ILogger<FlightPlansController> logger)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public ActionResult Get()
        {
            using (var db = new FlightPlanDBContext())
            {
                // Create FlightPlan
                Console.WriteLine("Inserting a new flight");
                FlightPlan flight = new FlightPlan
                {

                    Passengers = 266,
                    CompanyName = "Aerolineas",
                    InitialLocation = new InitialLocation
                    {

                        Longitude = 33.44,
                        Latitude = 33.45,
                        DateTime = DateTime.Now

                    },
                    Segments = new List<Location>
                    {
                        new Location
                        {
                            Latitude=40.0,
                            Longitude=40.1
                        }
                    }
                };

                db.Add(flight);

                db.Add(new Flight
                {
                    FlightPlan = flight,
                    FlightPlanId = flight.Id,
                    FlightIdentifier = "",
                    IsExternal = false
                });

                db.SaveChanges();
            }
            //return Ok(null);
            return Ok(_context.FlightPlans.Include(flight => flight.InitialLocation));
        }
    }
}
