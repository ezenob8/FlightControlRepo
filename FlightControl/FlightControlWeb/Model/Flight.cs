using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Model
{
    public class Flight
    {
        public int Passengers { get; set; }

        [JsonProperty("company_name")]
        public String CompanyName { get; set; }

        [JsonProperty(PropertyName = "initial_location")]
        public Location InitialLocation { get; set; }

        public Segment[] Segments { get; set; }
    }
}
