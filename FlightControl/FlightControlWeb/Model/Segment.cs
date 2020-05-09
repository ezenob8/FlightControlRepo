using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Model
{
    public class Segment
    {
        public double Longitude { get; set; }

        public double Latitude { get; set; }

        [JsonProperty(PropertyName = "time_span_seconds")]
        public int TimeSpanSeconds { get; set; }
    }
}
