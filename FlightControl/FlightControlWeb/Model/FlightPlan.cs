using FlightControlWeb.Model.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightControlWeb.Model
{
    public class FlightPlan : Entity
    {
        // unique to this class
        public Guid FlightGuid { get; set; }

        [JsonProperty("is_external")]
        // unique to this class
        public bool IsExternal { get; set; }
        
        public int Passengers { get { return Flight.Passengers; } }
        public string CompanyName { get { return Flight.CompanyName; } }
        public DateTime DateTime { get { return Flight.InitialLocation.DateTime; } }
        public double Longitude { get { return Flight.InitialLocation.Longitude; } }
        public double Latitude { get { return Flight.InitialLocation.Latitude; } }

        // Data base context- not used to get data
        public long FlightId { get; set; }

        // Data base context- not used to get data
        public Flight Flight { get; set; }
    }
}
