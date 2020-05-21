using FlightControlWeb.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightControlWeb.DTO
{
    // A version of flight that are valid
    public class FlightDTO
    {
        public int Passengers { get; set; }

        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        [JsonProperty("initial_location")]
        public virtual InitialLocation InitialLocation { get; set; }
        public ICollection<Location> Segments { get; set; }
    }
}
