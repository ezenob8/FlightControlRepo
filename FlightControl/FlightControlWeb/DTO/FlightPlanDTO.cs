﻿using FlightControlWeb.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightControlWeb.DTO
{
    // A version of flight that are valid
    public class FlightPlanDTO
    {
        public int Passengers { get; set; }

        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        [JsonProperty("initial_location")]
        public virtual InitialLocationDTO InitialLocation { get; set; }

        public LocationDTO[] Segments { get; set; }

}
}
