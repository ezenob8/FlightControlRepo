using FlightControlWeb.Model.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightControlWeb.Model
{
    public class Flight : Entity
    {
        public int Passengers { get; set; }
        
        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        [JsonProperty("initial_location")]
        public virtual InitialLocation InitialLocation { get; set; }

        public ICollection<Location> Segments { get; set; }

        // Data base context- not used to get data
        public FlightPlan FlightPlan { get; set; }
    }
}
