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
    public class ServersController : ControllerBase
    {
        private readonly ILogger<FlightsController> _logger;
        private readonly FlightDBContext _context;

        public ServersController(ILogger<FlightsController> logger, FlightDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public ActionResult Get()
        {
            using (var db = new FlightDBContext())
            {
                //Go to DB and get the external servers list
            }
            return Ok();
            //return Ok(_context.Flights.Include(flight => flight.InitialLocation));
        }

        [HttpPost]
        public ActionResult Post(Server server)
        {
            using (var db = new FlightDBContext())
            {
                db.Add(server);
                db.SaveChanges();
                return Ok(server);
            }
            
            //return Ok(_context.Flights.Include(flight => flight.InitialLocation));
        }
        [HttpDelete]
        public ActionResult Delete(Server server)
        {
            using (var db = new FlightDBContext())
            {
                db.Remove(server);
                db.SaveChanges();
                return NoContent();
            }
        }
    }
}
