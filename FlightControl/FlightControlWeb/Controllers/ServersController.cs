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
        private readonly ILogger<FlightPlanController> _logger;
        private readonly FlightPlanDBContext _context;

        public ServersController(ILogger<FlightPlanController> logger, FlightPlanDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public ActionResult Get()
        {
            using (var db = new FlightPlanDBContext())
            {
                // Create Server
                Console.WriteLine("Inserting a new server");

                ServerDTO server = new ServerDTO
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

        [HttpPost]
        public ActionResult Post(ServerDTO server)
        {
            using (var db = new FlightPlanDBContext())
            {
                db.Add(server);
                db.SaveChanges();
            }
            return Ok(null);
            //return Ok(_context.FlightPlans.Include(flight => flight.InitialLocation));
        }
    }
}
