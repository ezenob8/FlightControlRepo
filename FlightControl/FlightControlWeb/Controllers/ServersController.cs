using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.CORSManager;
using FlightControlWeb.DTO;
using FlightControlWeb.Model;
using Microsoft.AspNetCore.Cors;
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
        private readonly DataBaseCalls dataBaseCalls;

        public ServersController(ILogger<FlightPlanController> logger, FlightPlanDBContext context)
        {
            _logger = logger;
            _context = context;
            dataBaseCalls = new DataBaseCalls();
        }

        [HttpGet]
        public async Task<ActionResult<ServerDTO>> Get()
        {
            List<Server> servers;
            try { servers = await dataBaseCalls.GetListOfServers(_context); }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }

            // Create a server DTO to return
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
            // Making a normal server drom DTO form
            Server server = new Server
            {
                ServerId = serverDTO.ServerId,
                ServerURL = serverDTO.ServerURL
            };
            try
            {
                await dataBaseCalls.AddServer(_context, server);
                return Created("", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try { await dataBaseCalls.DeleteServer(_context, id); }
            catch (Exception ex) { _logger.LogError(ex.Message, ex); }
            return NoContent();
        }

    }
}
