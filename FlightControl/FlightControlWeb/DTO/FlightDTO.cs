using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightControlWeb.DTO
{
    public class FlightDTO
    {
        [JsonProperty("flight_id")]
        public string FlightIdentifier { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int Passengers { get; set; }

        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        [JsonProperty("date_time")]
        public string DateTime { get; set; }

        [JsonProperty("is_external")]
        public bool IsExternal { get; set; }

        public string ServerId { get; set; }
    }
}
