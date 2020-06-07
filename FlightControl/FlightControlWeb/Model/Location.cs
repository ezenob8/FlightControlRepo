using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightControlWeb.Model
{
    public class Location : Coordinates
    {
        public long Id { get; set; }
        [JsonProperty("timespan_seconds")]
        public int TimeSpanSeconds { get; set; }
        public long FlightPlanId { get; set; }
        public FlightPlan FlightPlan { get; set; }
    }
}
