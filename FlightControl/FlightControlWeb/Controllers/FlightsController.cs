using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FlightControlWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly ILogger<FlightsController> _logger;
        private readonly FlightDBContext _context;

        public FlightsController(ILogger<FlightsController> logger, FlightDBContext context)
        //public FlightsController(ILogger<FlightsController> logger)
        {
            _logger = logger;
            _context = context;
        }

        //public IEnumerable<Flight> Get()
        [HttpGet]
        public ActionResult Get(DateTime relative_to)
        {
            //Only for data from the local server data
            
            
            
            return Ok(_context.Flights.Include(flight => flight.Segments));
        }

        public ActionResult Get(DateTime relative_to, bool sync_all)
        {
            //Data from all servers



            using (var db = new FlightDBContext())
            {
                // Create Flight
                Console.WriteLine("");
            
            }
                //return Ok(null);
                return Ok(_context.Flights.Include(flight => flight.InitialLocation));
        }

        
    }
}
