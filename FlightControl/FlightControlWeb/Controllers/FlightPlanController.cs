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
        private Dictionary<int, FlightPlan> _idToFlightPlan;

        FlightPlanController(ILogger<FlightPlanController> logger, FlightDBContext context)
        {
            _context = context;
            _logger = logger;
            
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
        public ActionResult Post([FromBody] FlightPlan item)
        {
            item.FlightGuid = new Guid();
            
            //_idToFlightPlan[item.FlightGuid] = item;
            
            return CreatedAtAction(actionName: "GetFlightPlan", new { id = item.FlightGuid, item });
        }


    }
}
