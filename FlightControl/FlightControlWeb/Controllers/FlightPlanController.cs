using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightPlanController : ControllerBase
    {
        private readonly FlightsDBContext _context;
        private readonly ILogger<FlightPlanController> _logger;
        private Dictionary<int, FlightPlan> _idToFlightPlan;

        FlightPlanController(ILogger<FlightPlanController> logger, FlightsDBContext context)
        {
            _context = context;
            _logger = logger;
            _idToFlightPlan = //data from server
        }

        // GET: api/FlightPlan/5
        [HttpGet("{id}", Name = "Get")]
        public ActionResult<FlightPlan> Get(int id)
        {
            bool isOk = _idToFlightPlan.TryGetValue(id, out FlightPlan item );//find
            if (!isOk)
            {
                return NotFound(id);
            }

            return Ok(item);
        }

        // POST: api/FlightPlan
        [HttpPost("{id}")]
        public void Post([FromBody] FlightPlan item)
        {
            item.id = _idToFlightPlan.Count;
            _idToFlightPlan[item.id] = item;
            
            return CreatedAtAction(actionName: "GetFlightPlan", new { id = item.id, item });
        }


    }
}
