using FlightControlWeb.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightControlWeb.DTO
{
    public class LocationDTO
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        [JsonProperty("timespan_seconds")]
        public int TimeSpanSeconds { get; set; }
    }
}
