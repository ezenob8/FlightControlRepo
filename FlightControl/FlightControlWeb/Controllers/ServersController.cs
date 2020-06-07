using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.DTO;
using FlightControlWeb.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace FlightControlWeb.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ServersController : ControllerBase
    {
        private readonly ILogger<FlightPlanController> _logger;
        private readonly FlightPlanDBContext _context;

        public ServersController(ILogger<FlightPlanController> logger, FlightPlanDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return base.Ok(DataBaseCalls.GetListOfServers(_context));
        }

        [HttpPost]
        public ActionResult Post(ServerDTO serverDTO)
        {
            Server server = new Server
            {
                ServerId = serverDTO.ServerId,
                ServerURL = serverDTO.ServerURL
            };
            DataBaseCalls.AddServer(_context, server);

            return Ok(server);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            Server server = _context.Servers.Where(item => item.ServerId == id).First();
            DataBaseCalls.DeleteServer(_context, server);
            return NoContent();
        }
    }
}
