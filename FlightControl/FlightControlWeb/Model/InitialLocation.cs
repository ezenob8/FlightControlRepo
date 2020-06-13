using Newtonsoft.Json;
using System;

namespace FlightControlWeb.Model
{
    public class InitialLocation : Coordinates
    {
        public long Id { get; set; }
        [JsonProperty("date_time")]
        public DateTime DateTime { get; set; }
        public long FlightPlanId { get; set; }
    }
}
