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
        [Route("/api/[controller]/{id}")]
        public ActionResult Get(int id)
        {
            using (
                var db = new FlightPlanDBContext())
            {
                try
                {
                    long longId = (long)id;
                    FlightPlan found = db.Find<FlightPlan>(longId);
                    Flight found_flight = db.Find<Flight>(found.Id);
                    found.Flight = found_flight;
                    found.InitialLocation = db.Find<InitialLocation>(longId);

                    // are Segments kept as lists?
                    // var segments = db.Find<Location>(longId);
                    // found.Segments ? segments

                    return Ok(found);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.InnerException.Message);
                    return NoContent();
                }
            }
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
