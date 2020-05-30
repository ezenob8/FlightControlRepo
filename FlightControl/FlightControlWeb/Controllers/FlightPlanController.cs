using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using FlightControlWeb.Algorithms;
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

        [HttpGet("{id?}")]
        public ActionResult Get(string? id)
        {
            //using (
            //    var db = new FlightPlanDBContext())
            //{
            //    // Create FlightPlan
            //    Console.WriteLine("Inserting a new flight");
            //    FlightPlan flightPlan1 = new FlightPlan
            //    {

            //        Passengers = 266,
            //        CompanyName = "Aerolineas",
            //        InitialLocation = new InitialLocation
            //        {

                    // are Segments kept as lists?
                    // var segments = db.Find<Location>(longId);
                    // found.Segments ? segments

            var flightPlans = _context.FlightPlans.Include(item => item.Flight).Include(item => item.InitialLocation).Include(item => item.Segments).Where(item => id == null || item.Flight.FlightIdentifier == id).Take(1);
            var output = from flightPlan in flightPlans
                         select new FlightPlanDTO
                         {
                             Passengers = flightPlan.Passengers,
                             CompanyName = flightPlan.CompanyName,
                             InitialLocation = new InitialLocationDTO
                             {
                                 Longitude = flightPlan.InitialLocation.Longitude,
                                 Latitude = flightPlan.InitialLocation.Latitude,
                                 DateTime = flightPlan.InitialLocation.DateTime.ToString() + "Z",
                             },
                             Segments = (from location in flightPlan.Segments select new LocationDTO
                             {
                                 Longitude = location.Longitude,
                                 Latitude = location.Latitude,
                                 TimeSpanSeconds = location.TimeSpanSeconds
                             }).ToArray()
                         };
            return Ok(output);

            //return Ok(_context.FlightPlans.Include(flight => flight.InitialLocation));
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
                        DateTime = Convert.ToDateTime(flightPlanDTO.InitialLocation.DateTime.Replace('T', ' ').Replace('Z', ' '))                
                    },
                    Segments = (from segment in flightPlanDTO.Segments
                               select new Location { Longitude = segment.Longitude,
                                                     Latitude = segment.Latitude,
                                                     TimeSpanSeconds = segment.TimeSpanSeconds
                                                   }).ToList()
                };

                db.Add(flightPlan);
                string lastId = !db.FlightPlans.Include(item => item.Flight).OrderByDescending(item => item.Flight.FlightIdentifier).Any() ? "AAAA-0000" : db.FlightPlans.Include(item => item.Flight).OrderByDescending(item => item.Flight.FlightIdentifier).First().Flight.FlightIdentifier;
                Flight flight = new Flight
                {
                    FlightPlan = flightPlan,
                    FlightPlanId = flightPlan.Id,
                    FlightIdentifier = GenerateFlightId.GenerateFlightIdentifier(lastId),
                    IsExternal = false
                };

                db.Add(flight);

                db.SaveChanges();
            }


            return Created("", null);
        }

    }

}
