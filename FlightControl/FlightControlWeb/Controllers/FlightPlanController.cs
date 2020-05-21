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
    [Route("api/[controller]")]
    public class FlightPlanController : ControllerBase
    {
        private readonly FlightDBContext _context;
        private readonly ILogger<FlightPlanController> _logger;

        private Dictionary<long, FlightPlan> _idToFlightPlan;

        FlightPlanController(ILogger<FlightPlanController> logger, FlightDBContext context)
        {
            _context = context;
            _logger = logger;
            
        }

        // GET: api/FlightPlan/5
        [HttpGet("{id}", Name = "Get")]
        public ActionResult<FlightPlan> Get(int id)
        {
            Flight flight = _context.Find<Flight>(id);
            

            return Ok(flight.FlightPlan);
        }

        // POST: api/FlightPlan
        [HttpPost("{id}")]
        public ActionResult Post([FromBody] FlightPlan item)
        {
            item.Id = _idToFlightPlan.Count;
            _idToFlightPlan[item.Id] = item;
            
            // Sending to DB

            // uncomment when flight plans are in DB
            //_context.Flights.Add(item);

            
            return CreatedAtAction(actionName: "GetFlightPlan", new { id = item.Id, item });
        }


    }
}
