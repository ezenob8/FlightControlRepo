using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FlightControlWeb.Model
{
    public partial class Location
    {
        public Location()
        {
            Flight = new HashSet<Flight>();
        }

        public long Id { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        [JsonProperty("date_time")]
        public DateTime DateTime { get; set; }

        public virtual ICollection<Flight> Flight { get; set; }
    }
}
