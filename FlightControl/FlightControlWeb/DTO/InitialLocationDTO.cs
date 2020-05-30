using FlightControlWeb.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightControlWeb.DTO
{
    public class InitialLocationDTO
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        [JsonProperty("date_time")]
        public string DateTime { get; set; }
    }
}
