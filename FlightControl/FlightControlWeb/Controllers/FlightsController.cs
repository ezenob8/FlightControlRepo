using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightControlWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly ILogger<FlightsController> _logger;
        private readonly FlightsDBContext _context;

        public FlightsController(ILogger<FlightsController> logger, FlightsDBContext context)
        //public FlightsController(ILogger<FlightsController> logger)
        {
            _logger = logger;
            _context = context;
        }

        //public IEnumerable<Flight> Get()
        [HttpGet]
        public ActionResult Get()
        {
        //Flight[] array = new Flight[1];

        //array[0] = new Flight
        //{
        //    Passengers = 216,
        //    CompanyName = "SwissAir",
        //    InitialLocation = new Location
        //    {
        //        Longitude = 33.244,
        //        Latitude = 31.12,
        //        DateTime = DateTime.Now
        //    },
        //    Segments = new Segment[1] {
        //                                new Segment
        //                                {
        //                                    Longitude = 33.234 ,
        //                                    Latitude = 31.18 ,
        //                                    TimeSpanSeconds = 650
        //                                }
        //                               }
        //};

        //return Ok(null);
            return Ok(_context.Flight.Include(flight => flight.Location));
        }
    }
}
