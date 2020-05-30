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
    public class FlightsController : ControllerBase
    {
        private readonly ILogger<FlightPlanController> _logger;
        private readonly FlightPlanDBContext _context;

        public FlightsController(ILogger<FlightPlanController> logger, FlightPlanDBContext context)
        //public FlightPlansController(ILogger<FlightPlansController> logger)
        {
            _logger = logger;
            _context = context;
        }

        //public IEnumerable<FlightPlan> Get()
        [HttpGet]
        public ActionResult Get(DateTime relative_to, bool? sync_all)
        {
            var flights = _context.Flight.Include(item => item.FlightPlan).Include(item => item.FlightPlan.InitialLocation).ToList().Where(item => item.DateTime.Day == relative_to.Day &&
                                                    item.DateTime.Month == relative_to.Month &&
                                                    item.DateTime.Year == relative_to.Year &&
                                                    item.DateTime.Hour == relative_to.Hour &&
                                                    item.DateTime.Minute == relative_to.Minute &&
                                                    (!sync_all.HasValue || (sync_all.HasValue && item.IsExternal)) );
            var output = from flight in flights select new FlightDTO { FlightIdentifier=flight.FlightIdentifier,
                                                                       Longitude=flight.Longitude,
                                                                       Latitude=flight.Latitude,
                                                                       Passengers=flight.Passengers,
                                                                       CompanyName=flight.CompanyName,
                                                                       DateTime=flight.DateTime,
                                                                       IsExternal =flight.IsExternal};
            return Ok(output);
        }

        [HttpPost]
        public ActionResult Post(ServerDTO serverDTO)
        {
            using (var db = new FlightPlanDBContext())
            {
                Server server = new Server
                {
                    ServerId = "12345",
                    ServerURL = "www.server.com"
                };

                db.Add(server);
                db.SaveChanges();
            }
            return Ok(null);
            //return Ok(_context.FlightPlans.Include(flight => flight.InitialLocation));
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
            //return Ok(_context.FlightPlans.Include(flight => flight.InitialLocation));
        }

    }
}
