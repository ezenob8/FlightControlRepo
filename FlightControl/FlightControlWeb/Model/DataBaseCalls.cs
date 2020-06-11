using FlightControlWeb.DTO;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FlightControlWeb.Model
{
    public class DataBaseCalls
    {
        // Find the last id in the context given
        public IQueryable<FlightPlan> FindFlightPlanId(FlightPlanDBContext _context, string id)
        {
            return _context.FlightPlans.Include(item => item.Flight).Include(item => item.InitialLocation)
                .Include(item => item.Segments).Where(item => id == null || item.Flight.FlightIdentifier == id).Take(1);
        }

        // Send flightPlan and flight to the context given
        public async Task<int> AddAFlightPlanAndAFlight(FlightPlanDBContext _context, FlightPlan flightPlan, Flight flight)
        {
            _context.Add(flightPlan);
            _context.Add(flight);
            return await _context.SaveChangesAsync();
        }

        // Remove a flight with the same ID as "id" from the ontext given
        public async Task<int> RemoveFlight(FlightPlanDBContext _context, string id)
        {
            // Finding the flight
            Flight flight = _context.Flight.Include(item => item.FlightPlan)
                .Where(item => item.FlightIdentifier == id).First();
            FlightPlan flightPlan = flight.FlightPlan;
            // Removing the associated flight plan
            _context.FlightPlans.Remove(flightPlan);
            _context.Flight.Remove(flight);
            return await _context.SaveChangesAsync();
        }

        // Get all flights that are active acoording to the time of reletive_to from the context
        public IEnumerable<Flight> GetFlights(FlightPlanDBContext _context, DateTime relative_to)
        {
            return _context.Flight.Include(item => item.FlightPlan).Include(item => item.FlightPlan.InitialLocation)
                .Include(item => item.FlightPlan.Segments).ToList()
                .Where(filter => filter.FlightPlan.InitialLocation.DateTime.ToLocalTime()
                <= relative_to && relative_to <= filter.FlightPlan.EndDateFlight.ToLocalTime());
        }


        // Get all servers from the context
        public List<Server> GetServers(FlightPlanDBContext _context)
        {
            return _context.Servers.ToList();
        }

        // Get all servers from the context asynchronously
        public async Task<List<Server>> GetListOfServers(FlightPlanDBContext _context)
        {
            return await _context.Servers.ToListAsync();
        }

        // Adding a server to the given context
        public async Task<int> AddServer(FlightPlanDBContext _context, Server server)
        {
            _context.Add(server);
            return await _context.SaveChangesAsync();
        }

        // Deleting a server with the ID "id" from context
        public async Task<int> DeleteServer(FlightPlanDBContext _context, string id)
        {
            Server server = _context.Servers.Where(item => item.ServerId == id).First();
            _context.Servers.Remove(server);
            return await _context.SaveChangesAsync();
        }

        // Return the last used ID in the context
        public virtual string FindLastID(FlightPlanDBContext db)
        {
            return !db.FlightPlans.Include(item => item.Flight).OrderByDescending(item => item.Flight.FlightIdentifier).Any()
                ? "AAAA-0000" : db.FlightPlans.Include(item => item.Flight).OrderByDescending(item => item.Flight.FlightIdentifier).First()
                .Flight.FlightIdentifier;
        }

        // Call to another database and get all relavent flights
        public virtual List<FlightDTO> GetAllFlightsFromServer(List<FlightDTO> flightsData, Server server)
        {
            using (var client = new HttpClient())
            {
                // Connecting with the server and getting all relavent flights
                var response = client.GetAsync(server.ServerURL + "api/Flights?relative_to="
                    + DateTime.Now.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'")).Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;

                    string responseString = responseContent.ReadAsStringAsync().Result;
                    var array = JsonConvert.DeserializeObject(responseString);
                    flightsData.AddRange(((JArray)array).Select(x => new FlightDTO
                    {
                        FlightIdentifier = (string)x["flight_id"],
                        CompanyName = (string)x["company_name"],
                        Longitude = (double)x["longitude"],
                        Latitude = (double)x["longitude"],
                        Passengers = (int)x["passengers"],
                        DateTime = ((DateTime)x["date_time"]).ToLongDateString(),
                        IsExternal = true
                    }).ToList());
                }
            }
            return flightsData;
        }
    }
}
