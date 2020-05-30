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
            
            //return Ok(null);
            return Ok(from server in _context.Servers
                      select new ServerDTO
                      {
                          ServerId = server.ServerId,
                          ServerURL = server.ServerURL
                      });
        }

        [HttpPost]
        public ActionResult Post(ServerDTO serverDTO)
        {
            using (var db = new FlightPlanDBContext())
            {
                Server server = new Server
                {
                    ServerId = serverDTO.ServerId,
                    ServerURL = serverDTO.ServerURL
                };

                db.Add(server);
                try { db.SaveChanges(); }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    // 424 is Failed Dependency
                    return StatusCode(424);
                }
                ServerDTO ss = new ServerDTO
                {
                    ServerId = "1111",
                    ServerURL = "eeee"
                };
                return Ok(ss);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            Server server = _context.Servers.Where(item => item.ServerId == id).First();
            _context.Servers.Remove(server);
            _context.SaveChanges();
            return Ok(null);
            //return Ok(_context.FlightPlans.Include(flight => flight.InitialLocation));
        }
    }
}
