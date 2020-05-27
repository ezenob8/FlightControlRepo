using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
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

        public FlightPlanController(ILogger<FlightPlanController> logger, FlightPlanDBContext context)
        //public FlightPlansController(ILogger<FlightPlansController> logger)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public ActionResult Get()
        {
            //FlightPlan[] array = new FlightPlan[1];

            //array[0] = new FlightPlan
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

            using (
                var db = new FlightPlanDBContext())
            {
                // Create FlightPlan
                Console.WriteLine("Inserting a new flight");
                FlightPlan flightPlan = new FlightPlan
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

                db.Add(flightPlan);

                db.Add(new Flight
                {
                    FlightPlan = flightPlan,
                    FlightPlanId = flightPlan.Id,
                    FlightIdentifier = "",
                    IsExternal = false
                });

                db.SaveChanges();
            }
            return Ok(_context.FlightPlans.Include(flight => flight.InitialLocation));
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public CreatedResult Post(FlightPlanDTO flightPlanDTO)
        {
            using (var db = new FlightPlanDBContext())
            {
                // Create FlightPlan Plan
                FlightPlan flightPlan = new FlightPlan
                {

                    Passengers = flightPlanDTO.Passengers,
                    CompanyName = flightPlanDTO.CompanyName,
                    InitialLocation = new InitialLocation
                    {

                        Longitude = flightPlanDTO.InitialLocation.Longitude,
                        Latitude = flightPlanDTO.InitialLocation.Latitude,
                        DateTime = flightPlanDTO.InitialLocation.DateTime
                    },
                    Segments = null
                };

                db.Add(flightPlan);

                Flight flight = new Flight
                {
                    FlightPlan = flightPlan,
                    FlightPlanId = flightPlan.Id,
                    FlightIdentifier = "",
                    IsExternal = false
                };

                db.Add(flight);

                db.SaveChanges();
            }


            return Created("", null);
        }

    }

}
