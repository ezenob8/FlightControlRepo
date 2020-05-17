using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FlightControlWeb.Model
{
    public partial class Flight
    {
        public long Id { get; set; }
        public int Passengers { get; set; }

        [JsonProperty("company_name")]
        public string CompanyName { get; set; }
        public long LocationId { get; set; }

        [JsonProperty("initial_location")]
        public virtual Location Location { get; set; }
    }
}
