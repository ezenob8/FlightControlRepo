﻿using System;
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

            

            return Ok(_context.Flight.Include(flight => flight.Location));
        }
        public ActionResult Get(DateTime relative_to, bool sync_all)
        {
            //Data from all servers



            using (var db = new FlightDBContext())
            {
                // Create Flight
                Console.WriteLine("Inserting a new flight");
                Flight flight = new Flight
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

                db.Add(new FlightPlan
                {
                    Flight = flight,
                    FlightId = flight.Id,
                    FlightGuid = Guid.NewGuid(),
                    IsExternal = false
                });

                db.SaveChanges();
            }
                //return Ok(null);
                return Ok(_context.Flights.Include(flight => flight.InitialLocation));
        }

        
    }
}
