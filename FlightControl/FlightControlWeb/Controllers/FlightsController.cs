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
    [Route("/api/[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly ILogger<FlightsController> _logger;
        private readonly FlightDBContext _context;

        public FlightsController(ILogger<FlightsController> logger, FlightDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        
        [HttpGet]
         //Check module
        public ActionResult Get()
        {
            var qa = _context.Flights.Include(flight => flight.Segments);
            return Ok(qa);
        }

        private Flight FlightTime(Flight flight, DateTime relative_to)
        {
            foreach(var seg in flight.Segments)
            {
                // We need to check that: endTime> relative_to> startTime
                // But segmant does not have this data yet.
                return flight;
            }
            return null;
        }

        public ActionResult Get(DateTime relative_to)
        {
            //Only for data from the local server data

            // The sql quary should ask the DB to get only the segmantes in the right time

            // way 1
            var rawData = _context.Flights.FromSqlRaw("" /*SELECT * FROM flights WHERE (relative_to)>startTime AND (relative_to)<endTime*/);

            //way 2
            _context.Flights.Include(flight=> FlightTime(flight,relative_to));


            if (rawData.Count() == 0)
                return NoContent();

            return Ok(_context.Flights.Include(flight => flight.Segments));
        }
        
        public ActionResult Get(DateTime relative_to, bool sync_all)
        {
            //Data from all servers



            using (var db = new FlightDBContext())
            {
                // The sql quary should ask the DB to get only the segmantes in the right time

                var rawData = _context.Flights.FromSqlRaw(""
                     /*SELECT * FROM flights WHERE (relative_to)>startTime AND (relative_to)<endTime 
                       UNION
                       SELECT * FROM externalFlights WHERE (relative_to)>startTime AND (relative_to)<endTime 
                      */);

            }
                //return Ok(null);
                return Ok(_context.Flights.Include(flight => flight.InitialLocation));
        }

        
    }
}
