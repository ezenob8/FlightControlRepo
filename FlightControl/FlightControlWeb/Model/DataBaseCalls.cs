using FlightControlWeb.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Model
{
    static public class DataBaseCalls
    {
        public static IQueryable<FlightPlanDTO> FindFlightPlanId(FlightPlanDBContext _context, string id)
        {
            var flightPlans = _context.FlightPlans.Include(item => item.Flight).Include(item => item.InitialLocation)
                .Include(item => item.Segments).Where(item => id == null || item.Flight.FlightIdentifier == id).Take(1);
            var output = from flightPlan in flightPlans
                         select new FlightPlanDTO
                         {
                             Passengers = flightPlan.Passengers,
                             CompanyName = flightPlan.CompanyName,
                             InitialLocation = new InitialLocationDTO
                             {
                                 Longitude = flightPlan.InitialLocation.Longitude,
                                 Latitude = flightPlan.InitialLocation.Latitude,
                                 DateTime = flightPlan.InitialLocation.DateTime.ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss"),
                             },
                             Segments = (from location in flightPlan.Segments
                                         select new LocationDTO
                                         {
                                             Longitude = location.Longitude,
                                             Latitude = location.Latitude,
                                             TimeSpanSeconds = location.TimeSpanSeconds
                                         }).ToArray()
                         };
            return output;
        }

        public static void AddAFlightPlanAndAFlight(FlightPlanDBContext _context, FlightPlan flightPlan, Flight flight)
        {
            _context.Add(flightPlan);
            _context.Add(flight);
            _context.SaveChanges();
        }

        public static void RemoveFlight(FlightPlanDBContext _context, Flight flight, FlightPlan flightPlan)
        {
            _context.FlightPlans.Remove(flightPlan);
            _context.Flight.Remove(flight);
            _context.SaveChanges();
        }

        public static IEnumerable<Flight> GetFlights(FlightPlanDBContext _context, DateTime relative_to)
        {
            return _context.Flight.Include(item => item.FlightPlan).Include(item => item.FlightPlan.InitialLocation).Include(item => item.FlightPlan.Segments).ToList().Where(filter => filter.FlightPlan.InitialLocation.DateTime.ToLocalTime() <= relative_to && relative_to <= filter.FlightPlan.EndDateFlight.ToLocalTime());
        }

        public static List<Server> GetServers(FlightPlanDBContext _context)
        {
            return _context.Servers.ToList();
        }

        public static IQueryable<ServerDTO> GetListOfServers(FlightPlanDBContext _context)
        {
            return from server in _context.Servers
                   select new ServerDTO
                   {
                       ServerId = server.ServerId,
                       ServerURL = server.ServerURL
                   };
        }

        public static void AddServer(FlightPlanDBContext _context, Server server)
        {
            _context.Add(server);
            _context.SaveChanges();
        }

        public static void DeleteServer(FlightPlanDBContext _context, Server server)
        {
            _context.Servers.Remove(server);
            _context.SaveChanges();
        }
    }
}
