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
        public async Task<ActionResult<ServerDTO>> Get()
        {
            var servers = await DataBaseCalls.GetListOfServers(_context);

            return Ok(from server in servers
                      select new ServerDTO
                      {
                          ServerId = server.ServerId,
                          ServerURL = server.ServerURL
                      });
        }

        [HttpPost]
        public async Task<IActionResult> Post(ServerDTO serverDTO)
        {
            Server server = new Server
            {
                ServerId = serverDTO.ServerId,
                ServerURL = serverDTO.ServerURL
            };
            await DataBaseCalls.AddServer(_context, server);

            return Created("", null);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await DataBaseCalls.DeleteServer(_context, id);
            return NoContent();
        }

    }
}
