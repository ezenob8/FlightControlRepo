using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FlightControlWeb.Model
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServersController : ControllerBase
    {
        private readonly FlightsDBContext _context;
        private readonly ILogger<ServersController> _logger;
        private Dictionary<int, Servers> _idToServer;

        ServersController(ILogger<ServersController> logger, FlightsDBContext context)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Servers
        [HttpGet]
        public ActionResult<Servers> Get()
        {
            // Maybe check if null
            return Ok(_idToServer.Values.ToList<Servers>());
        }

        // POST: api/Servers
        [HttpPost]
        public ActionResult Post([FromBody] Servers server)
        {
            //Add new Server
            server.id = _idToServer.Count;
            _idToServer[server.id] = server;
            return CreatedAtAction(actionName: "GetServer", new { id = server.id }, server);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public NoContentResult Delete(int id)
        {
            _idToServer.Remove(id);
            return NoContent();
        }
    }
}
