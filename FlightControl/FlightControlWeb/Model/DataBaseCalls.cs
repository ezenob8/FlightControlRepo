using FlightControlWeb.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Model
{
    public class DataBaseCalls
    {
        public IQueryable<FlightPlan> FindFlightPlanId(FlightPlanDBContext _context, string id)
        {
            return _context.FlightPlans.Include(item => item.Flight).Include(item => item.InitialLocation)
                .Include(item => item.Segments).Where(item => id == null || item.Flight.FlightIdentifier == id).Take(1);
        }

        public async Task<int> AddAFlightPlanAndAFlight(FlightPlanDBContext _context, FlightPlan flightPlan, Flight flight)
        {
            _context.Add(flightPlan);
            _context.Add(flight);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> RemoveFlight(FlightPlanDBContext _context, string id)
        {
            Flight flight = _context.Flight.Include(item => item.FlightPlan).Where(item => item.FlightIdentifier == id).First();
            FlightPlan flightPlan = flight.FlightPlan;
            _context.FlightPlans.Remove(flightPlan);
            _context.Flight.Remove(flight);
            return await _context.SaveChangesAsync();
        }

        public IEnumerable<Flight> GetFlights(FlightPlanDBContext _context, DateTime relative_to)
        {
            return _context.Flight.Include(item => item.FlightPlan).Include(item => item.FlightPlan.InitialLocation).Include(item => item.FlightPlan.Segments).ToList().Where(filter => filter.FlightPlan.InitialLocation.DateTime.ToLocalTime() <= relative_to && relative_to <= filter.FlightPlan.EndDateFlight.ToLocalTime());
        }

        public List<Server> GetServers(FlightPlanDBContext _context)
        {
            return _context.Servers.ToList();
        }

        public async Task<List<Server>> GetListOfServers(FlightPlanDBContext _context)
        {
            return await _context.Servers.ToListAsync();
        }

        public async Task<int> AddServer(FlightPlanDBContext _context, Server server)
        {
            _context.Add(server);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteServer(FlightPlanDBContext _context, string id)
        {
            Server server = _context.Servers.Where(item => item.ServerId == id).First();
            _context.Servers.Remove(server);
            return await _context.SaveChangesAsync();
        }

        public string FindLastID(FlightPlanDBContext db)
        {
            return !db.FlightPlans.Include(item => item.Flight).OrderByDescending(item => item.Flight.FlightIdentifier).Any()
                ? "AAAA-0000" : db.FlightPlans.Include(item => item.Flight).OrderByDescending(item => item.Flight.FlightIdentifier).First()
                .Flight.FlightIdentifier;
        }
    }
}
